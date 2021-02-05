using System;
using BeatFollower.UI;
using BS_Utils.Utilities;


namespace BeatFollower.Services
{
    public class EventService : IDisposable
    {
   

        public event Action<IBeatmapLevel> LevelStarted;
        public event Action<LevelCompletionResults> LevelFinished;
        private ActivityService _activityService;

        public void Initialize()
        {
            BS_Utils.Utilities.BSEvents.earlyMenuSceneLoadedFresh += BSEventsOnearlyMenuSceneLoadedFresh;
            BS_Utils.Utilities.BSEvents.levelCleared += BSEvents_levelLeft;
            BS_Utils.Utilities.BSEvents.levelFailed += BSEvents_levelLeft;
            BS_Utils.Utilities.BSEvents.levelQuit += BSEvents_levelLeft;
            _activityService = new ActivityService();
            
        }


        public void Dispose()
        {
            BS_Utils.Utilities.BSEvents.earlyMenuSceneLoadedFresh -= BSEventsOnearlyMenuSceneLoadedFresh;
            BS_Utils.Utilities.BSEvents.levelCleared -= BSEvents_levelLeft;
            BS_Utils.Utilities.BSEvents.levelFailed -= BSEvents_levelLeft;
            BS_Utils.Utilities.BSEvents.levelQuit -= BSEvents_levelLeft;

            _activityService = null;
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
        }


        private void BSEvents_levelLeft(StandardLevelScenesTransitionSetupDataSO arg1, LevelCompletionResults arg2)
        {
            SongService.LastSong = BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.difficultyBeatmap.level;

            _activityService.SubmitActivity(arg2);
        }

        private void PluginOnLevelDidFinishEvent(StandardLevelScenesTransitionSetupDataSO levelscenestransitionsetupdataso, LevelCompletionResults levelcompletionresults)
        {
            
        }

      
    }
}