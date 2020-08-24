using Zenject;
using BeatFollower.Models;
using BeatFollower.Services;
using System.Collections.Generic;
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

        private FollowService _followService;

        [Inject]
        public void Construct(FollowService followService)
        {
            _followService = followService;
        }

        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
            base.DidActivate(firstActivation, activationType);
            Logger.log.Debug("IN VC");
            if (firstActivation)
            {
                _followService.GetFollowing(SetFollowers);
            }
        }
        public void SetFollowers(List<Follower> followers)
        {
            Logger.log.Debug("Called SetFollowers");
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