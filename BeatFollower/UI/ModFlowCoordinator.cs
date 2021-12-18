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
		private FollowerListViewController _followerListViewController = null!;

		private MenuButton? _menuButton;

		[Inject]
		internal void Construct(MainFlowCoordinator mainFlowCoordinator, FollowerListViewController followerListViewController)
		{
			_mainFlowCoordinator = mainFlowCoordinator;
			_followerListViewController = followerListViewController;
		}

		public void Initialize()
		{
			_menuButton ??= new MenuButton(nameof(BeatFollower), Showtime);
			MenuButtons.instance.RegisterButton(_menuButton);
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

			ProvideInitialViewControllers(_followerListViewController);
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
			_mainFlowCoordinator.DismissFlowCoordinator(this);
		}
	}
}