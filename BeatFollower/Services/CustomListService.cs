using System;
using System.Collections.Generic;
using BeatFollower.Models;
using Newtonsoft.Json;

namespace BeatFollower.Services
{
    public static class CustomListService
    {
        private static RequestService _requestService;
        public static List<CustomList> CustomLists = new List<CustomList>();


        public static void GetCustomLists(Action<List<CustomList>> callback)
        {
            if(_requestService == null)
                _requestService = new RequestService();

            Logger.log.Debug("Getting Custom Lists..");
            if (CustomLists.Count > 0)
            {
                Logger.log.Debug("Custom Lists: " + CustomLists.Count);
                callback?.Invoke(CustomLists);
            }
            else
            {
                SharedCoroutineStarter.instance.StartCoroutine(_requestService.Get("lists/get", response =>
                {
                    var customlistsResponse = JsonConvert.DeserializeObject<List<CustomList>>(response);
                    CustomLists = customlistsResponse;
                    Logger.log.Debug("Custom Lists Fetched: " + CustomLists.Count);
                    callback?.Invoke(CustomLists);
                }));
            }
        }
    }
}