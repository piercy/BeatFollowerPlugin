using System.ComponentModel;
using System.Runtime.CompilerServices;
using BeatFollower.Models;
using BeatFollower.Services;
using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;
using SiraUtil.Logging;

namespace BeatFollower.UI
{
	internal class FollowerListObject : INotifyPropertyChanged
	{
		private readonly SiraLog _siraLog;
		private readonly PlaylistService _playlistService;

		public FollowerListObject(SiraLog siraLog, PlaylistService playlistService, Follower follower)
		{
			_siraLog = siraLog;
			_playlistService = playlistService;

			name = follower.Twitch;
			profileImageUrl = follower.ProfileImageUrl;
			ShowDownloadButton = !follower.RecommendedPlaylistInstalled;
			ShowUpdateButton = follower.RecommendedPlaylistInstalled;

			if (follower.RecommendedPlaylistInstalled)
			{
				recommendedCount = follower.RecommendedPlaylistCount + "/" + follower.RecommendedPlaylistWebsiteCount;
			}
			else
			{
				recommendedCount = follower.RecommendedPlaylistWebsiteCount.ToString();
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
			_siraLog.Debug("Download/Update Pressed.");

			DownloadInteractable = false;
			ShowUpdateButton = false;
			ShowDownloadButton = false;
			ShowTickButton = true;
			_playlistService.DownloadPlaylist(name);
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}