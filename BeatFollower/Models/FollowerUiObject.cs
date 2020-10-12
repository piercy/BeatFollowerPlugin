using BeatFollower.Services;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;

namespace BeatFollower.Models
{
    public class FollowerUiObject : BSMLAutomaticViewController
    {
        private PlaylistService _playlistService;

        public FollowerUiObject(string name, string imageUrl)
        {
            this.name = name;
            this.profileImageUrl = imageUrl;
            _playlistService = new PlaylistService();
            
        }

        [UIValue("follower-name")]
        string name;

        [UIValue("follower-profile-image")]
        string profileImageUrl;

        [UIValue("download-interactable")]
        bool downloadInteractable = true;

        [UIAction("follow-download-pressed")]
        protected void FollowDownloadPressed()
        {
            Logger.log.Debug("Download Pressed.");

            downloadInteractable = false;
            NotifyPropertyChanged(nameof(downloadInteractable));
            //  _playlistService.DownloadPlaylist(this.name);
        }

        

    }
}