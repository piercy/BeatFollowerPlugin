using System;
using System.Collections;
using UnityEngine.Networking;


namespace BeatFollower.Services
{
    public class RequestService
    {
        public RequestService()
        {
        }

        public IEnumerator Get(string path, Action<string> callback)
        {
            var url = ConfigService.ApiUrl + path;
            Logger.log.Debug($"GET: {url}");

            if (ConfigService.Debug)
                Logger.log.Debug($"ApiKey: {ConfigService.ApiKey}");

            if (string.IsNullOrEmpty(ConfigService.ApiKey) || ConfigService.ApiKey == ConfigService.DefaultApiKey)
            {
                Logger.log.Debug("API Key is either default or empty");
            }
            else
            {
                UnityWebRequest uwr = UnityWebRequest.Get(url);
                uwr.SetRequestHeader("ApiKey", ConfigService.ApiKey);
                yield return uwr.SendWebRequest();
                if (uwr.isNetworkError || uwr.isHttpError)
                {
                    Logger.log.Error($"Error While Getting: {url} {uwr.responseCode} {uwr.error}");
                }
                else
                {
                    string responseString = uwr.downloadHandler.text;
                    Logger.log.Debug("Response : " + responseString);
                    callback?.Invoke(responseString);

                }
            }
        }
        public IEnumerator Post(string path, string json)
        {
            var url = ConfigService.ApiUrl + path;
            Logger.log.Debug($"POST: {url}:{json}");
            if(ConfigService.Debug)
                Logger.log.Debug($"ApiKey: {ConfigService.ApiKey}");

            if (string.IsNullOrEmpty(ConfigService.ApiKey) || ConfigService.ApiKey == ConfigService.DefaultApiKey)
            {
                Logger.log.Debug("API Key is either default or empty");
            }
            else
            {

                var uwr = new UnityWebRequest(url, "POST");
                uwr.SetRequestHeader("ApiKey", ConfigService.ApiKey);
                byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
                uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
                uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                uwr.SetRequestHeader("Content-Type", "application/json");

                //Send the request then wait here until it returns
                yield return uwr.SendWebRequest();

                if (uwr.isNetworkError || uwr.isHttpError)
                {
                    Logger.log.Error($"Error While Posting: {uwr.responseCode} {uwr.error}" );
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