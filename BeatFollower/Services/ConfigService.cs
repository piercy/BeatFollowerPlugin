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
        private string _apiKey;
        private string _apiUrl;
        private string _position;
        private bool _debug;

        public string ApiUrl
        {
            get => _apiUrl;
            private set => _apiUrl = value;
        }

        public string ApiKey
        {
            get => _apiKey;
            private set => _apiKey = value;
        }

        public string Position
        {
            get => _position;
            private set => _position = value;
        }

        public bool Debug
        {
            get => _debug;
            private set => _debug = value;
        }

        public ConfigService([Inject(Id = "BeatFollower Config")] Config config)
        {
            _config = config;
        }
        public void Initialize()
        {
            _position = _config.GetString(Name, "Position");
            _apiKey = _config.GetString(Name, "ApiKey");
            _apiUrl = _config.GetString(Name, "ApiUrl");
            _debug = _config.GetBool(Name, "Debug");

            Logger.log.Debug($"##### BEATFOLLOWER DEBUG IS SET TO TRUE. YOU SHOULD NOT UPLOAD YOUR LOG FILES ANYWHERE AS SENSITIVE INFORMATION COULD BE LEAKED! #####");
            Logger.log.Debug($"##### BEATFOLLOWER DEBUG IS SET TO TRUE. USE OF DEBUG MODE IS AT YOUR OWN RISK! #####");

            // Clearing out the old address automatically for the testers. It will then set the default
            if (_apiUrl.StartsWith("http://direct.beatfollower.com"))
                _apiUrl = null;

            // Set defaults
            if (string.IsNullOrEmpty(_apiUrl))
            {
                _config.SetString(Name, "ApiUrl", defaultApiUrl);
                _apiUrl = defaultApiUrl;
            }

            if (string.IsNullOrEmpty(Position))
            {
                _config.SetString(Name, "Position", "BottomLeft");
            }

            if (string.IsNullOrEmpty(_apiKey))
            {
                _config.SetString(Name, "ApiKey", DefaultApiKey);
            }

            if (!_apiUrl.EndsWith("/"))
            {
                _apiUrl += "/";
            }
            Logger.log.Debug($"ApiUrl: {_apiUrl}");
            if(_debug)
                Logger.log.Debug($"ApiKey: {_apiKey}");
        }


        public void Dispose()
        {
        }
    }
}