using System;
using System.Runtime.CompilerServices;
using IPA.Config.Stores;
using IPA.Config.Stores.Attributes;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace BeatFollower
{
	internal class PluginConfig
	{
		[Ignore]
		internal const string DEFAULT_API_URL = "https://api.beatfollower.com";

		[Ignore]
		internal const string DEFAULT_API_KEY = "0000000-0000000-0000000-0000000";

		public virtual string ApiUrl { get; set; } = DEFAULT_API_URL;

		public virtual string ApiKey { get; set; } = DEFAULT_API_KEY;

		public virtual string Position { get; set; } = "BottomLeft";

		public virtual bool Debug { get; set; }

		public virtual string? ApiUrlDebug { get; set; }

		public virtual string? ApiKeyDebug { get; set; }

		[Ignore]
		public string AggregatedApiUrl => ApiUrlDebug ?? ApiUrl;

		[Ignore]
		public string AggregatedApiKey => ApiKeyDebug ?? ApiKey;

		public virtual void Changed()
		{
			// this is called whenever one of the virtual properties is changed
			// can be called to signal that the content has been changed
			FixIncorrectPropertyState();
		}

		public virtual void OnReload()
		{

			// this is called whenever the config file is reloaded from disk
			// use it to tell all of your systems that something has changed

			// this is called off of the main thread, and is not safe to interact
			//   with Unity in
			FixIncorrectPropertyState();
		}

		public virtual IDisposable ChangeTransaction => null!;

		private void FixIncorrectPropertyState()
		{
			// Set defaults
			if (string.IsNullOrEmpty(ApiUrl))
			{
				ApiUrl = PluginConfig.DEFAULT_API_URL;
			}
			else if (!ApiUrl.EndsWith("/"))
			{
				ApiUrl += "/";
			}

			if (string.IsNullOrEmpty(Position))
			{
				Position = "BottomLeft";
			}

			if (string.IsNullOrEmpty(ApiKey))
			{
				ApiKey = PluginConfig.DEFAULT_API_KEY;
			}
		}
	}
}