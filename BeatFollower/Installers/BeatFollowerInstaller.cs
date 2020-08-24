using BS_Utils.Utilities;
using BeatFollower.Services;
using BeatFollower.UI;
using BeatSaberMarkupLanguage;
using SiraUtil.Zenject;

namespace BeatFollower.Installers
{
    public class BeatFollowerInstaller : Zenject.Installer
    {
        internal static bool firstInstallHappened = false;

        public override void InstallBindings()
        {
            string name = "BeatFollower";
            Container.BindInstance(new Config(name)).WithId($"{name} Config").AsSingle();

            Container.BindInterfacesAndSelfTo<ConfigService>().AsSingle();
            Container.BindInterfacesAndSelfTo<EventService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ActivityService>().AsSingle();
            Container.BindInterfacesAndSelfTo<FollowService>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlaylistService>().AsSingle();
            Container.BindInterfacesAndSelfTo<RequestService>().AsSingle().NonLazy();
            
            firstInstallHappened = true;
        }
    }
}