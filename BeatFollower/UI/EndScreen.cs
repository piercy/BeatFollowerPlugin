using System;
using System.Linq;
using System.Reflection;
using BeatFollower.Services;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using TMPro;
using UnityEngine;

namespace BeatFollower.UI
{
    internal class EndScreen : PersistentSingleton<EndScreen>
    {
        public static IPA.Logging.Logger log;
        private BeatFollowerService _beatFollowerService;
        public IBeatmapLevel LastSong { get; set; }

        [UIAction("recommend-pressed")]
        private void RecommendPressed()
        {
            Logger.log.Debug("Recommend Pressed.");
            if(_beatFollowerService == null)
                _beatFollowerService = new BeatFollowerService();
            Logger.log.Debug("Got Service.");

            _beatFollowerService.SubmitRecommendation(LastSong);
            
        }

        internal void Setup()
        {
            try
            {
                var resultsView = Resources.FindObjectsOfTypeAll<ResultsViewController>().FirstOrDefault();
                if (!resultsView) return;
                BSMLParser.instance.Parse(
                    BeatSaberMarkupLanguage.Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), "BeatFollower.UI.EndScreen.bsml"),
                    resultsView.gameObject, this);
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex);
            }
        }



    }
}
