using BeatFollower.UI;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.MenuButtons;
using HMUI;
using SiraUtil.Zenject;

namespace BeatFollower.Installers
{
    public class BeatFollowerMenuInstaller : Zenject.Installer
    {
        private ModFlowCoordinator flowCoordinator;
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<EndScreen>().AsSingle().NonLazy();

            BindViewController<BeatFollowerViewController>();



            flowCoordinator = BeatSaberUI.CreateFlowCoordinator<ModFlowCoordinator>();
            Container.InjectSpecialInstance<ModFlowCoordinator>(flowCoordinator);


            MenuButton button = new MenuButton("BeatFollower", "", OnClick);
            MenuButtons.instance.RegisterButton(button);
        }
        private void OnClick()
        {
            BeatSaberUI.MainFlowCoordinator.PresentFlowCoordinator(flowCoordinator);
        }
        private void BindViewController<T>() where T : ViewController
        {
            T view = BeatSaberUI.CreateViewController<T>();
            Container.InjectSpecialInstance<T>(view);
        }
    }
}