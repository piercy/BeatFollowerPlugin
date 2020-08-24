using HMUI;
using Zenject;
using BeatSaberMarkupLanguage;

namespace BeatFollower.UI
{
    class ModFlowCoordinator : FlowCoordinator
    {
        private BeatFollowerViewController _beatFollowerViewController;

        [Inject]
        public void Construct(BeatFollowerViewController beatFollowerViewController)
        {
            _beatFollowerViewController = beatFollowerViewController;
        }

        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
            if (firstActivation)
            {
                title = "BeatFollower";
                showBackButton = true;
            }
            Logger.log.Debug("im in the FC");
            
            ProvideInitialViewControllers(_beatFollowerViewController);
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this);
        }
    }
}