using System;
using System.Collections;
using System.Collections.Generic;
using BeatFollower.Models;
using BS_Utils.Utilities;
using IPA;
using IPA.Config;
using Newtonsoft.Json;
using UnityEngine.Networking;


namespace BeatFollower
{

    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static Plugin instance { get; private set; }
        internal static string Name => "BeatFollower";

        public static IPA.Logging.Logger log;

        private string _apiUrl;
        private string _apiKey;
        private string defaultApiKey = "0000000-0000000-0000000-0000000";
        private BS_Utils.Utilities.Config _config;

        [Init]
        public void Init(object nullObject, IPA.Logging.Logger logger)
        {
            log = logger;
        }

        [OnStart]
        public void OnApplicationStart()
        {
            log.Debug("OnApplicationStart");

            
            _config = new BS_Utils.Utilities.Config(Name);

            _apiKey = _config.GetString(Name, "ApiKey");
            _apiUrl = _config.GetString(Name, "ApiUrl");

            // Set defaults
            if (string.IsNullOrEmpty(_apiUrl))
                _config.SetString(Name, "ApiUrl", "http://direct.beatfollower.com:3000");

            if (string.IsNullOrEmpty(_apiKey))
                _config.SetString(Name, "ApiKey", defaultApiKey);

            if (!_apiUrl.EndsWith("/"))
            {
                _apiUrl += "/";
            }

            log.Debug($"ApiKey: {_apiKey}");
            log.Debug($"ApiUrl: {_apiUrl}");

            BS_Utils.Plugin.LevelDidFinishEvent += PluginOnLevelDidFinishEvent;
            BSEvents.gameSceneLoaded += OnGameSceneLoaded;
        }

        private void OnGameSceneLoaded()
        {
            log.Debug("getting key..");
            _apiKey = _config.GetString(Name, "ApiKey");


        }

        private void PluginOnLevelDidFinishEvent(StandardLevelScenesTransitionSetupDataSO levelscenestransitionsetupdataso, LevelCompletionResults levelcompletionresults)
        {

            _apiKey = _config.GetString(Name, "ApiKey");
            if (string.IsNullOrEmpty(_apiKey) || _apiKey == defaultApiKey)
            {
                log.Debug("API Key is either default or empty");
                return;
            }

            SubmitActivity(levelcompletionresults);
        }

        private void SubmitActivity(LevelCompletionResults levelcompletionresults)
        {
            try
            {
                
                var currentMap = BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData;
                var currentSong = currentMap.difficultyBeatmap.level;
                if (currentSong.levelID.EndsWith("WIP"))
                {
                    log.Debug("WIP so not recording activity.");
                    return;
                }

                var activity = new Activity();
                activity.ApiKey = _apiKey;

                activity.Hash = SongCore.Utilities.Hashing.GetCustomLevelHash(currentSong as CustomPreviewBeatmapLevel);
                activity.NoFail = currentMap.gameplayModifiers.noFail;
                activity.WipMap = currentSong.levelID.EndsWith("WIP");
                activity.PracticeMode = currentMap.practiceSettings != null;
                activity.Difficulty = currentMap.difficultyBeatmap.difficulty;
                activity.EndType = levelcompletionresults.levelEndStateType;
                activity.SongName = currentSong.songName;
                activity.SongSubName = currentSong.songSubName;
                activity.SongAuthorName = currentSong.songAuthorName;
                activity.LevelAuthorName = currentSong.levelAuthorName;
                log.Debug($"API: {_apiUrl}");
                log.Debug($"Sending Activity: {activity.Hash}:{activity.ApiKey}");
                string json = JsonConvert.SerializeObject(activity);


                SharedCoroutineStarter.instance.StartCoroutine(PostRequest(_apiUrl + "activity/", json));
                log.Debug($"Activity Sent: {activity.Hash}:{_apiKey}");
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
        IEnumerator PostRequest(string url, string json)
        {
            log.Debug($"POST: {url}:{json}");

            var uwr = new UnityWebRequest(url, "POST");
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");

            //Send the request then wait here until it returns
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                log.Debug("Error While Sending: " + uwr.error);
            }
            else
            {
               log.Debug("Received: " + uwr.downloadHandler.text);
            }
        }

        [OnExit]
        public void OnApplicationQuit()
        {
            log.Debug("OnApplicationQuit");
        }
    }
}
