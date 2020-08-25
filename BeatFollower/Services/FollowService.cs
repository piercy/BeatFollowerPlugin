using System;
using System.Collections.Generic;
using BeatFollower.Models;
using Newtonsoft.Json;

namespace BeatFollower.Services
{
    public class FollowService
    {
        private RequestService _requestService;

        public FollowService()
        {
            _requestService = new RequestService();
        }
        public void GetFollowing(Action<List<Follower>> callback)
        {
            Logger.log.Debug("Called GetFollowing");
            SharedCoroutineStarter.instance.StartCoroutine(_requestService.Get("following", response =>
            {
                var following = JsonConvert.DeserializeObject<List<Follower>>(response);
                callback?.Invoke(following);
            }));
        }

    }
}