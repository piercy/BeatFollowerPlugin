using System.Runtime.CompilerServices;
using BeatFollower.Services;
using BeatFollower.Utilities;
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
            SetImage(imageUrl);
            _playlistService = new PlaylistService();
            
        }

        public void SetImage(string url)
        {
            SharedCoroutineStarter.instance.StartCoroutine(LoadScripts.LoadSpriteCoroutine(url, (profileImage) =>
            {
                image.texture = profileImage;
                image.color = Color.white;
            }));

        }
        [UIValue("follower-name")]
        string name;


        [UIComponent("follower-image")]
        private RawImage image;

        private PlaylistService _playlistService;

        [UIAction("follow-download-pressed")]
        protected void FollowDownloadPressed()
        {
            Logger.log.Debug("Download Pressed.");
            _playlistService.DownloadPlaylist(this.name);
        }

    }
}