using BeatFollower.UI;
using BS_Utils.Utilities;
using HMUI;
using SiraUtil.Affinity;
using SiraUtil.Logging;
using Zenject;

namespace BeatFollower.AffinityPatches
{
	internal class LevelEndUiPatches : IAffinity
	{
		private readonly SiraLog _siraLog;
		private readonly LazyInject<EndLevelViewController> _endLevelViewController;

		[Inject]
		public LevelEndUiPatches(SiraLog siraLog, LazyInject<EndLevelViewController> endLevelViewController)
		{
			_siraLog = siraLog;
			_endLevelViewController = endLevelViewController;
		}

		[AffinityPostfix]
		[AffinityPatch(typeof(SoloFreePlayFlowCoordinator), "ProcessLevelCompletionResultsAfterLevelDidFinish")]
		private void SoloPostfix(ref SoloFreePlayFlowCoordinator __instance, LevelCompletionResults levelCompletionResults)
		{
			ShowEndLevelViewController(__instance, levelCompletionResults);
		}

		[AffinityPostfix]
		[AffinityPatch(typeof(PartyFreePlayFlowCoordinator), "ProcessLevelCompletionResultsAfterLevelDidFinish")]
		private void PartyPostfix(ref PartyFreePlayFlowCoordinator __instance, LevelCompletionResults levelCompletionResults)
		{
			ShowEndLevelViewController(__instance, levelCompletionResults);
		}

		private void ShowEndLevelViewController(FlowCoordinator fc, LevelCompletionResults levelCompletionResults)
		{
			if (levelCompletionResults.levelEndAction == LevelCompletionResults.LevelEndAction.None)
			{
				fc.InvokeMethod("SetTopScreenViewController", _endLevelViewController.Value, ViewController.AnimationType.None);
			}
		}
	}
}