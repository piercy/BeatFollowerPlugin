using BeatFollower.Services;
using Zenject;

namespace BeatFollower.Installers
{
	internal class BFGameInstaller : Installer
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesTo<BeatmapCollector>().AsSingle();
		}
	}
}