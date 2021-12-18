using System;
using System.Collections.Generic;
using BeatFollower.Models;
using Newtonsoft.Json;
using SiraUtil.Logging;

namespace BeatFollower.Services
{
	internal class FollowService
	{
		private readonly SiraLog _siraLog;
		private readonly RequestService _requestService;
		private readonly PlaylistService _playlistService;

		public FollowService(SiraLog siraLog, RequestService requestService, PlaylistService playlistService)
		{
			_siraLog = siraLog;
			_requestService = requestService;
			_playlistService = playlistService;
		}

		public void GetFollowing(Action<List<Follower>> callback)
		{
			_siraLog.Debug("Called GetFollowing");
			SharedCoroutineStarter.instance.StartCoroutine(_requestService.Get("following", response =>
			{
				var following = JsonConvert.DeserializeObject<List<Follower>>(response);
				foreach (var follower in following)
				{
					follower.RecommendedPlaylistInstalled = _playlistService.DoesPlaylistExist(follower.Twitch);
					follower.RecommendedPlaylistCount = _playlistService.GetSongCount(follower.Twitch);
				}

				callback(following);
			}));
		}
	}
}