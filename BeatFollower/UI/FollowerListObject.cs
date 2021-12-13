using BeatFollower.Services;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;

namespace BeatFollower.Models
{
    public class FollowerListObject : BSMLAutomaticViewController
    {
        private PlaylistService _playlistService;

        public FollowerListObject(Follower follower)
        {
            _playlistService = new PlaylistService();

            this.name = follower.Twitch;
            this.profileImageUrl = follower.ProfileImageUrl;
            ShowDownloadButton = !follower.RecommendedPlaylistInstalled;
            ShowUpdateButton = follower.RecommendedPlaylistInstalled;
            if (follower.RecommendedPlaylistInstalled)
            {
                this.recommendedCount = follower.RecommendedPlaylistCount + "/" + follower.RecommendedPlaylistWebsiteCount;
            }
            else
            {
                this.recommendedCount = follower.RecommendedPlaylistWebsiteCount.ToString();
            }
        }

        [UIValue("follower-name")]
        string name;
        [UIValue("recommended-count")]
        string recommendedCount;

        [UIValue("follower-profile-image")]
        string profileImageUrl;

        private bool showDownloadButton = true;
        [UIValue("show-download")]
        public bool ShowDownloadButton
        {
            get => showDownloadButton;
            set
            {
                showDownloadButton = value;
                NotifyPropertyChanged();
            }
        }

        private bool showUpdateButton = false;
        [UIValue("show-update")]
        public bool ShowUpdateButton
        {
            get => showUpdateButton;
            set
            {
                showUpdateButton = value;
                NotifyPropertyChanged();
            }
        }

        private bool showTickButton = false;
        [UIValue("show-tick")]
        public bool ShowTickButton
        {
            get => showTickButton;
            set
            {
                showTickButton = value;
                NotifyPropertyChanged();
            }
        }

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
            Logger.log.Debug("Download/Update Pressed.");

            DownloadInteractable = false;
            ShowUpdateButton = false;
            ShowDownloadButton = false;
            ShowTickButton = true;
            _playlistService.DownloadPlaylist(this.name);
        }

        

    }
}