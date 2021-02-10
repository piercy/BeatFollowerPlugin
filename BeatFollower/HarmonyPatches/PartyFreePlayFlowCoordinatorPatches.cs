using BeatFollower.UI;
using HarmonyLib;

namespace BeatFollower.HarmonyPatches
{
	[HarmonyPatch(typeof(PartyFreePlayFlowCoordinator), "ProcessLevelCompletionResultsAfterLevelDidFinish")]
	//[HarmonyPatch("SetDataToUI", MethodType.Normal)]
	class PartyFreePlayFlowCoordinatorPatches
	{
		static void Postfix(ref PartyFreePlayFlowCoordinator __instance, LevelCompletionResults levelCompletionResults)
		{
			// Show end of song UI
			if (levelCompletionResults.levelEndAction == LevelCompletionResults.LevelEndAction.None)
				EndLevelUiCreator.Show(__instance);
		}
	}
}