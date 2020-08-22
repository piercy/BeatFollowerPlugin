using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using UnityEngine;

namespace BeatFollower.UI
{
    [HotReload(@"C:\working\BeatFollowerPlugin\BeatFollower\UI\BeatFollowerView.bsml")]
    [ViewDefinition("BeatFollower.UI.BeatFollowerView.bsml")]
    public class BeatFollowerViewController : BSMLAutomaticViewController // BSMLResourceViewController
    {
      //  public override string ResourceName => "BeatFollower.UI.BeatFollowerView.bsml";
      [UIObject("ScrollContent")] internal GameObject ScrollContent;

        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
            Logger.log.Debug("Im in the VC");
            base.DidActivate(firstActivation, activationType);
            
            Logger.log.Debug("Im in the VC1");
            for (int i = 0; i < 10; i++)
            {
                var res = BeatSaberMarkupLanguage.Utilities.GetResourceContent(Assembly.GetExecutingAssembly(), "BeatFollower.UI.Follower.bsml");
                if(res == null)
                    Logger.log.Error("Res is null");

                BSMLParser.instance.Parse(res, ScrollContent, this);
                
            }

          
        }

    }
}