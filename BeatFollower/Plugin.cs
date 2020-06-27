using System;
using System.Collections;
using System.Collections.Generic;
using BeatFollower.Models;
using BeatFollower.Services;
using BeatFollower.UI;
using BS_Utils.Utilities;
using IPA;
using IPA.Config;
using Newtonsoft.Json;
using UnityEngine.Networking;


namespace BeatFollower
{

    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        internal static Plugin instance { get; private set; }
        internal static string Name => "BeatFollower";

        private BeatFollowerService _beatFollowerService;
        private EndScreen _endScreen;
        [Init]
        public void Init(object nullObject, IPA.Logging.Logger logger)
        {
            Logger.log = logger;
        }

        [OnStart]
        public void OnApplicationStart()
        {
            Logger.log.Debug("OnApplicationStart");
         
        }

        [OnEnable]
        public void OnEnable()
        {
            try
            {
                Logger.log.Debug("BeatFollower Enabled");
                _beatFollowerService = new BeatFollowerService();
                _endScreen = new EndScreen();
                BS_Utils.Utilities.BSEvents.earlyMenuSceneLoadedFresh += BSEventsOnearlyMenuSceneLoadedFresh;
                BS_Utils.Plugin.LevelDidFinishEvent += PluginOnLevelDidFinishEvent;
                BS_Utils.Utilities.BSEvents.gameSceneLoaded += BSEvents_gameSceneLoaded;
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex);
            }
        }

        [OnDisable]
        public void OnDisable()
        {
            try
            {
                Logger.log.Debug("BeatFollower Disabled");
                _beatFollowerService = null;
                _endScreen = null;
                BS_Utils.Utilities.BSEvents.earlyMenuSceneLoadedFresh -= BSEventsOnearlyMenuSceneLoadedFresh;
                BS_Utils.Plugin.LevelDidFinishEvent -= PluginOnLevelDidFinishEvent;
                BS_Utils.Utilities.BSEvents.gameSceneLoaded -= BSEvents_gameSceneLoaded;
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex);
            }
        }

        private void BSEventsOnearlyMenuSceneLoadedFresh(ScenesTransitionSetupDataSO obj)
        {
            // MOD CHECKING EXAMPLE
            //#pragma warning disable CS0618 // remove PluginManager.Plugins is obsolete warning
            //SongBrowserTweaks.ModLoaded = IPAPluginManager.GetPluginFromId("SongBrowser") != null || IPAPluginManager.GetPlugin("Song Browser") != null || IPAPluginManager.Plugins.Any(x => x.Name == "Song Browser");
            //SongDataCoreTweaks.ModLoaded = IPAPluginManager.GetPluginFromId("SongDataCore") != null;
            //SongDataCoreTweaks.ModVersion = IPAPluginManager.GetPluginFromId("SongDataCore")?.Version;
            //BeatSaverVotingTweaks.ModLoaded = IPAPluginManager.GetPluginFromId("BeatSaverVoting") != null;
            //#pragma warning restore CS0618

            Logger.log.Debug("Start Setup");
            _endScreen.Setup();
        }

        private void BSEvents_gameSceneLoaded()
        {
            _endScreen.EnableRecommmendButton();
            _endScreen.LastSong = BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.difficultyBeatmap.level;
        }

        private void PluginOnLevelDidFinishEvent(StandardLevelScenesTransitionSetupDataSO levelscenestransitionsetupdataso, LevelCompletionResults levelcompletionresults)
        {
            _beatFollowerService.SubmitActivity(levelcompletionresults);
        }

        [OnExit]
        public void OnApplicationQuit()
        {
            Logger.log.Debug("OnApplicationQuit");
        }
    }
}
