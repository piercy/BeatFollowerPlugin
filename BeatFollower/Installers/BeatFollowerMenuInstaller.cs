using BeatFollower.UI;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.MenuButtons;
using SiraUtil.Zenject;

namespace BeatFollower.Installers
{
    public class BeatFollowerMenuInstaller : Zenject.Installer
    {
        private ModFlowCoordinator flowCoordinator;
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<EndScreen>().AsSingle().NonLazy();
            
            flowCoordinator = BeatSaberUI.CreateFlowCoordinator<ModFlowCoordinator>();

            MenuButton button = new MenuButton("BeatFollower", "", OnClick);
            MenuButtons.instance.RegisterButton(button);


        }
        private void OnClick()
        {
            BeatSaberUI.MainFlowCoordinator.PresentFlowCoordinator(flowCoordinator);
        }
    }
}