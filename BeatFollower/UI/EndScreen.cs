using System;
using System.Linq;
using System.Reflection;
using BeatFollower.Services;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using UnityEngine;

namespace BeatFollower.UI
{
    public class EndScreen : NotifiableSingleton<EndScreen>
    {
        private ActivityService _activityService;
        public IBeatmapLevel LastSong { get; set; }

        const string Name = "BeatFollower";
        private BS_Utils.Utilities.Config _config;
        private bool recommendInteractable = true;
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
        private void RecommendPressed()
        {
            Logger.log.Debug("Recommend Pressed.");
            if(_activityService == null)
                _activityService = new ActivityService();

            _activityService.SubmitRecommendation(LastSong);
            RecommendInteractable = false;


        }

        public void Setup()
        {
            try
            {
                _config = new BS_Utils.Utilities.Config(Name);
                var resultsView = Resources.FindObjectsOfTypeAll<ResultsViewController>().FirstOrDefault();
                if (!resultsView) return;

                var position = _config.GetString(Name, "Position", "BottomLeft");
                var pos = position.ToLower().Replace(" ", "");
                if (pos != "bottomleft" || pos != "bottomright" || pos != "topleft" || pos != "topright") pos = "bottomleft";
                BSMLParser.instance.Parse(BeatSaberMarkupLanguage.Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), $"BeatFollower.UI.EndScreen-{pos}.bsml"), resultsView.gameObject, this);
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex);
            }
        }


        public void EnableRecommmendButton()
        {
            RecommendInteractable = true;
        }
    }
}
