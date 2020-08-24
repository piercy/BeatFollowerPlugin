using Zenject;
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
    public class BeatFollowerViewController : BSMLAutomaticViewController // BSMLResourceViewController
    {
        [UIAction("download-pressed")]
        protected void DownloadPressed()
        {
            Logger.log.Debug("Download Pressed.");
            _playlistService.DownloadPlaylist();
        }
        //[UIAction("follow-selected")]
        //private void FollowSelected(TableView sender, FollowerUiObject obj)
        //{
        //    Logger.log.Debug("Selected: " + obj.Name);
        //}

        [UIComponent("follower-list")]
        public CustomCellListTableData followerList;

        [UIValue("followers")]
        public List<object> followersUiList = new List<object>();

        private FollowService _followService;
        private PlaylistService _playlistService;

        [Inject]
        public void Construct(FollowService followService, PlaylistService playlistService)
        {
            _followService = followService;
            _playlistService = playlistService;
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