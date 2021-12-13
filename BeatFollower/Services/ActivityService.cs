using System;
using Newtonsoft.Json;
using System.Globalization;
using System.Linq;
using BeatFollower.Models;
using BeatFollower.Utilities;
using UnityEngine;

namespace BeatFollower.Services
{
    public class ActivityService
    {
        private RequestService _requestService;


        public ActivityService()
        {
            _requestService = new RequestService();
        }

        public void SubmitActivity(LevelCompletionResults levelCompletionResults)
        {
            try
            {

                LevelCompletionResults.LevelEndStateType levelEndState = levelCompletionResults.levelEndStateType;
                var currentMap = BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData;
                var currentSong = currentMap.difficultyBeatmap.level;

                if (currentSong.IsWip())
                {
                    Logger.log.Debug("WIP so not sending activity.");
                    return;
                }

                var scoreController = Resources.FindObjectsOfTypeAll<ScoreController>().LastOrDefault(x => x.isActiveAndEnabled);
                var modifiedScore = ScoreModel.GetModifiedScoreForGameplayModifiersScoreMultiplier(levelCompletionResults.rawScore, scoreController.gameplayModifiersScoreMultiplier);
                var maxScore = scoreController.immediateMaxPossibleRawScore;
                var acc = modifiedScore / (maxScore * scoreController.gameplayModifiersScoreMultiplier);
                var normalizedAcc = (acc * 100.0f);

				var customLevel = true;

                if (!(currentSong is CustomBeatmapLevel))
                {
                    customLevel = false; // OST Level
                    Logger.log.Debug("OST Level");
                }

				Logger.log.Debug("levelCompletionResults.energy " + levelCompletionResults.energy);

				var activity = new Activity
                {
                    NoFail = (currentMap.gameplayModifiers.noFailOn0Energy || currentMap.gameplayModifiers.demoNoFail) && Math.Abs(levelCompletionResults.energy) <= 0,
                    WipMap = currentSong.levelID.EndsWith("WIP"),
                    PracticeMode = currentMap.practiceSettings != null,
                    Difficulty = currentMap.difficultyBeatmap.difficulty,
                    EndType = levelEndState,
                    SongName = currentSong.songName,
                    SongSubName = currentSong.songSubName,
                    SongAuthorName = currentSong.songAuthorName,
                    LevelAuthorName = currentSong.levelAuthorName,
                    FullCombo = levelCompletionResults.fullCombo,
                    EndSongTime = levelCompletionResults.endSongTime.ToString("##.##", new CultureInfo("en-US")),
                    SongDuration = levelCompletionResults.songDuration.ToString("##.##", new CultureInfo("en-US")),
                    Accuracy = normalizedAcc.ToString("##.##",  new CultureInfo("en-US")),
                    Is90 = currentMap.difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.serializedName.Equals("90Degree")
                };

                if (activity.Accuracy == "NaN")
                {
	                Logger.log.Debug("modified score: " + modifiedScore);
	                Logger.log.Debug("maxScore: " + maxScore);
	                Logger.log.Debug("gameplayModifiersScoreMultiplier: " + scoreController.gameplayModifiersScoreMultiplier);
	                Logger.log.Debug("acc: " + acc);
                }

				// cant be 90 and 360 at the same time, so if weve already detected 90, cant be 360, this is a best guess effort
				activity.Is360 = (currentMap.difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.serializedName.Equals("360Degree") || currentMap.difficultyBeatmap.beatmapData.spawnRotationEventsCount > 0) && !activity.Is90;
                activity.OneSaber = currentMap.difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.serializedName.Equals("OneSaber");

                if (customLevel)
                {
                    activity.Hash =
                        SongCore.Utilities.Hashing.GetCustomLevelHash(currentSong as CustomPreviewBeatmapLevel);
                }
                else
                {
                    activity.Hash = currentSong.levelID;
                    activity.Ost = true;
                }


                string json = JsonConvert.SerializeObject(activity);

                SharedCoroutineStarter.instance.StartCoroutine(_requestService.Post("activity/", json));
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex);
            }
        }
        public void SubmitRecommendation(IBeatmapLevel level, string listId = "")
        {
            Logger.log.Debug("Submitting Level");
            try
            {
                if (level.IsWip())
                {
                    Logger.log.Debug("WIP so not sending recommendation.");
                    return;
                }

                var recommendation = new Recommendation();

                recommendation.Hash = SongCore.Utilities.Hashing.GetCustomLevelHash(level as CustomPreviewBeatmapLevel);
                recommendation.SongName = level.songName;
                recommendation.SongSubName = level.songSubName;
                recommendation.SongAuthorName = level.songAuthorName;
                recommendation.LevelAuthorName = level.levelAuthorName;

                string json = JsonConvert.SerializeObject(recommendation);

                SharedCoroutineStarter.instance.StartCoroutine(_requestService.Post("recommendation/" + listId, json));
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex);
            }
        }
    }
}
