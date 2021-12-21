namespace BeatFollower.Models
{
    public class Recommendation
    {
        public string Hash { get; set; }
        public string SongName { get; set; }
        public string SongSubName { get; set; }
        public string SongAuthorName { get; set; }
        public string LevelAuthorName { get; set; }
        public BeatmapDifficulty Difficulty { get; set; }
        public string Characteristic { get; set; }
    }
}