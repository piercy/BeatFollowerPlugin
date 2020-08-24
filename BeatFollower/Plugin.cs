using IPA;
using System;
using BeatFollower.Installers;
using SiraUtil.Zenject;

namespace BeatFollower
{

    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        internal static string Name => "BeatFollower";

        private BFIStartup _bfiStartup;
        private BFMIStartup _bfmiStartup;

        [Init]
        public void Init(IPA.Logging.Logger logger)
        {
            Logger.log = logger;

            _bfiStartup = new BFIStartup();
            _bfmiStartup = new BFMIStartup();
        }

        [OnEnable]
        public void OnEnable()
        {
            try
            {
                Installer.RegisterAppInstaller(_bfiStartup);
                Installer.RegisterMenuInstaller(_bfmiStartup);
                _bfmiStartup.AddButton();
                Logger.log.Debug("BeatFollower Enabled");
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
                Installer.UnregisterAppInstaller(_bfiStartup);
                Installer.UnregisterMenuInstaller(_bfmiStartup);
                _bfmiStartup.RemoveButton();
                Logger.log.Debug("BeatFollower Disabled");
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex);
            }
        }
    }
}
