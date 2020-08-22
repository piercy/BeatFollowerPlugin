using Zenject;
using UnityEngine;
using SiraUtil.Zenject;

namespace BeatFollower.Installers
{
    public class BFIStartup : ISiraInstaller
    {
        public void Install(DiContainer container, GameObject source)
        {
            container.Install<BeatFollowerInstaller>();
        }
    }

    public class BFMIStartup : ISiraInstaller
    {
        public void Install(DiContainer container, GameObject source)
        {
            container.Install<BeatFollowerMenuInstaller>();
        }
    }
}