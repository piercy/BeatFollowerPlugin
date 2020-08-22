using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage;
using HMUI;
using Zenject;

namespace BeatFollower.UI
{
    class ModFlowCoordinator : FlowCoordinator
    {
        [Inject] private BeatFollowerViewController beatFollowerViewController;

        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
            if (firstActivation)
            {
                title = "BeatFollower";
                showBackButton = true;
            }
            Logger.log.Debug("im in the FC");
            
            ProvideInitialViewControllers(beatFollowerViewController);
        }
        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this);
        }
    }
}
    