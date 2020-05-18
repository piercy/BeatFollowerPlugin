using System;
using System.Collections;
using System.Collections.Generic;
using BeatFollower.Models;
using BeatFollower.Services;
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

        private BeatFollowerService _beatFollowerService;
        [Init]
        public void Init(object nullObject, IPA.Logging.Logger logger)
        {
            Logger.log = logger;
        }

        [OnStart]
        public void OnApplicationStart()
        {
            Logger.log.Debug("OnApplicationStart");
            _beatFollowerService = new BeatFollowerService();
       

            BS_Utils.Utilities.BSEvents.earlyMenuSceneLoadedFresh += BSEventsOnearlyMenuSceneLoadedFresh; 
            BS_Utils.Plugin.LevelDidFinishEvent += PluginOnLevelDidFinishEvent;
            BS_Utils.Utilities.BSEvents.gameSceneLoaded += BSEvents_gameSceneLoaded;
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
            UI.EndScreen.instance.Setup();
        }

        private void BSEvents_gameSceneLoaded()
        {
            UI.EndScreen.instance.LastSong = BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.difficultyBeatmap.level;
        }

        private void PluginOnLevelDidFinishEvent(StandardLevelScenesTransitionSetupDataSO levelscenestransitionsetupdataso, LevelCompletionResults levelcompletionresults)
        {
            _beatFollowerService.SubmitActivity(levelcompletionresults.levelEndStateType);
        }

        [OnExit]
        public void OnApplicationQuit()
        {
            Logger.log.Debug("OnApplicationQuit");
        }
    }
}
