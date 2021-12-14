using BeatFollower.Services;
using Zenject;

namespace BeatFollower.Installers
{
	internal class BFAppInstaller : Installer
	{
		private readonly PluginConfig _config;

		public BFAppInstaller(PluginConfig config)
		{
			_config = config;
		}

		public override void InstallBindings()
		{
			Container.BindInstance(_config);

			Container.BindInterfacesAndSelfTo<ActivityService>().AsSingle();
			Container.Bind<CustomListService>().AsSingle();
			Container.Bind<RequestService>().AsSingle();
			Container.Bind<FollowService>().AsSingle();
			Container.Bind<PlaylistService>().AsSingle();

			Container.Bind<LastBeatmapManager>().AsSingle();
		}
	}
}