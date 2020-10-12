using System;
using System.IO;
using System.Linq;
using System.Reflection;
using BeatFollower.Models;
using BS_Utils.Utilities;
using Newtonsoft.Json;

namespace BeatFollower.Services
{
    public class PlaylistService
    {
        private RequestService _requestService;
        private string playlistFolderPath => $@"{IPA.Utilities.UnityGame.InstallPath}\Playlists\";

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
                    var playlistFileName = $"BeatFollowerRecommended-{twitch}.json";
                    if (Directory.Exists(playlistFolderPath))
                    {
                        Logger.log.Debug("Writing Playlist: " + playlistFolderPath + playlistFileName + " Length: " + playlistJson.Length);
                        File.WriteAllText(playlistFolderPath + playlistFileName, playlistJson);
                    }

                }));
        }

        public bool DoesPlaylistExist(string twitch, string playlistType = "Recommended")
        {
            var playlistFileName = $"BeatFollowerRecommended-{twitch}.json";
            var exists = File.Exists(playlistFolderPath + playlistFileName);
            Logger.log.Debug("Exists: " + exists + " " + playlistFolderPath + playlistFileName);
            return exists;
        }

        public int GetSongCount(string twitch, string playlistType = "Recommended")
        {
            var playlistFileName = $"BeatFollowerRecommended-{twitch}.json";
            var exists = File.Exists(playlistFolderPath + playlistFileName);
            if (exists)
            {
                var playlist = JsonConvert.DeserializeObject<Playlist>(File.ReadAllText(playlistFolderPath + playlistFileName));
                Logger.log.Debug(playlistFileName + " " + playlist.Songs.Count);
                return playlist.Songs.Count;
            }
            else
            {
                return 0;
            }
        }
    }
}