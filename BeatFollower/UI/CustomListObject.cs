using System.ComponentModel;
using System.Runtime.CompilerServices;
using BeatFollower.Models;
using BeatFollower.Services;
using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;
using SiraUtil.Logging;

namespace BeatFollower.UI
{
	internal class CustomListObject : INotifyPropertyChanged
	{
		private readonly SiraLog _siraLog;
		private readonly ActivityService _activityService;
		private readonly LastBeatmapManager _lastBeatmapManager;

		private readonly string _id;

		[UIValue("list-name")]
		internal readonly string listName;

		public CustomListObject(SiraLog siraLog, ActivityService activityService, LastBeatmapManager lastBeatmapManager, CustomList customList)
		{
			_siraLog = siraLog;
			_activityService = activityService;
			_lastBeatmapManager = lastBeatmapManager;

			_id = customList._Id;
			listName = customList.Name;
		}

		[UIAction("list-share-pressed")]
		protected void ListSharePressed()
		{
			_siraLog.Debug($"Share Pressed: {listName} {_id}");

			ButtonInteractable = false;
			_activityService.SubmitRecommendation(_lastBeatmapManager.LastBeatmap, _id);
		}

		private bool _buttonInteractable = true;

		[UIValue("button-interactable")]
		public bool ButtonInteractable
		{
			get => _buttonInteractable;
			set
			{
				_buttonInteractable = value;
				NotifyPropertyChanged();
			}
		}

		public void Reset()
		{
			ButtonInteractable = true;
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}