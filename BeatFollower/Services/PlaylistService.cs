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
                    var playlistFolderPath = $@"{Application.dataPath}\..\Playlists\";
                    var playlistFileName = $"BeatFollowerRecommended-{twitch}.json";
                    if (Directory.Exists(playlistFolderPath))
                    {
                        Logger.log.Debug("Writing Playlist: " + playlistFolderPath + playlistFileName + " Length: " + playlistJson.Length);
                        File.WriteAllText(playlistFolderPath + playlistFileName, playlistJson);
                    }

                }));
            
            
        }


    }
}