using BeatFollower.Installers;
using BeatFollower.Services;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using IPA.Logging;
using SiraUtil.Zenject;

namespace BeatFollower
{
	[Plugin(RuntimeOptions.DynamicInit), NoEnableDisable]
	public class Plugin
	{
		[Init]
		public Plugin(Config conf, Logger logger, Zenjector zenjector)
		{
			var config = conf.Generated<PluginConfig>();
			ConfigMigrationService.MigrateFromOldConfig(logger, config);

			zenjector.UseLogger(logger);
			zenjector.UseMetadataBinder<Plugin>();

			zenjector.Install<BFAppInstaller>(Location.App, config);
			zenjector.Install<BFMenuInstaller>(Location.Menu);
			zenjector.Install<BFGameInstaller>(Location.StandardPlayer);
		}
	}
}