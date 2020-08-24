using System;
using BS_Utils.Utilities;
using Zenject;

namespace BeatFollower.Services
{
    public class PlaylistService : IInitializable, IDisposable
    {

        public PlaylistService()
        {
           
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void DownloadPlaylist()
        {
            //http://localhost:3000/feed/playlist/recommended/PiercyTTV/BeatFollowerRecommended-PiercyTTV.json
        }


    }
}