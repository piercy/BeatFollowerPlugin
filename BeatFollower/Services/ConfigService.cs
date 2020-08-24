using System;
using BS_Utils.Utilities;
using Zenject;

namespace BeatFollower.Services
{
    public class ConfigService : IInitializable, IDisposable
    {
        private Config _config;

        const string Name = "BeatFollower";

        public string DefaultApiKey => "0000000-0000000-0000000-0000000";
        private string defaultApiUrl = "https://api.beatfollower.com";

        public string ApiUrl { get; private set; }
        public string ApiKey { get; private set; }
        public string Position { get; private set; }

        public bool Debug { get; private set; }

        public ConfigService([Inject(Id = "BeatFollower Config")] Config config)
        {
            _config = config;
        }
        public void Initialize()
        {
            Position = _config.GetString(Name, "Position");
            ApiKey = _config.GetString(Name, "ApiKey");
            ApiUrl = _config.GetString(Name, "ApiUrl");
            Debug = _config.GetBool(Name, "Debug");

            Logger.log.Debug($"##### BEATFOLLOWER DEBUG IS SET TO TRUE. YOU SHOULD NOT UPLOAD YOUR LOG FILES ANYWHERE AS SENSITIVE INFORMATION COULD BE LEAKED! #####");
            Logger.log.Debug($"##### BEATFOLLOWER DEBUG IS SET TO TRUE. USE OF DEBUG MODE IS AT YOUR OWN RISK! #####");

            // Clearing out the old address automatically for the testers. It will then set the default
            if (ApiUrl.StartsWith("http://direct.beatfollower.com"))
                ApiUrl = null;

            // Set defaults
            if (string.IsNullOrEmpty(ApiUrl))
            {
                _config.SetString(Name, "ApiUrl", defaultApiUrl);
                ApiUrl = defaultApiUrl;
            }

            if (string.IsNullOrEmpty(Position))
            {
                _config.SetString(Name, "Position", "BottomLeft");
            }

            if (string.IsNullOrEmpty(ApiKey))
            {
                _config.SetString(Name, "ApiKey", DefaultApiKey);
            }

            if (!ApiUrl.EndsWith("/"))
            {
                ApiUrl += "/";
            }
            Logger.log.Debug($"ApiUrl: {ApiUrl}");
            if(Debug)
                Logger.log.Debug($"ApiKey: {ApiKey}");
        }


        public void Dispose()
        {
        }
    }
}