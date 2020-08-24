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
            Container.BindInterfacesAndSelfTo<EventService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BeatFollowerService>().AsSingle().NonLazy();
            Container.BindInstance(new Config(name)).WithId($"{name} Config").AsSingle();
            firstInstallHappened = true;
        }
    }
}