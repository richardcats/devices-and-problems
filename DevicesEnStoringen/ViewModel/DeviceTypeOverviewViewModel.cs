﻿using DevicesEnStoringen.Extensions;
using DevicesEnStoringen.Messages;
using DevicesEnStoringen.Services;
using DevicesEnStoringen.Utility;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace DevicesEnStoringen.ViewModel
{
    public class DeviceTypeOverviewViewModel : OverviewViewModel, INotifyPropertyChanged
    {
        private DeviceTypeDataService deviceTypeDataService = new DeviceTypeDataService();
        private DialogService dialogService = new DialogService();

        private ObservableCollection<DeviceType> deviceTypes;

        public ObservableCollection<DeviceType> DeviceTypes
        {
           get
           {
                return deviceTypes;
           }
           set
           {
               deviceTypes = value;
               RaisePropertyChanged("DeviceTypes");
           }
        }

        private string searchInput = "";
        public string SearchInput
        {
            get
            {
                return searchInput;
            }
            set
            {
                searchInput = value;
                RaisePropertyChanged("SearchInput");
                FilterDataGrid();
            }
        } 

        private DeviceType selectedDeviceType;

        public DeviceType SelectedDeviceType
        {
            get
            {
                return selectedDeviceType;
            }
            set
            {
                selectedDeviceType = value;
                RaisePropertyChanged("SelectedDeviceType");
            }
        }

        private bool showEditButton;
        public bool ShowEditButton
        {
            get
            {
                return showEditButton;
            }
            set
            {
                showEditButton = value;
                RaisePropertyChanged("ShowEditButton");
            }
        }

        private bool showAddButton;
        public bool ShowAddButton
        {
            get
            {
                return showAddButton;
            }
            set
            {
                showAddButton = value;
                RaisePropertyChanged("ShowAddButton");
            }
        }
        

        public ICommand AddCommand { get; set; }

        public ICommand EditCommand { get; set; }

        public DeviceTypeOverviewViewModel()
        {
            LoadData();
            LoadCommands();

            Messenger.Default.Register<UpdateListMessage>(this, OnUpdateListMessageReceived);
            Messenger.Default.Register<string>(this, OnDeviceTypeOverviewOpened);
        }


        private void LoadData()
        {
            DeviceTypes = deviceTypeDataService.GetAllDeviceTypes().ToObservableCollection();
        }

        private void LoadCommands()
        {
            AddCommand = new CustomCommand(AddDeviceType, CanAddDeviceType);
            EditCommand = new CustomCommand(EditDeviceType, CanEditDeviceType);
        }

        private void FilterDataGrid()
        {
            ICollectionView DeviceTypesView = CollectionViewSource.GetDefaultView(DeviceTypes);
            var searchFilter = new Predicate<object>(item => ((DeviceType)item).DeviceTypeName.ToLower().Contains(SearchInput.ToLower()));
            DeviceTypesView.Filter = searchFilter;
        }

        private void OnUpdateListMessageReceived(UpdateListMessage obj)
        {
            LoadData();
            FilterDataGrid();

            if (obj.CloseScreen == true)
                dialogService.CloseDialog();
        }

        private void OnDeviceTypeOverviewOpened(string obj)
        {
            ShowAddButton = true;
            ShowEditButton = true;

            // temporary .. use this code for ProblemOverviewViewModel and DeviceOverviewViewModel instead
            /*if (CurrentEmployee.AccountTypeOfCurrentEmployee() == "IT-manager")
            {
                ShowAddButton = false;
                ShowEditButton = false;
            }
            else
            {
                ShowAddButton = true;
                ShowEditButton = true;
            }*/
        }

        private void AddDeviceType(object obj)
        {
            Messenger.Default.Send("NewDeviceType");
            dialogService.ShowAddDialog();
        }

        private bool CanAddDeviceType(object obj)
        {
            return true;
        }

        private void EditDeviceType(object obj)
        {
            Messenger.Default.Send(selectedDeviceType);
            Messenger.Default.Send(CurrentEmployee, "DeviceTypeDetailView");
            dialogService.ShowEditDialog();
        }

        private bool CanEditDeviceType(object obj)
        {
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
