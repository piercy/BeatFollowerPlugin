using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BeatFollower.Models;
using BeatFollower.Services;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using UnityEngine;
using Zenject;

namespace BeatFollower.UI
{
	[HotReload(RelativePathToLayout = @"Views\EndLevel.bsml")]
	[ViewDefinition("BeatFollower.UI.Views.EndLevel.bsml")]
	public class EndLevelViewController : BSMLAutomaticViewController
	{
		private DiContainer _container = null!;
		private CustomListService _customListService = null!;

		[UIComponent("customlist-list")] public RectTransform customListsListRect;

		[UIComponent("customlist-list")] public CustomCellListTableData customListsList;

		[UIValue("customlists")] public List<object> customListUi = new();

		[Inject]
		internal void Construct(DiContainer container, CustomListService customListService)
		{
			_container = container;
			_customListService = customListService;
		}

		protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
		{
			base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);

			if (firstActivation)
			{
				_customListService.GetCustomLists(SetCustomList);
			}
			else
			{
				StartCoroutine(RecalculateListWidths());
				ResetButtons();
			}
		}

		private void ResetButtons()
		{
			foreach (CustomListObject button in customListUi)
			{
				button.Reset();
			}

			foreach (CustomListObject button in customListUi2)
			{
				button.Reset();
			}
		}

		[UIComponent("customlist-list2")] public RectTransform customListList2Rect;

		[UIComponent("customlist-list2")] public CustomCellListTableData customListsList2;

		private int list1VisibleCells = 5;

		[UIValue("customlist-list-visiblecells")]
		public int List1VisibleCells
		{
			get => list1VisibleCells;
			set
			{
				list1VisibleCells = value;
				NotifyPropertyChanged();
			}
		}

		private int list2VisibleCells = 5;

		[UIValue("customlist-list2-visiblecells")]
		public int List2VisibleCells
		{
			get => list2VisibleCells;
			set
			{
				list2VisibleCells = value;
				NotifyPropertyChanged();
			}
		}

		[UIValue("customlists2")] public List<object> customListUi2 = new();

		private void SetCustomList(List<CustomList> customLists)
		{
			customListUi.Clear();
			customListUi2.Clear();

			// max 10 so its 5x2
			foreach (var customList in customLists.Take(10))
			{
				var customListObject = _container.Instantiate<CustomListObject>(new[] { customList });

				if (customListUi.Count < 5)
				{
					customListUi.Add(customListObject);
				}
				else
				{
					customListUi2.Add(customListObject);
				}
			}

			List1VisibleCells = customListUi.Count;
			List2VisibleCells = customListUi2.Count;

			StartCoroutine(RecalculateListWidths());
			customListsList.tableView.ReloadData();
			customListsList2.tableView.ReloadData();
		}

		private IEnumerator RecalculateListWidths()
		{
			yield return new WaitForEndOfFrame(); // Unity Moment
			// Manually set the list container to the correct size
			customListsListRect.sizeDelta = new Vector2(customListUi.Count * customListsList.cellSize, customListsListRect.sizeDelta.y);
			customListList2Rect.sizeDelta = new Vector2(customListUi2.Count * customListsList2.cellSize, customListList2Rect.sizeDelta.y);
		}
	}
}