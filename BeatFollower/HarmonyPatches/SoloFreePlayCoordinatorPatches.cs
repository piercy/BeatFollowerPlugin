using BeatFollower.UI;
using HarmonyLib;

namespace BeatFollower.HarmonyPatches
{
	[HarmonyPatch(typeof(SoloFreePlayFlowCoordinator), "ProcessLevelCompletionResultsAfterLevelDidFinish")]
    //[HarmonyPatch("SetDataToUI", MethodType.Normal)]
    class SoloFreePlayFlowCoordinatorPatches
    {
        static void Postfix(ref SoloFreePlayFlowCoordinator __instance, LevelCompletionResults levelCompletionResults)
        {
            // Show end of song UI
            if (levelCompletionResults.levelEndAction == LevelCompletionResults.LevelEndAction.None)
                EndLevelUiCreator.Show(__instance);
        }
    }
}
