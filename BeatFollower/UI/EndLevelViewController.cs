using System;
using System.Collections.Generic;
using System.Linq;
using BeatFollower.Models;
using BeatFollower.Services;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;

namespace BeatFollower.UI
{
    [HotReload(@"C:\working\BeatFollowerPlugin\BeatFollower\UI\Views\EndLevel.bsml")]
    [ViewDefinition("BeatFollower.UI.Views.EndLevel.bsml")]
    public class EndLevelViewController : BSMLAutomaticViewController
    {
        [UIComponent("customlist-list")]
        public CustomCellListTableData customListsList;


        [UIValue("customlists")]
        public List<object> customListUi = new List<object>();

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);

            if (firstActivation)
            {
                CustomListService.GetCustomLists(SetCustomList);
            }

        } 
        [UIComponent("customlist-list2")]
        public CustomCellListTableData customListsList2;

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

        
        
        [UIValue("customlists2")]
        public List<object> customListUi2 = new List<object>();

        private void SetCustomList(List<CustomList> customLists)
        {
            customListUi.Clear();
            customListUi2.Clear();
            try
            {
                if (customLists != null)
                {
                    // max 10 so its 5x2
                    foreach (var customList in customLists.Take(10))
                    {
                        if (customListUi.Count >= 5)
                        {
                            customListUi.Add(new CustomListObject(customList));
                        }
                        else
                        {
                            customListUi2.Add(new CustomListObject(customList));
                        }
                    }

                    List1VisibleCells = customListUi.Count;
                    List2VisibleCells = customListUi2.Count;

                }
            }
            catch (Exception e)
            {
                Logger.log.Error(e);
            }

            customListsList.tableView.ReloadData();
            customListsList2.tableView.ReloadData();
        }
    }
}