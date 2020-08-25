namespace BeatFollower.Models
{
    public class Follower {
        public string Twitch { get; set; }
        public string ProfileImageUrl { get; set; }

        public Follower(string twitch, string profileImageUrl)
        {
            Twitch = twitch;
            ProfileImageUrl = profileImageUrl;
        }
    }
}