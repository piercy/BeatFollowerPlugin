using System;
using Zenject;
using System.Reflection;
using BS_Utils.Utilities;
using BeatFollower.Services;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Notify;
using System.Runtime.CompilerServices;
using BeatSaberMarkupLanguage.Attributes;

namespace BeatFollower.UI
{
    public class EndScreen : INotifiableHost, IInitializable, IDisposable
    {
        private IBeatmapLevel _lastSong;
        private bool recommendInteractable = true;
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly Config _config;
        private readonly EventService _eventService;
        private readonly BeatFollowerService _beatFollowerService;
        private readonly ResultsViewController _resultsViewController;

        public EndScreen([Inject(Id = "BeatFollower Config")] Config config, EventService eventService, BeatFollowerService beatFollowerService, ResultsViewController resultsViewController)
        {
            _resultsViewController = resultsViewController;
            _beatFollowerService = beatFollowerService;
            _eventService = eventService;
            _config = config;
            Setup();
        }

        [UIValue("recommendInteractable")]
        public bool RecommendInteractable
        {
            get => recommendInteractable;
            set
            {
                recommendInteractable = value;
                NotifyPropertyChanged();
            }
        }

        [UIAction("recommend-pressed")]
        protected void RecommendPressed()
        {
            Logger.log.Debug("Recommend Pressed.");
            _beatFollowerService.SubmitRecommendation(_lastSong);
            RecommendInteractable = false;
        }

        public void EnableRecommmendButton()
        {
            RecommendInteractable = true;
        }

        private void Setup()
        {
            try
            {
                var position = _config.GetString("BeatFollower", "Position", "BottomLeft");
                if (string.IsNullOrEmpty(position))
                    position = "BottomLeft";

                if (!_resultsViewController) return;

                // Replaces spaces to be more friendly, in case a user types "Bottom Left" rather than "BottomLeft"
                var pos = position.ToLower().Replace(" ", "");
                if (pos != "bottomleft" || pos != "bottomright" || pos != "topleft" || pos != "topright") pos = "bottomleft";
                BSMLParser.instance.Parse(BeatSaberMarkupLanguage.Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), $"BeatFollower.UI.EndScreen-{pos}.bsml"), _resultsViewController.gameObject, this);
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex);
            }
        }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            catch { }
        }

        public void Initialize()
        {
            _eventService.LevelStarted += LevelStarted;
        }

        public void Dispose()
        {
            _eventService.LevelStarted -= LevelStarted;
        }

        private void LevelStarted(IBeatmapLevel level)
        {
            EnableRecommmendButton();
            _lastSong = level;
        }
    }
}