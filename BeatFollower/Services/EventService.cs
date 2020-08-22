using System;
using Zenject;

namespace BeatFollower.Services
{
    public class EventService : IInitializable, IDisposable
    {
        private readonly GameScenesManager _gameScenesManager;

        public event Action<IBeatmapLevel> LevelStarted;
        public event Action<LevelCompletionResults> LevelFinished;

        public EventService(GameScenesManager gameScenesManager)
        {
            _gameScenesManager = gameScenesManager;
        }

        private void GameScenesManager_transitionDidFinishEvent(ScenesTransitionSetupDataSO _, DiContainer container)
        {
            IDifficultyBeatmap beatmap = container.TryResolve<IDifficultyBeatmap>();
            if (beatmap != null)
            {
                LevelStarted?.Invoke(beatmap.level);
            }
        }

        public void Initialize()
        {
            BS_Utils.Plugin.LevelDidFinishEvent += OnLevelDidFinishEvent;
            _gameScenesManager.transitionDidFinishEvent += GameScenesManager_transitionDidFinishEvent;
        }

        public void Dispose()
        {
            BS_Utils.Plugin.LevelDidFinishEvent += OnLevelDidFinishEvent;
            _gameScenesManager.transitionDidFinishEvent -= GameScenesManager_transitionDidFinishEvent;
        }

        private void OnLevelDidFinishEvent(StandardLevelScenesTransitionSetupDataSO _, LevelCompletionResults levelCompletionResults)
        {
            LevelFinished?.Invoke(levelCompletionResults);
        }
    }
}