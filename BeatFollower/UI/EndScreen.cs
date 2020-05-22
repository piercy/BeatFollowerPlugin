using System;
using System.Linq;
using System.Reflection;
using BeatFollower.Services;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using UnityEngine;

namespace BeatFollower.UI
{
    public class EndScreen : MonoBehaviour
    {
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

        public void Setup()
        {
            try
            {
                var resultsView = Resources.FindObjectsOfTypeAll<ResultsViewController>().FirstOrDefault();
                if (!resultsView) return;
                var param = BSMLParser.instance.Parse(BeatSaberMarkupLanguage.Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), "BeatFollower.UI.EndScreen.bsml"), resultsView.gameObject, this);
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex);
            }
        }



    }
}
