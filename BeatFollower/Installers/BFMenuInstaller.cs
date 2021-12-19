using BeatFollower.AffinityPatches;
using BeatFollower.UI;
using Zenject;

namespace BeatFollower.Installers
{
	internal class BFMenuInstaller : Installer
	{
		public override void InstallBindings()
		{
			Container.Bind<FollowerListObject>().AsTransient();
			Container.Bind<FollowerListViewController>().FromNewComponentAsViewController().AsSingle();
			Container.Bind<SettingsMenuViewController>().FromNewComponentAsViewController().AsSingle();
			Container.BindInterfacesTo<ModFlowCoordinator>().FromNewComponentOnNewGameObject().AsSingle();

			Container.Bind<CustomListObject>().AsTransient();
			Container.Bind<EndLevelViewController>().FromNewComponentAsViewController().AsSingle();
			Container.BindInterfacesTo<LevelEndUiPatches>().AsSingle();
		}
	}
}