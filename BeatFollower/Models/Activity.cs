using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatFollower.Models
{
    public class Activity
    {
        public string Hash { get; set; }
        public bool PracticeMode { get; set; }
        public bool NoFail { get; set; }
        public bool WipMap { get; set; }
        public BeatmapDifficulty Difficulty { get; set; }
        public LevelCompletionResults.LevelEndStateType EndType { get; set; }
        public string SongName { get; set; }
        public string SongSubName { get; set; }
        public string SongAuthorName { get; set; }
        public string LevelAuthorName { get; set; }
        public bool Ost { get; set; }
        public bool Is360 { get; set; }
        public bool Is90 { get; set; }
        public bool OneSaber { get; set; }
        public bool FullCombo { get; set; }
        public string EndSongTime { get; set; }
        public string Accuracy { get; set; }
        public string SongDuration { get; set; }
    }
}
