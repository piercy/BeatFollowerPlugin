using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BeatFollower.Models;
using BeatFollower.Services;
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
        private BeatFollowerService _beatFollowerService;

        [UIComponent("follower-list")]
        public CustomCellListTableData followerList;

        [UIValue("followers")]
        public List<object> followersUiList = new List<object>();


        public BeatFollowerViewController(BeatFollowerService beatFollowerService)
        {
            _beatFollowerService = beatFollowerService;
        }

        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
            base.DidActivate(firstActivation, activationType);
            Logger.log.Debug("IN VC");
            if (firstActivation)
            {
                Logger.log.Debug("Called BeatFollowerService");
                if(_beatFollowerService == null)
                    Logger.log.Debug("BeatFollowerService is null");

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