
using BeatFollower.Models;
using BeatFollower.Services;
using System.Collections.Generic;
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

        [UIAction("download-pressed")]
        protected void DownloadPressed()
        {
            Logger.log.Debug("Download Pressed.");
            _playlistService.DownloadPlaylist("piercyttv");
        }

        [UIComponent("follower-list")]
        public CustomCellListTableData followerList;

        [UIValue("followers")]
        public List<object> followersUiList = new List<object>();

        private FollowService _followService;
        private PlaylistService _playlistService;
        

        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
            base.DidActivate(firstActivation, activationType);
            Logger.log.Debug("IN VC");
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
            Logger.log.Debug("Called SetFollowers");
            followersUiList.Clear();

            if (followers != null)
            {
                foreach (var follower in followers)
                {
                    followersUiList.Add(new FollowerListObject(follower.Twitch, follower.ProfileImageUrl));
                }
            }
            
            followerList.tableView.ReloadData();
        }
    }
}