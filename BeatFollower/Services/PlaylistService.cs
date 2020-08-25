using System;
using System.IO;
using System.Linq;
using System.Reflection;
using BS_Utils.Utilities;
using PlaylistLoaderLite.UI;
using UnityEngine;

namespace BeatFollower.Services
{
    public class PlaylistService
    {
        private RequestService _requestService;

        public PlaylistService()
        {
            _requestService = new RequestService();
        }

        public void DownloadPlaylist(string twitch, string playlistType = "Recommended")
        {
            Logger.log.Debug("Requesting playlist");
            if(_requestService == null)
                Logger.log.Debug("Requesting is null");

            SharedCoroutineStarter.instance.StartCoroutine(_requestService.Get($"feed/playlist/{playlistType}/{twitch}/BeatFollower{playlistType}-{twitch}.json", playlistJson =>
                {
                    Logger.log.Debug("PL: " + playlistJson.Length);
                    var filePath = $@"G:\SteamLibrary\steamapps\common\Beat Saber\Playlists\BeatFollowerRecommended-{twitch}.json";

                    File.WriteAllText(filePath, playlistJson);
                    Logger.log.Debug("LOADING");

                    PluginUI obj = Resources.FindObjectsOfTypeAll<PluginUI>().FirstOrDefault<PluginUI>();
                    typeof(PluginUI).GetMethod("RefreshButtonPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(obj, null);



                }));
            
            
        }


    }
}