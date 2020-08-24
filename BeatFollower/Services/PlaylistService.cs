using System;
using System.IO;
using BS_Utils.Utilities;
using Zenject;

namespace BeatFollower.Services
{
    public class PlaylistService : IInitializable, IDisposable
    {
        private RequestService _requestService;

        public PlaylistService(RequestService requestService)
        {
            _requestService = requestService;
        }

        public void Initialize()
        {
            
        }

        public void Dispose()
        {
            
        }

        public void DownloadPlaylist()
        {
            Logger.log.Debug("Requesting playlist");
            if(_requestService == null)
                Logger.log.Debug("Requesting is null");

            _requestService.Get("feed/playlist/recommended/PiercyTTV/BeatFollowerRecommended-PiercyTTV.json", playlistJson =>
                {
                    Logger.log.Debug("PL: " + playlistJson.Length);
                    // BeatFollowerRecommended-PiercyTTV.json
                    var filePath = @"G:\SteamLibrary\steamapps\common\Beat Saber\Playlists\" +
                                   "BeatFollowerRecommended-PiercyTTV.json";

                    File.WriteAllText(filePath, playlistJson);
                    Logger.log.Debug("LOADING");
                    PlaylistLoaderLite.LoadPlaylistScript.load();
                });
            
            
        }


    }
}