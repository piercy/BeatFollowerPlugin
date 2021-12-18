using System;
using Newtonsoft.Json;
using System.Globalization;
using BeatFollower.Models;
using BeatFollower.Utilities;
using SiraUtil.Logging;
using Zenject;

namespace BeatFollower.Services
{
	internal class ActivityService : IInitializable, IDisposable
	{
		private readonly SiraLog _siraLog;
		private readonly RequestService _requestService;
		private readonly StandardLevelScenesTransitionSetupDataSO _standardLevelScenesTransitionSetupDataSo;

		public ActivityService(SiraLog siraLog, RequestService requestService, StandardLevelScenesTransitionSetupDataSO standardLevelScenesTransitionSetupDataSo)
		{
			_siraLog = siraLog;
			_requestService = requestService;
			_standardLevelScenesTransitionSetupDataSo = standardLevelScenesTransitionSetupDataSo;
		}

		public void Initialize()
		{
			_standardLevelScenesTransitionSetupDataSo.didFinishEvent += DidFinishLevelHandler;
		}

		public void Dispose()
		{
			_standardLevelScenesTransitionSetupDataSo.didFinishEvent -= DidFinishLevelHandler;
		}

		public void SubmitActivity(StandardLevelScenesTransitionSetupDataSO setupData, LevelCompletionResults levelCompletionResults)
		{
			try
			{
				var levelEndState = levelCompletionResults.levelEndStateType;
				var difficultyBeatmap = setupData.difficultyBeatmap;
				var currentSong = difficultyBeatmap.level;

				if (currentSong.IsWip())
				{
					_siraLog.Debug("WIP so not sending activity.");
					return;
				}


				var maxScore = ScoreModel.MaxRawScoreForNumberOfNotes(difficultyBeatmap.beatmapData.cuttableNotesCount);
				var normalizedAcc = ((double)levelCompletionResults.rawScore / maxScore) * 100;

				_siraLog.Debug("levelCompletionResults.energy " + levelCompletionResults.energy);

				var activity = new Activity
				{
					NoFail = (levelCompletionResults.gameplayModifiers.noFailOn0Energy || levelCompletionResults.gameplayModifiers.demoNoFail) && Math.Abs(levelCompletionResults.energy) <= 0,
					WipMap = currentSong.levelID.EndsWith("WIP"),
					PracticeMode = setupData.practiceSettings != null,
					Difficulty = difficultyBeatmap.difficulty,
					EndType = levelEndState,
					SongName = currentSong.songName,
					SongSubName = currentSong.songSubName,
					SongAuthorName = currentSong.songAuthorName,
					LevelAuthorName = currentSong.levelAuthorName,
					FullCombo = levelCompletionResults.fullCombo,
					EndSongTime = levelCompletionResults.endSongTime.ToString("##.##", new CultureInfo("en-US")),
					SongDuration = levelCompletionResults.songDuration.ToString("##.##", new CultureInfo("en-US")),
					Accuracy = normalizedAcc.ToString("##.##", new CultureInfo("en-US")),
					Is90 = difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.serializedName.Equals("90Degree")
				};

				if (activity.Accuracy == "NaN")
				{
					_siraLog.Debug("modified score: " + levelCompletionResults.rawScore);
					_siraLog.Debug("maxScore: " + maxScore);
					_siraLog.Debug("acc: " + normalizedAcc);
				}

				// cant be 90 and 360 at the same time, so if we've already detected 90, cant be 360, this is a best guess effort
				activity.Is360 = (difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.serializedName.Equals("360Degree") ||
				                  difficultyBeatmap.beatmapData.spawnRotationEventsCount > 0) && !activity.Is90;
				activity.OneSaber = difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.serializedName.Equals("OneSaber");

				if (currentSong is CustomBeatmapLevel customBeatmapLevel)
				{
					activity.Hash = SongCore.Utilities.Hashing.GetCustomLevelHash(customBeatmapLevel);
				}
				else
				{
					_siraLog.Debug("OST Level");
					activity.Hash = currentSong.levelID;
					activity.Ost = true;
				}

				var json = JsonConvert.SerializeObject(activity);

				SharedCoroutineStarter.instance.StartCoroutine(_requestService.Post("activity/", json));
			}
			catch (Exception ex)
			{
				_siraLog.Error(ex);
			}
		}

		public void SubmitRecommendation(IBeatmapLevel level, string listId = "")
		{
			_siraLog.Debug("Submitting Level");
			try
			{
				if (level.IsWip())
				{
					_siraLog.Debug("WIP so not sending recommendation.");
					return;
				}

				var recommendation = new Recommendation
				{
					Hash = SongCore.Utilities.Hashing.GetCustomLevelHash(level as CustomPreviewBeatmapLevel),
					SongName = level.songName,
					SongSubName = level.songSubName,
					SongAuthorName = level.songAuthorName,
					LevelAuthorName = level.levelAuthorName
				};

				var json = JsonConvert.SerializeObject(recommendation);

				SharedCoroutineStarter.instance.StartCoroutine(_requestService.Post("recommendation/" + listId, json));
			}
			catch (Exception ex)
			{
				_siraLog.Error(ex);
			}
		}

		private void DidFinishLevelHandler(StandardLevelScenesTransitionSetupDataSO setupData, LevelCompletionResults completionResults)
		{
			SubmitActivity(setupData, completionResults);
		}
	}
}