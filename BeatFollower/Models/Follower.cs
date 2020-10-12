using BeatFollower.Services;

namespace BeatFollower.Models
{
    public class Follower {
        private PlaylistService _playlistService;
        public string Twitch { get; set; }
        public string ProfileImageUrl { get; set; }

        public bool RecommendedPlaylistInstalled { get; set; }

        public int RecommendedPlaylistCount { get; set; }

        public int RecommendedPlaylistWebsiteCount { get; set; }

        public Follower(string twitch, string profileImageUrl)
        {
            _playlistService = new PlaylistService();

            Twitch = twitch;
            ProfileImageUrl = profileImageUrl;
            RecommendedPlaylistInstalled = _playlistService.DoesPlaylistExist(twitch);
            RecommendedPlaylistCount = _playlistService.GetSongCount(twitch);
        }
    }
}