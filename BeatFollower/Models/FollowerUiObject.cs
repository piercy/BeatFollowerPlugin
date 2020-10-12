using System.Runtime.CompilerServices;
using BeatFollower.Services;
using BeatFollower.Utilities;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

namespace BeatFollower.Models
{
    public class FollowerUiObject
    {
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

        private PlaylistService _playlistService;

        [UIAction("follow-download-pressed")]
        protected void FollowDownloadPressed()
        {
            Logger.log.Debug("Download Pressed.");
            _playlistService.DownloadPlaylist(this.name);
        }

    }
}