namespace BeatFollower.Services
{
    internal class LastBeatmapManager
    {
	    public IDifficultyBeatmap LastBeatmap { get; set; } = null!;
    }
}