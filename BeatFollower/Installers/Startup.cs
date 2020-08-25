using BeatSaberMarkupLanguage.MenuButtons;
using BeatSaberMarkupLanguage;
using BeatFollower.UI;

namespace BeatFollower.Installers
{
    public class Startup
    {
        private MenuButton _menuButton;

        public void Install()
        {
            var beatFollowerViewController = BeatSaberUI.CreateViewController<BeatFollowerViewController>();
        
        }

    public void AddButton()
        {
            if (_menuButton == null)
                _menuButton = new MenuButton("BeatFollower", "", SummonFlowCoordinator);

            MenuButtons.instance.RegisterButton(_menuButton);
        }

        public void RemoveButton()
        {
            MenuButtons.instance.UnregisterButton(_menuButton);
        }

        private void SummonFlowCoordinator()
        {
            var flowCoordinator = BeatSaberUI.CreateFlowCoordinator<ModFlowCoordinator>();
            if (flowCoordinator != null)
                    BeatSaberUI.MainFlowCoordinator.PresentFlowCoordinator(flowCoordinator);
        }
    }
}