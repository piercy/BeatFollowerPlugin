using BeatFollower.Services;
using HMUI;
using BeatSaberMarkupLanguage;

namespace BeatFollower.UI
{
    class ModFlowCoordinator : FlowCoordinator
    {
        private FollowerListViewController _followerListViewController;
       
        
        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            if (firstActivation)
            {
                SetTitle("BeatFollower");
                showBackButton = true;
            }

            _followerListViewController = BeatSaberUI.CreateViewController<FollowerListViewController>();
            this.ProvideInitialViewControllers(_followerListViewController);
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this);
            var playlistService = new PlaylistService();

            playlistService.Reload();
            
        }
    }
}