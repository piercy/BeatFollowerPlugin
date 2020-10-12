using System.Linq;
using System.Reflection;
using HMUI;
using BeatSaberMarkupLanguage;
using PlaylistLoaderLite.UI;
using UnityEngine;

namespace BeatFollower.UI
{
    class ModFlowCoordinator : FlowCoordinator
    {
        private FollowerListViewController _followerListViewController;

        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
            if (firstActivation)
            {
                title = "BeatFollower";
                showBackButton = true;
            }

            _followerListViewController = BeatSaberUI.CreateViewController<FollowerListViewController>();
            this.ProvideInitialViewControllers(_followerListViewController);
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this);
            PluginUI obj = Resources.FindObjectsOfTypeAll<PluginUI>().FirstOrDefault<PluginUI>();
            typeof(PluginUI).GetMethod("RefreshButtonPressed", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(obj, null);
        }
    }
}