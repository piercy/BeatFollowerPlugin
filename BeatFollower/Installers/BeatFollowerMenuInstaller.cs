using BeatFollower.UI;

namespace BeatFollower.Installers
{
    public class BeatFollowerMenuInstaller : Zenject.Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<EndScreen>().AsSingle().NonLazy();
        }
    }
}