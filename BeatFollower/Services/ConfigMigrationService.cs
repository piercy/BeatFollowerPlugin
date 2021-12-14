using System.IO;
using System.Runtime.CompilerServices;
using IPA.Loader;
using IPA.Utilities;
using Logger = IPA.Logging.Logger;

namespace BeatFollower.Services
{
	internal class ConfigMigrationService
	{
		internal static void MigrateFromOldConfig(Logger logger, PluginConfig config)
		{
			var oldConfigPath = Path.Combine(UnityGame.UserDataPath, nameof(BeatFollower) + ".ini");
			if (!File.Exists(oldConfigPath))
			{
				return;
			}

			if (PluginManager.GetPlugin("BS Utils") != null)
			{
				MigrateConfigInternal(logger, config);
			}

			File.Delete(oldConfigPath);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void MigrateConfigInternal(Logger logger, PluginConfig config)
		{
			using var _ = config.ChangeTransaction;

			var bsUtilsConfig = new BS_Utils.Utilities.Config(nameof(BeatFollower));

			config.ApiKey = bsUtilsConfig.GetString(nameof(BeatFollower), "ApiKey");
			config.ApiUrl = bsUtilsConfig.GetString(nameof(BeatFollower), "ApiUrl");
			config.Position = bsUtilsConfig.GetString(nameof(BeatFollower), "Position");

			// if (config.Debug)
			{
				logger.Debug("##### BEATFOLLOWER DEBUG IS SET TO TRUE. YOU SHOULD NOT UPLOAD YOUR LOG FILES ANYWHERE AS SENSITIVE INFORMATION COULD BE LEAKED! #####");
				logger.Debug("##### BEATFOLLOWER DEBUG IS SET TO TRUE. USE OF DEBUG MODE IS AT YOUR OWN RISK! #####");
			}

			// Clearing out the old address automatically for the testers. It will then set the default
			if (config.ApiUrl.StartsWith("http://direct.beatfollower.com"))
			{
				config.ApiUrl = PluginConfig.DEFAULT_API_URL;
			}
		}
	}
}