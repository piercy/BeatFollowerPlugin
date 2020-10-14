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

                var position = _config.GetString(Name, "Position", "BottomLeft");
                if (string.IsNullOrEmpty(position))
                    position = "BottomLeft";

                //var resultsView = Resources.FindObjectsOfTypeAll<ResultsViewController>().FirstOrDefault();
                //if (!resultsView) return;

                //// Replaces spaces to be more friendly, in case a user types "Bottom Left" rather than "BottomLeft"
                //switch (position.ToLower().Replace(" ", ""))
                //{
                //    case "topleft":
                //        BSMLParser.instance.Parse(BeatSaberMarkupLanguage.Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), "BeatFollower.UI.EndScreen-TopLeft.bsml"), resultsView.gameObject, this);
                //        break;
                //    case "topright":
                //        BSMLParser.instance.Parse(BeatSaberMarkupLanguage.Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), "BeatFollower.UI.EndScreen-TopRight.bsml"), resultsView.gameObject, this);
                //        break;
                //    case "bottomright":
                //        BSMLParser.instance.Parse(BeatSaberMarkupLanguage.Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), "BeatFollower.UI.EndScreen-BottomRight.bsml"), resultsView.gameObject, this);
                //        break;
                //    case "bottomleft":
                //        BSMLParser.instance.Parse(BeatSaberMarkupLanguage.Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), "BeatFollower.UI.EndScreen-BottomLeft.bsml"), resultsView.gameObject, this);
                //        break;
                //    default: // opted for duplication for clarity and future proofing
                //        BSMLParser.instance.Parse(BeatSaberMarkupLanguage.Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), "BeatFollower.UI.EndScreen-BottomLeft.bsml"), resultsView.gameObject, this);
                //        break;

                //}
               
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
