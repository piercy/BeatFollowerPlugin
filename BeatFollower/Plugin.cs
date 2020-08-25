using System;
using System.Collections;
using System.Collections.Generic;
using BeatFollower.Installers;
using BeatFollower.Models;
using BeatFollower.Services;
using BeatFollower.UI;
using BS_Utils.Utilities;
using IPA;
using IPA.Config;
using Newtonsoft.Json;
using UnityEngine.Networking;
using Config = BS_Utils.Utilities.Config;


namespace BeatFollower
{

    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        private EventService _eventService;
        private Startup _startup;
        internal static Plugin instance { get; private set; }
        internal static string Name => "BeatFollower";


        [Init]
        public void Init(object nullObject, IPA.Logging.Logger logger)
        {
            Logger.log = logger;
        }

        [OnStart]
        public void OnApplicationStart()
        {
            Logger.log.Debug("OnApplicationStart");
            ConfigService.Initialize();
            _eventService = new EventService();
            _startup = new Startup();
            
        }

        [OnEnable]
        public void OnEnable()
        {
            try
            {
                Logger.log.Debug("BeatFollower Enabled");
                _eventService.Initialize();
                _startup.AddButton();
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex);
            }
        }

        [OnDisable]
        public void OnDisable()
        {
            try
            {
                Logger.log.Debug("BeatFollower Disabled");
                _eventService.Dispose();
                _startup.RemoveButton();

            }
            catch (Exception ex)
            {
                Logger.log.Error(ex);
            }
        }


        [OnExit]
        public void OnApplicationQuit()
        {
            Logger.log.Debug("OnApplicationQuit");
        }
    }
}
