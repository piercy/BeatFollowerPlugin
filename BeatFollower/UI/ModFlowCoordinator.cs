using HMUI;
using BeatSaberMarkupLanguage;

namespace BeatFollower.UI
{
    class ModFlowCoordinator : FlowCoordinator
    {
        private BeatFollowerViewController _beatFollowerViewController;

        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
            if (firstActivation)
            {
                title = "BeatFollower";
                showBackButton = true;
            }
            Logger.log.Debug("im in the FC");

            _beatFollowerViewController = BeatSaberUI.CreateViewController<BeatFollowerViewController>();
            this.ProvideInitialViewControllers(_beatFollowerViewController);
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this);
        }
    }
}