namespace BeatFollower.Utilities
{
    public static class ExtensionMethods
    {
        public static bool IsWip(this IBeatmapLevel level)
        {
            return level.levelID.EndsWith("WIP");
        }
    }
}
