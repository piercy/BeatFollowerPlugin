using BS_Utils.Utilities;
using BeatFollower.Services;

namespace BeatFollower.Installers
{
    public class BeatFollowerInstaller : Zenject.Installer
    {
        public override void InstallBindings()
        {
            string name = "BeatFollower";
            Container.BindInterfacesAndSelfTo<EventService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BeatFollowerService>().AsSingle().NonLazy();
            Container.BindInstance(new Config(name)).WithId($"{name} Config").AsSingle();
        }
    }
}