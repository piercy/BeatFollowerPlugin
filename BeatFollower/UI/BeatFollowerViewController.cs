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
        private BeatFollowerService _beatFollowerService;

        [UIComponent("follower-list")]
        public CustomCellListTableData followerList;

        [UIValue("followers")]
        public List<object> followersUiList = new List<object>();

        [Inject]
        public void Construct(BeatFollowerService beatFollowerService)
        {
            _beatFollowerService = beatFollowerService;
        }

        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
            base.DidActivate(firstActivation, activationType);
            Logger.log.Debug("IN VC");
            if (firstActivation)
            {
                _beatFollowerService.GetFollowing(SetFollowers);
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