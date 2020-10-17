using System;
using BeatFollower.UI;


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
            BS_Utils.Plugin.LevelDidFinishEvent += PluginOnLevelDidFinishEvent;
            BS_Utils.Utilities.BSEvents.gameSceneLoaded += BSEvents_gameSceneLoaded;
            _activityService = new ActivityService();
            
        }

        public void Dispose()
        {
            BS_Utils.Utilities.BSEvents.earlyMenuSceneLoadedFresh -= BSEventsOnearlyMenuSceneLoadedFresh;
            BS_Utils.Plugin.LevelDidFinishEvent -= PluginOnLevelDidFinishEvent;
            BS_Utils.Utilities.BSEvents.gameSceneLoaded -= BSEvents_gameSceneLoaded;
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

        private void BSEvents_gameSceneLoaded()
        {
            SongService.LastSong = BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.difficultyBeatmap.level;
        }

        private void PluginOnLevelDidFinishEvent(StandardLevelScenesTransitionSetupDataSO levelscenestransitionsetupdataso, LevelCompletionResults levelcompletionresults)
        {
            _activityService.SubmitActivity(levelcompletionresults);
        }

      
    }
}