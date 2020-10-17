using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage;
using BS_Utils.Utilities;
using HMUI;
using UnityEngine;

namespace BeatFollower.UI
{
    public class EndLevelUiCreator : MonoBehaviour
    {
        public Plugin plugin;

        private static EndLevelUiCreator instance;

        private static EndLevelViewController endLevelUI;


        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }

        public static void Show(FlowCoordinator fc)
        {

            endLevelUI = BeatSaberUI.CreateViewController<EndLevelViewController>();
            // Logger.log.Debug("MP GOT HERE");
         //   fc.InvokeMethod("SetTitle", new object[] {"BeatFollower", ViewController.AnimationType.None});
            fc.InvokeMethod("SetTopScreenViewController", new object[] { endLevelUI, ViewController.AnimationType.None });
        }

        public static void Create()
        {
            instance.StartCoroutine(instance.WaitForData());
        }

        private IEnumerator WaitForData()
        {
            endLevelUI = BeatSaberUI.CreateViewController<EndLevelViewController>();
            yield return null;
        }
    }
}
