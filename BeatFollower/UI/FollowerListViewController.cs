
using System;
using BeatFollower.Models;
using BeatFollower.Services;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;

namespace BeatFollower.UI
{
    [HotReload(@"C:\working\BeatFollowerPlugin\BeatFollower\UI\FollowerList.bsml")]
    [ViewDefinition("BeatFollower.UI.FollowerList.bsml")]
    public class FollowerListViewController : BSMLAutomaticViewController
    {

        [UIComponent("follower-list")]
        public CustomCellListTableData followerList;

        [UIValue("followers")]
        public List<object> followersUiList = new List<object>();

        private FollowService _followService;
        private PlaylistService _playlistService;
        

        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
            base.DidActivate(firstActivation, activationType);

            if (firstActivation)
            {
                if (_followService == null)
                    _followService = new FollowService();

                if(_playlistService == null)
                    _playlistService = new PlaylistService();
                
                _followService.GetFollowing(SetFollowers);


            }
            
        }

        public void SetFollowers(List<Follower> followers)
        {
            followersUiList.Clear();
            try
            {
                if (followers != null)
                {
                    // Sort the follow list to show the installed playlists first
                    foreach (var follower in followers.OrderByDescending(x => x.RecommendedPlaylistInstalled)
                        .ThenBy(x => x.Twitch).ToList())
                    {
                        followersUiList.Add(new FollowerListObject(follower));
                    }
                }
            }
            catch (Exception e)
            {
                Logger.log.Error(e);
            }

            followerList.tableView.ReloadData();
        }
    }
}