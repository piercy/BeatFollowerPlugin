namespace BeatFollower.Models
{
	internal class Follower
	{
		public string Twitch { get; set; }
		public string ProfileImageUrl { get; set; }

		public bool RecommendedPlaylistInstalled { get; set; }

		public int RecommendedPlaylistCount { get; set; }

		public int RecommendedPlaylistWebsiteCount { get; set; }

		public Follower(string twitch, string profileImageUrl)
		{
			Twitch = twitch;
			ProfileImageUrl = profileImageUrl;
		}


	}
}