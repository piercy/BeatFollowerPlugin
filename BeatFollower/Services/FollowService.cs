using System;
using System.Collections.Generic;
using BeatFollower.Models;
using Newtonsoft.Json;
using Zenject;

namespace BeatFollower.Services
{
    public class FollowService : IInitializable, IDisposable
    {
        private RequestService _requestService;

        public FollowService(RequestService requestService)
        {
            _requestService = requestService;
        }
        public void GetFollowing(Action<List<Follower>> callback)
        {
            Logger.log.Debug("Called GetFollowing");
            SharedCoroutineStarter.instance.StartCoroutine(_requestService.Get("/following", response =>
            {
                var following = JsonConvert.DeserializeObject<List<Follower>>(response);
                callback?.Invoke(following);
            }));
        }

        public void Initialize()
        {
            
        }

        public void Dispose()
        {
            
        }
    }
}