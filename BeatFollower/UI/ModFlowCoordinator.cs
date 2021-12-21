using System;
using HMUI;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.MenuButtons;
using Zenject;

namespace BeatFollower.UI
{
	internal class ModFlowCoordinator : FlowCoordinator, IInitializable, IDisposable
	{
		private MainFlowCoordinator _mainFlowCoordinator = null!;
		private SettingsMenuViewController _settingsMenuViewController = null!;
		private FollowerListViewController _followerListViewController;

		private MenuButton? _menuButton;

		[Inject]
		internal void Construct(MainFlowCoordinator mainFlowCoordinator, SettingsMenuViewController settingsMenuViewController, FollowerListViewController followerListViewController)
		{
			_mainFlowCoordinator = mainFlowCoordinator;
			_settingsMenuViewController = settingsMenuViewController;
			_followerListViewController = followerListViewController;
		}

		public void Initialize()
		{
			_menuButton ??= new MenuButton(nameof(BeatFollower), Showtime);
			MenuButtons.instance.RegisterButton(_menuButton);
			_settingsMenuViewController.ShowFollowerListEvent += ShowFollowerListEvent;
		}

		private void ShowFollowerListEvent(object sender, EventArgs e)
		{
			PresentViewController(_followerListViewController);
		}

		private void Showtime()
		{
			_mainFlowCoordinator.PresentFlowCoordinator(this);

		}

		protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
		{
			if (firstActivation)
			{
				SetTitle(nameof(BeatFollower));
				showBackButton = true;
			}

			ProvideInitialViewControllers(_settingsMenuViewController);
		}

		public void Dispose()
		{
			if (_menuButton == null)
			{
				return;
			}

			if (MenuButtons.IsSingletonAvailable && BSMLParser.IsSingletonAvailable)
			{
				MenuButtons.instance.UnregisterButton(_menuButton);
			}

			_menuButton = null!;
		}

		protected override void BackButtonWasPressed(ViewController _)
		{
			if (_followerListViewController.isActivated)
			{
				DismissViewController(_followerListViewController);
			}
			else
			{
				//DismissViewController(_settingsMenuViewController);
				_mainFlowCoordinator.DismissFlowCoordinator(this);
			}
		}
	}
}