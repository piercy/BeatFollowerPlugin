using System;
using BeatFollower.Models;
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

        [Init]
        public void Init(object nullObject, IPA.Logging.Logger logger)
        {
            log = logger;
        }

        [OnStart]
        public void OnApplicationStart()
        {
            log.Debug("OnApplicationStart");

            BS_Utils.Utilities.Config config = new BS_Utils.Utilities.Config(Name);

            _apiKey = config.GetString(Name, "ApiKey");
            _apiUrl = config.GetString(Name, "ApiUrl");

            // Set defaults
            if (string.IsNullOrEmpty(_apiUrl))
                config.SetString(Name, "ApiUrl", "http://direct.beatfollower.com:3000");

            if (string.IsNullOrEmpty(_apiKey))
                config.SetString(Name, "ApiKey", Guid.Empty.ToString());

            log.Debug($"ApiKey: {_apiKey}");
            log.Debug($"ApiUrl: {_apiUrl}");

            BS_Utils.Plugin.LevelDidFinishEvent += PluginOnLevelDidFinishEvent;
        }

        private void PluginOnLevelDidFinishEvent(StandardLevelScenesTransitionSetupDataSO levelscenestransitionsetupdataso, LevelCompletionResults levelcompletionresults)
        {
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
                log.Debug($"Sending Activity: {activity.Hash}:{activity.ApiKey}");
                string json = JsonConvert.SerializeObject(activity);
                log.Debug("Activity: " + json);
                UnityWebRequest activityRequest = UnityWebRequest.Post(_apiUrl + "/activity/", json);


                byte[] jsonBytes = new System.Text.UTF8Encoding().GetBytes(json);
                activityRequest.uploadHandler = new UploadHandlerRaw(jsonBytes);
                activityRequest.SetRequestHeader("Content-Type", "application/json");
                activityRequest.timeout = 30;
                activityRequest.SendWebRequest();
                log.Debug($"Activity Sent: {activity.Hash}:{_apiKey}");
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        [OnExit]
        public void OnApplicationQuit()
        {
            log.Debug("OnApplicationQuit");
        }
    }
}
