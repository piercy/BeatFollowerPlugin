using Zenject;
using UnityEngine;
using SiraUtil.Zenject;
using BeatSaberMarkupLanguage.MenuButtons;
using BeatSaberMarkupLanguage;
using BeatFollower.UI;

namespace BeatFollower.Installers
{
    public class BFIStartup : ISiraInstaller
    {
        public void Install(DiContainer container, GameObject source)
        {
            container.Install<BeatFollowerInstaller>();
        }
    }

    public class BFMIStartup : ISiraInstaller
    {
        public DiContainer Container;
        private MenuButton _menuButton;

        public void Install(DiContainer container, GameObject source)
        {
            Container = container;
            if (BeatFollowerInstaller.firstInstallHappened)
                container.Install<BeatFollowerMenuInstaller>();
        }

        public void AddButton()
        {
            if (_menuButton == null)
                _menuButton = new MenuButton("BeatFollower", "", SummonFlowCoordinator);
            MenuButtons.instance.RegisterButton(_menuButton);
        }

        public void RemoveButton()
        {
            MenuButtons.instance.UnregisterButton(_menuButton);
        }

        private void SummonFlowCoordinator()
        {
            var flowCoordinator = Container.TryResolve<ModFlowCoordinator>();
            if (flowCoordinator != null)
                BeatSaberUI.MainFlowCoordinator.PresentFlowCoordinator(flowCoordinator);
        }
    }
}