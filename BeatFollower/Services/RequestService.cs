using System;
using System.Collections;
using BS_Utils.Utilities;
using UnityEngine.Networking;
using Zenject;

namespace BeatFollower.Services
{
    public class RequestService :  IInitializable, IDisposable
    {
        
        private ConfigService _configService;
        private string _apiKey;

        public RequestService(ConfigService configService)
        {
            _configService = configService;
        }
        public IEnumerator Get(string path, Action<string> callback)
        {
            var url = _configService.ApiUrl + path;
            Logger.log.Debug($"GET: {url}");
            if (_configService.Debug)
                Logger.log.Debug($"ApiKey: {_apiKey}");

            _apiKey = _configService.ApiKey;
            if (string.IsNullOrEmpty(_apiKey) || _apiKey == _configService.DefaultApiKey)
            {
                Logger.log.Debug("API Key is either default or empty");
            }
            else
            {
                UnityWebRequest www = UnityWebRequest.Get(url);
                www.SetRequestHeader("ApiKey", _configService.ApiKey);
                yield return www.SendWebRequest();
                if (www.isNetworkError || www.isHttpError)
                {
                    Logger.log.Debug($"Error getting: {url}");
                    Logger.log.Debug(www.error);
                }
                else
                {
                    string responseString = www.downloadHandler.text;
                    Logger.log.Debug("Response : " + responseString);
                    callback?.Invoke(responseString);

                }
            }
        }
        public IEnumerator Post(string path, string json)
        {
            var url = _configService.ApiUrl + path;
            Logger.log.Debug($"POST: {url}:{json}");
            if(_configService.Debug)
                Logger.log.Debug($"ApiKey: {_apiKey}");

            _apiKey = _configService.ApiKey;
            if (string.IsNullOrEmpty(_apiKey) || _apiKey == _configService.DefaultApiKey)
            {
                Logger.log.Debug("API Key is either default or empty");
            }
            else
            {

                var uwr = new UnityWebRequest(url, "POST");
                uwr.SetRequestHeader("ApiKey", _apiKey);
                byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
                uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
                uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                uwr.SetRequestHeader("Content-Type", "application/json");

                //Send the request then wait here until it returns
                yield return uwr.SendWebRequest();

                if (uwr.isNetworkError || uwr.isHttpError)
                {
                    Logger.log.Debug("Error While Sending: " + uwr.error);
                }
                else
                {
                    Logger.log.Debug("Received: " + uwr.downloadHandler.text);
                }
            }
        }

        public void Initialize()
        {
            
        }

        public void Dispose()
        {
            
        }
    }
}