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
            ButtonInteractable = false;
            _activityService.SubmitRecommendation(SongService.LastSong, id);
        }


        private bool buttonInteractable = true;
        [UIValue("button-interactable")]
        public bool ButtonInteractable
        {
            get => buttonInteractable;
            set
            {
                buttonInteractable = value;
                NotifyPropertyChanged();
            }
        }

        public void Reset()
        {
	        ButtonInteractable = true;
        }

    }
}