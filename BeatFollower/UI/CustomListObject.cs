using BeatFollower.Models;
using BeatFollower.Services;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;

namespace BeatFollower.UI
{
    public class CustomListObject : BSMLAutomaticViewController
    {
        public CustomListObject(CustomList customList)
        {
            this.name = customList.Name;
            id = customList._Id;
            _activityService = new ActivityService();
        }

        [UIValue("list-name")]
        string name;

        private string id;
        private ActivityService _activityService;


        [UIAction("list-share-pressed")]
        protected void ListSharePressed()
        {
            Logger.log.Debug($"Share Pressed: {name} {id}");
            _activityService.SubmitRecommendation(SongService.LastSong, id);
        }


    }
}