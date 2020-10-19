using BeatFollower.UI;
using HarmonyLib;

namespace BeatFollower.HarmonyPatches
{
    [HarmonyPatch(typeof(ResultsViewController), "SetDataToUI")]
    class ResultsViewControllerPatches
    {
        static void Postfix(ref ResultsViewController __instance)
        {
            // Create end of song UI
            EndLevelUiCreator.Create();
        }
    }
}