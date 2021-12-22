using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
		private bool _firstRun = false;
		private RequestService _requestService;
		public EventHandler ShowFollowerListEvent;

		[UIValue("first-run")]
		public bool FirstRun
		{
			get => _firstRun;
			set
			{
				_firstRun = value;
				NotifyPropertyChanged();
			}
		}
		[UIValue("inverted-first-run")]
		public bool InvertedFirstRun => !_firstRun;

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

			if (_config.AggregatedApiKey == "0000000-0000000-0000000-0000000")
			{
				FirstRun = true;
				_siraLog.Info("No API Key has been provided");
			}
		}

		[UIAction("enter-pin-pressed")]
		public void enterPin_Pressed()
		{
			FirstRun = true;
		}
		[UIAction("show-follower-list")]
		public void showFollowerList_Pressed()
		{
			ShowFollowerListEvent?.Invoke(this, null);
		}
		[UIAction("pin_onchange")]
		public async Task Pin_OnChange(string value)
		{
			_siraLog.Info($"Pin: {value}");
			var httpResponse = await _requestService.Get($"apikey/get", new Dictionary<string, string>() {{"x-api-pin", value}});
			if (httpResponse != null)
			{

				if (httpResponse.Successful)
				{

					var keyResponse = JsonConvert.DeserializeObject<ApiKeyResponse>(await httpResponse.ReadAsStringAsync());
					_siraLog.Info("ApiKey: " + keyResponse.apiKey);
					if (_config.Debug)
					{
						_config.ApiKeyDebug = keyResponse.apiKey;
					}
					else
					{
						_config.ApiKey = keyResponse.apiKey;
					}

					FirstRun = false;
				}
				else
				{
					_siraLog.Error(await httpResponse.Error());
				}
			}
		}
	}
}