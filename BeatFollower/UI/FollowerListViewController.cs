using System;
using BeatFollower.Models;
using BeatFollower.Services;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using SiraUtil.Logging;
using Zenject;

namespace BeatFollower.UI
{
    [HotReload(RelativePathToLayout = @"Views\FollowerList.bsml")]
    [ViewDefinition("BeatFollower.UI.Views.FollowerList.bsml")]
    internal class FollowerListViewController : BSMLAutomaticViewController
    {
	    private DiContainer _container = null!;
	    private SiraLog _siraLog = null!;
	    private FollowService _followService = null!;

        [Inject]
        internal void Construct(DiContainer container, SiraLog siraLog, FollowService followService)
        {
	        _container = container;
	        _siraLog = siraLog;
	        _followService = followService;
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);

            if (firstActivation)
            {
            }

            _followService.GetFollowing(SetFollowers);
        }

        public void SetFollowers(List<Follower> followers)
        {
            followersUiList.Clear();
            try
            {
	            // Sort the follow list to show the installed playlists first
	            foreach (var follower in followers.OrderByDescending(x => x.RecommendedPlaylistInstalled).ThenBy(x => x.Twitch).ToList())
	            {
		            var followerListObject = _container.Instantiate<FollowerListObject>(new[] { follower });
		            followersUiList.Add(followerListObject);
	            }
            }
            catch (Exception e)
            {
                _siraLog.Error(e);
            }

            followerList.tableView.ReloadData();
        }

        [UIComponent("follower-list")]
        public CustomCellListTableData followerList;

        [UIValue("followers")]
        public List<object> followersUiList = new();
    }
}