using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeatFollower.Models;
using BeatFollower.Utilities;
using Newtonsoft.Json;
using UnityEngine.Networking;

namespace BeatFollower.Services
{
    class BeatFollowerService
    {
        const string Name = "BeatFollower";
        private string defaultApiKey = "0000000-0000000-0000000-0000000";
        private string defaultApiUrl = "https://api.beatfollower.com";
        private string _apiUrl;
        private string _apiKey;
        private string _position;
        private BS_Utils.Utilities.Config _config;
        public BeatFollowerService()
        {

            _config = new BS_Utils.Utilities.Config(Name);
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
            Logger.log.Debug($"ApiKey: {_apiKey}");
            Logger.log.Debug($"ApiUrl: {_apiUrl}");
        }

        public void SubmitActivity(LevelCompletionResults.LevelEndStateType levelEndState)
        {
            try
            {
                var currentMap = BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData;
                var currentSong = currentMap.difficultyBeatmap.level;

                if (currentSong.IsWip())
                {
                    Logger.log.Debug("WIP so not sending activity.");
                    return;
                }

                Logger.log.Debug(currentMap.difficultyBeatmap.beatmapData.beatmapEventData.ToString());
                Logger.log.Debug(BS_Utils.Gameplay.Gamemode.GameMode);
                Logger.log.Debug("ss:" + BS_Utils.Gameplay.ScoreSubmission.Disabled.ToString());
                Logger.log.Debug("ss:" + BS_Utils.Gameplay.ScoreSubmission.LastDisablers.FirstOrDefault());
                Logger.log.Debug("sr:" + BS_Utils.Gameplay.ScoreSubmission.LastDisabledModString.ToString());
                

                Logger.log.Debug(BS_Utils.Gameplay.Gamemode.SelectedCharacteristic.requires360Movement.ToString());
                var customLevel = true;

                if (!(currentSong is CustomBeatmapLevel))
                {
                    customLevel = false; // OST Level
                    Logger.log.Debug("OST Level");
                }

                var activity = new Activity();

                if (customLevel)
                    activity.Hash =
                        SongCore.Utilities.Hashing.GetCustomLevelHash(currentSong as CustomPreviewBeatmapLevel);
                else
                {
                    activity.Hash = currentSong.levelID;
                    activity.Ost = true;
                }

                activity.Is90 = BS_Utils.Gameplay.Gamemode.GameMode.Equals("90Degree");
                activity.Is360 = (BS_Utils.Gameplay.Gamemode.GameMode.Equals("360Degree") || currentMap.difficultyBeatmap.beatmapData.spawnRotationEventsCount > 0) && !activity.Is90;
                activity.OneSaber = BS_Utils.Gameplay.Gamemode.GameMode.Equals("OneSaber");
                activity.NoFail = currentMap.gameplayModifiers.noFail;
                activity.WipMap = currentSong.levelID.EndsWith("WIP");
                activity.PracticeMode = currentMap.practiceSettings != null;
                activity.Difficulty = currentMap.difficultyBeatmap.difficulty;
                activity.EndType = levelEndState;
                activity.SongName = currentSong.songName;
                activity.SongSubName = currentSong.songSubName;
                activity.SongAuthorName = currentSong.songAuthorName;
                activity.LevelAuthorName = currentSong.levelAuthorName;
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


    }
   
    
}
