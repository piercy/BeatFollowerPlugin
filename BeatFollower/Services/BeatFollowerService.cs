using System;
using Zenject;
using Newtonsoft.Json;
using BS_Utils.Utilities;
using System.Collections;
using BeatFollower.Models;
using BeatFollower.Utilities;
using UnityEngine.Networking;

namespace BeatFollower.Services
{
    public class BeatFollowerService : IInitializable, IDisposable
    {
        const string Name = "BeatFollower";
        private string defaultApiKey = "0000000-0000000-0000000-0000000";
        private string defaultApiUrl = "https://api.beatfollower.com";
        private string _apiUrl;
        private string _apiKey;
        private string _position;
        private readonly Config _config;
        private readonly EventService _eventService;

        public BeatFollowerService([Inject(Id = "BeatFollower Config")] Config config, EventService eventService)
        {
            _config = config;
            _eventService = eventService;
             _position = _config.GetString(Name, "Position");
            _apiKey = _config.GetString(Name, "ApiKey");
            _apiUrl = _config.GetString(Name, "ApiUrl");

            // Clearing out the old address automatically for the testers. It will then set the default
            if (_apiUrl.StartsWith("http://direct.beatfollower.com"))
                _apiUrl = null;

            // Set defaults
            if (string.IsNullOrEmpty(_apiUrl))
            {
                _config.SetString(Name, "ApiUrl", defaultApiUrl);
                _apiUrl = defaultApiUrl;
            }

            if(string.IsNullOrEmpty(_position))
            {
                _config.SetString(Name, "Position", "BottomLeft");
            }

            if (string.IsNullOrEmpty(_apiKey))
            {
                _config.SetString(Name, "ApiKey", defaultApiKey);
            }

            if (!_apiUrl.EndsWith("/"))
            {
                _apiUrl += "/";
            }
            Logger.log.Debug($"ApiUrl: {_apiUrl}");
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

                // Calculations for acc
                var modifiedScore = levelCompletionResults.gameplayModifiersModel.GetModifiedScoreForGameplayModifiers(levelCompletionResults.rawScore, levelCompletionResults.gameplayModifiers);
                var maxScore = ScoreModel.MaxRawScoreForNumberOfNotes(currentMap.difficultyBeatmap.beatmapData.notesCount);
                var multiplier = levelCompletionResults.gameplayModifiersModel.GetTotalMultiplier(levelCompletionResults.gameplayModifiers);
                var acc = modifiedScore / (maxScore * multiplier);
                var normalizedAcc = (acc * 100.0f);


                var customLevel = true;

                if (!(currentSong is CustomBeatmapLevel))
                {
                    customLevel = false; // OST Level
                    Logger.log.Debug("OST Level");
                }

                var activity = new Activity
                {
                    NoFail = currentMap.gameplayModifiers.noFail,
                    WipMap = currentSong.levelID.EndsWith("WIP"),
                    PracticeMode = currentMap.practiceSettings != null,
                    Difficulty = currentMap.difficultyBeatmap.difficulty,
                    EndType = levelEndState,
                    SongName = currentSong.songName,
                    SongSubName = currentSong.songSubName,
                    SongAuthorName = currentSong.songAuthorName,
                    LevelAuthorName = currentSong.levelAuthorName,
                    FullCombo = levelCompletionResults.fullCombo,
                    EndSongTime = levelCompletionResults.endSongTime.ToString("##.##"),
                    SongDuration = levelCompletionResults.songDuration.ToString("##.##"),
                    Accuracy = normalizedAcc.ToString("##.##"),
                    Is90 = BS_Utils.Gameplay.Gamemode.GameMode.Equals("90Degree")
                };
                // cant be 90 and 360 at the same time, so if weve already detected 90, cant be 360, this is a best guess effort
                activity.Is360 = (BS_Utils.Gameplay.Gamemode.GameMode.Equals("360Degree") || currentMap.difficultyBeatmap.beatmapData.spawnRotationEventsCount > 0) && !activity.Is90;
                activity.OneSaber = BS_Utils.Gameplay.Gamemode.GameMode.Equals("OneSaber");

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

                SharedCoroutineStarter.instance.StartCoroutine(PostRequest(_apiUrl + "activity/", json));
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex);
            }
        }
        public void SubmitRecommendation(IBeatmapLevel level)
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

                SharedCoroutineStarter.instance.StartCoroutine(PostRequest(_apiUrl + "recommendation/", json));
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex);
            }
        }

        IEnumerator PostRequest(string url, string json)
        {
            Logger.log.Debug($"POST: {url}:{json}");

            _apiKey = _config.GetString(Name, "ApiKey");
            if (string.IsNullOrEmpty(_apiKey) || _apiKey == defaultApiKey)
            {
                Logger.log.Debug("API Key is either default or empty");
            }
            else
            {

                var uwr = new UnityWebRequest(url, "POST");
                uwr.SetRequestHeader("ApiKey", _apiKey);
                byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
                uwr.uploadHandler = (UploadHandler) new UploadHandlerRaw(jsonToSend);
                uwr.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
                uwr.SetRequestHeader("Content-Type", "application/json");

                //Send the request then wait here until it returns
                yield return uwr.SendWebRequest();

                if (uwr.isNetworkError || uwr.isHttpError)
                {
                    Logger.log.Debug("Error While Sending: " + uwr.error);
                }
                else
                {
                    Logger.log.Debug("Received: " + uwr.downloadHandler.text);
                }
            }
        }

        public void Initialize()
        {
            _eventService.LevelFinished += SubmitActivity;
        }

        public void Dispose()
        {
            _eventService.LevelFinished -= SubmitActivity;
        }
    }
}
