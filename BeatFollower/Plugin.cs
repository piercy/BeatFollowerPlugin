using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BeatFollower.Installers;
using BeatFollower.Models;
using BeatFollower.Services;
using BeatFollower.UI;
using BS_Utils.Utilities;
using HarmonyLib;
using IPA;
using IPA.Config;
using Newtonsoft.Json;
using UnityEngine;
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
        public const string HarmonyId = "com.github.piercy.BeatFollower";
        internal static Harmony harmony;

        [Init]
        public void Init(object nullObject, IPA.Logging.Logger logger)
        {
            Logger.log = logger;
            harmony = new Harmony(HarmonyId);
        }

        [OnStart]
        public void OnApplicationStart()
        {
            Logger.log.Debug("OnApplicationStart");
            
            _eventService = new EventService();
            _startup = new Startup();
        }
        [OnEnable]
        public void OnEnable()
        {
            try
            {
                Logger.log.Debug("BeatFollower Enabled");
                ApplyHarmonyPatches();
                ConfigService.Initialize();
                _eventService.Initialize();
                _startup.AddButton();

                new GameObject("EndLevelUiCreator").AddComponent<EndLevelUiCreator>().plugin = this;
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
                RemoveHarmonyPatches();


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

        public static void ApplyHarmonyPatches()
        {
            try
            {
                Logger.log.Debug("Applying Harmony patches.");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Logger.log.Critical("Error applying Harmony patches: " + ex.Message);
                Logger.log.Error(ex);
            }
        }
        public static void RemoveHarmonyPatches()
        {
            try
            {
                Logger.log.Debug("Removing Harmony patches.");
                harmony.UnpatchAll(HarmonyId);
            }
            catch (Exception ex)
            {
                Logger.log.Critical("Error removing harmony patches: " + ex.Message);
                Logger.log.Error(ex);
            }
        }

    }
}
