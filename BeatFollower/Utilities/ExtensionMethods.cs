namespace BeatFollower.Utilities
{
    public static class ExtensionMethods
    {
        public static bool IsWip(this BeatmapLevel level)
        {
            return level.levelID.EndsWith("WIP");
        }
    }
}
