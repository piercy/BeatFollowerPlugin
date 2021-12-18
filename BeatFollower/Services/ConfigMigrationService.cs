using System.IO;
using IPA.Utilities;

namespace BeatFollower.Services
{
	// TODO: Remove this in due time.
	internal class ConfigMigrationService
	{
		private static readonly string OldConfigPath = Path.Combine(UnityGame.UserDataPath, nameof(BeatFollower) + ".ini");

		internal static bool ShouldRunMigration()
		{
			return File.Exists(OldConfigPath);
		}

		internal static void MigrateFromOldConfig(PluginConfig config)
		{
			using var _ = config.ChangeTransaction;

			var bsUtilsConfig = new BS_Utils.Utilities.Config(nameof(BeatFollower));

			config.ApiKey = bsUtilsConfig.GetString(nameof(BeatFollower), "ApiKey");
			config.ApiUrl = bsUtilsConfig.GetString(nameof(BeatFollower), "ApiUrl");
			config.Position = bsUtilsConfig.GetString(nameof(BeatFollower), "Position");

			// Clearing out the old address automatically for the testers. It will then set the default
			if (config.ApiUrl.StartsWith("http://direct.beatfollower.com"))
			{
				config.ApiUrl = PluginConfig.DEFAULT_API_URL;
			}

			File.Delete(OldConfigPath);
		}
	}
}