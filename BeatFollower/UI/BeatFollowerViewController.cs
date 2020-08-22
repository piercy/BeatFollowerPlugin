using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BeatFollower.Models;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;

namespace BeatFollower.UI
{
    [HotReload(@"C:\working\BeatFollowerPlugin\BeatFollower\UI\FollowerList.bsml")]
    [ViewDefinition("BeatFollower.UI.FollowerList.bsml")]




    public class BeatFollowerViewController : BSMLAutomaticViewController // BSMLResourceViewController
    {
        [UIComponent("follower-list")]
        public CustomCellListTableData followerList;

        [UIValue("followers")]
        public List<object> followersUiList = new List<object>();

        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
            base.DidActivate(firstActivation, activationType);

            SetFollowers(new List<Follower>()
            {
                new Follower("PiercyTTV","https://static-cdn.jtvnw.net/jtv_user_pictures/cecc0819-b7df-4e7d-8db8-9695e241267c-profile_image-300x300.png"),
                new Follower("Acetari","https://static-cdn.jtvnw.net/jtv_user_pictures/45c580d5-e3e5-4900-8309-c8816b16339a-profile_image-300x300.png"),

            });
        }
        public void SetFollowers(List<Follower> followers)
        {
            followersUiList.Clear();

            if (followers != null)
            {
                foreach (var follower in followers)
                {
                    followersUiList.Add(new FollowerUiObject(follower.Twitch,follower.ProfileImageUrl ));
                }
            }
            
            followerList.tableView.ReloadData();

        }
    }
}