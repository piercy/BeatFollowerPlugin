using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
