using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SiraUtil.Logging;
using SiraUtil.Web;
using UnityEngine.Networking;

namespace BeatFollower.Services
{
	internal class RequestService
	{
		private readonly SiraLog _siraLog;
		private readonly PluginConfig _config;
		private readonly IHttpService _httpService;

		public RequestService(SiraLog siraLog, PluginConfig config, IHttpService httpService)
		{
			_siraLog = siraLog;
			_config = config;
			_httpService = httpService;
		}

		public async Task<IHttpResponse?> Get(string path, Dictionary<string, string>? headers = null)
		{
			var url = _config.AggregatedApiUrl + path;
			_siraLog.Info($"GET: {url}");

			if (_config.Debug)
			{
				_siraLog.Info($"ApiKey: {_config.AggregatedApiKey}");
			}


			if (_config.AggregatedApiKey != PluginConfig.DEFAULT_API_KEY)
			{
				if (!_httpService.Headers.ContainsKey("ApiKey"))
				{
					_httpService.Headers.Add("ApiKey", _config.AggregatedApiKey);
				}
			}

			if (headers != null)
			{
				foreach (var header in headers)
				{
					if (!_httpService.Headers.ContainsKey(header.Key))
					{
						_httpService.Headers.Add(header.Key, header.Value);
					}
				}
			}


			CancellationTokenSource tokenSource = new CancellationTokenSource();
			IProgress<float>? progress = null;

			var httpResponse = await _httpService.GetAsync(url, progress, tokenSource.Token);

			if(!httpResponse.Successful)
			{
				_siraLog.Error($"Error While Getting: {url} {httpResponse.Code} {await httpResponse.Error()}");
			}
			else
			{
				return httpResponse;
			}

			return null;
		}

		public async Task<IHttpResponse?>  Post(string path, object? json)
		{
			var url = _config.AggregatedApiUrl + path;
			_siraLog.Debug($"POST: {url}:{json}");

			if (_config.Debug)
			{
				_siraLog.Debug($"ApiKey: {_config.AggregatedApiKey}");
			}

			if (string.IsNullOrEmpty(_config.AggregatedApiKey) || _config.AggregatedApiKey == PluginConfig.DEFAULT_API_KEY)
			{
				_siraLog.Debug("API Key is either default or empty");
			}
			else
			{
				CancellationTokenSource tokenSource = new CancellationTokenSource();

				if (_config.AggregatedApiKey != PluginConfig.DEFAULT_API_KEY)
				{
					if (!_httpService.Headers.ContainsKey("ApiKey"))
					{
						_httpService.Headers.Add("ApiKey", _config.AggregatedApiKey);
					}
				}


				var httpResponse = await _httpService.PostAsync(url, json, tokenSource.Token);

				//
				// var jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
				// uwr.uploadHandler = new UploadHandlerRaw(jsonToSend);
				// uwr.downloadHandler = new DownloadHandlerBuffer();
				// uwr.SetRequestHeader("Content-Type", "application/json");

				//Send the request then wait here until it returns
				//yield return uwr.SendWebRequest();

				if (!httpResponse.Successful)
				{
					_siraLog.Error($"Error While Posting: {url} {httpResponse.Code} {await httpResponse.Error()}");
				}
				else
				{
					return httpResponse;
				}
			}

			return null;
		}
	}
}