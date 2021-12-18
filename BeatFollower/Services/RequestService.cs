using System;
using System.Collections;
using SiraUtil.Logging;
using UnityEngine.Networking;

namespace BeatFollower.Services
{
	internal class RequestService
	{
		private readonly SiraLog _siraLog;
		private readonly PluginConfig _config;

		public RequestService(SiraLog siraLog, PluginConfig config)
		{
			_siraLog = siraLog;
			_config = config;
		}

		public IEnumerator Get(string path, Action<string> callback)
		{
			var url = _config.AggregatedApiUrl + path;
			_siraLog.Debug($"GET: {url}");

			// if (_config.Debug.HasValue && _config.Debug.Value)
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
				var uwr = UnityWebRequest.Get(url);
				uwr.SetRequestHeader("ApiKey", _config.AggregatedApiKey);
				yield return uwr.SendWebRequest();
				if (uwr.isNetworkError || uwr.isHttpError)
				{
					_siraLog.Error($"Error While Getting: {url} {uwr.responseCode} {uwr.error}");
				}
				else
				{
					var responseString = uwr.downloadHandler.text;
					_siraLog.Debug("Response : " + responseString);
					callback?.Invoke(responseString);
				}
			}
		}

		public IEnumerator Post(string path, string json)
		{
			var url = _config.AggregatedApiUrl + path;
			_siraLog.Debug($"POST: {url}:{json}");
			// if (_config.Debug.HasValue && _config.Debug.Value)
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
				var uwr = new UnityWebRequest(url, "POST");
				uwr.SetRequestHeader("ApiKey", _config.AggregatedApiKey);
				var jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
				uwr.uploadHandler = new UploadHandlerRaw(jsonToSend);
				uwr.downloadHandler = new DownloadHandlerBuffer();
				uwr.SetRequestHeader("Content-Type", "application/json");

				//Send the request then wait here until it returns
				yield return uwr.SendWebRequest();

				if (uwr.isNetworkError || uwr.isHttpError)
				{
					_siraLog.Error($"Error While Posting: {uwr.responseCode} {uwr.error}");
				}
				else
				{
					_siraLog.Debug("Received: " + uwr.downloadHandler.text);
				}
			}
		}
	}
}