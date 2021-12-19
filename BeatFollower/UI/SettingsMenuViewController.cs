using System;
using System.Collections.Generic;
using BeatFollower.Models;
using BeatFollower.Services;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using Newtonsoft.Json;
using SiraUtil.Logging;
using Zenject;

namespace BeatFollower.UI
{
	[HotReload(RelativePathToLayout = @"Views\settings-menu.bsml")]
	[ViewDefinition("BeatFollower.UI.Views.settings-menu.bsml")]
	internal class SettingsMenuViewController : BSMLAutomaticViewController
	{
		private DiContainer _container = null!;
		private SiraLog _siraLog = null!;
		private PluginConfig _config;
		private bool noApiKey = false;
		private RequestService _requestService;

		[Inject]
		internal void Construct(DiContainer container, SiraLog siraLog, PluginConfig config, RequestService requestService)
		{
			_container = container;
			_siraLog = siraLog;
			_config = config;
			_requestService = requestService;
		}

		protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
		{
			base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);

			if (firstActivation)
			{
			}

			if (_config.ApiKey == "0000000-0000000-0000000-0000000")
			{
				noApiKey = true;
				_siraLog.Info("No API Key has been provided");
			}
		}

		[UIAction("pin_onchange")]
		public void Pin_OnChange(string value)
		{
			_siraLog.Info($"Pin: {value}");
			SharedCoroutineStarter.instance.StartCoroutine(_requestService.Get("apikey/get", response =>
			{
				var keyResponse = JsonConvert.DeserializeObject<ApiKeyResponse>(response);
				_siraLog.Info("ApiKey: " + keyResponse.apiKey);
				if (_config.Debug)
				{
					_config.ApiKeyDebug = keyResponse.apiKey;
				}
				else
				{
					_config.ApiKey = keyResponse.apiKey;
				}
			}, new Dictionary<string, string>() {{"x-api-pin", value}}));
		}
	}
}