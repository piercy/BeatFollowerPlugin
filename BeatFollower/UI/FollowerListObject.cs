using BeatFollower.Services;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;

namespace BeatFollower.Models
{
    public class FollowerListObject : BSMLAutomaticViewController
    {
        private PlaylistService _playlistService;

        public FollowerListObject(string name, string imageUrl)
        {
            this.name = name;
            this.profileImageUrl = imageUrl;
            _playlistService = new PlaylistService();
            
        }

        [UIValue("follower-name")]
        string name;

        [UIValue("follower-profile-image")]
        string profileImageUrl;

      
        private bool downloadInteractable = true;

        [UIValue("download-interactable")]
        public bool DownloadInteractable
        {
            get => downloadInteractable;
            set {
                downloadInteractable = value;
                NotifyPropertyChanged(); 
            }
        }

        [UIAction("follow-download-pressed")]
        protected void FollowDownloadPressed()
        {
            Logger.log.Debug("Download Pressed.1");

            DownloadInteractable = false;
            _playlistService.DownloadPlaylist(this.name);
        }

        

    }
}