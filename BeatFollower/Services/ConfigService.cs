﻿using BS_Utils.Utilities;

namespace BeatFollower.Services
{
    public static class ConfigService
    {
        private static Config _config;

        const string Name = "BeatFollower";

        public static string DefaultApiKey => "0000000-0000000-0000000-0000000";
        private static string defaultApiUrl = "https://api.beatfollower.com";

        public static string ApiUrl { get; private set; }

        public static string ApiKey { get; private set; }

        public static string Position { get; private set; }

        public static bool Debug { get; private set; }
        public static string ApiUrlDebug { get; set; }

        public static string ApiKeyDebug { get; set; }

        public static void Initialize()
        {
            if(_config == null)
                _config = new BS_Utils.Utilities.Config(Name);

            Position = _config.GetString(Name, "Position");
            ApiKey = _config.GetString(Name, "ApiKey");
            ApiUrl = _config.GetString(Name, "ApiUrl");

            ApiKeyDebug = _config.GetString(Name, "ApiKeyDebug");
            ApiUrlDebug = _config.GetString(Name, "ApiUrlDebug");

            Debug = _config.GetBool(Name, "Debug");


            if(Debug) {
                Logger.log.Debug($"##### BEATFOLLOWER DEBUG IS SET TO TRUE. YOU SHOULD NOT UPLOAD YOUR LOG FILES ANYWHERE AS SENSITIVE INFORMATION COULD BE LEAKED! #####");
                Logger.log.Debug($"##### BEATFOLLOWER DEBUG IS SET TO TRUE. USE OF DEBUG MODE IS AT YOUR OWN RISK! #####");
            }

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

            if (Debug)
            {
                ApiKey = ApiKeyDebug;
                ApiUrl = ApiUrlDebug;
            }



            if (!ApiUrl.EndsWith("/"))
            {
                ApiUrl += "/";
            }


            Logger.log.Debug($"ApiUrl: {ApiUrl}");

            if(Debug)
                Logger.log.Debug($"ApiKey: {ApiKey}");
        }


    }
}