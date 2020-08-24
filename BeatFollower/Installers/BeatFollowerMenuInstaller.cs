using BeatFollower.UI;
using SiraUtil.Zenject;
using BeatSaberMarkupLanguage;

namespace BeatFollower.Installers
{
    public class BeatFollowerMenuInstaller : Zenject.Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<EndScreen>().AsSingle().NonLazy();
            var beatFollowerViewController = BeatSaberUI.CreateViewController<BeatFollowerViewController>();
            var flowCoordinator = BeatSaberUI.CreateFlowCoordinator<ModFlowCoordinator>();

            Container.ForceBindComponent<BeatFollowerViewController>(beatFollowerViewController);
            Container.ForceBindComponent<ModFlowCoordinator>(flowCoordinator);
        }
    }
}