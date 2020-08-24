using System.Runtime.CompilerServices;
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
        private string name;


        [UIComponent("follower-image")]
        private RawImage image;

    }
}