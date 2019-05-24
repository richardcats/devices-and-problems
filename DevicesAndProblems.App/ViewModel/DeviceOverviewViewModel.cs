using DevicesAndProblems.App.Extensions;
using DevicesAndProblems.App.Messages;
using DevicesAndProblems.App.Services;
using DevicesAndProblems.App.Utility;
using DevicesAndProblems.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace DevicesAndProblems.App.ViewModel
{
    public class DeviceOverviewViewModel : OverviewViewModel, INotifyPropertyChanged
    {
        private IDeviceDataService deviceDataService;
        private IDialogService dialogService;

        private ObservableCollection<Device> devices;

        public ObservableCollection<Device> Devices
        {
            get
            {
                return devices;
            }
            set
            {
                devices = value;
                RaisePropertyChanged("Devices");
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

        private Device selectedDevice;

        public Device SelectedDevice
        {
            get
            {
                return selectedDevice;
            }
            set
            {
                selectedDevice = value;
                RaisePropertyChanged("SelectedDevice");
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

        public DeviceOverviewViewModel(IDeviceDataService deviceDataService, IDialogService dialogService)
        {
            this.deviceDataService = deviceDataService;
            this.dialogService = dialogService;

            LoadData();
            LoadCommands();

            Messenger.Default.Register<UpdateListMessage>(this, OnUpdateListMessageReceived);
            Messenger.Default.Register<OpenOverviewMessage>(this, OnDeviceOverviewOpened, "Devices");
        }

        private void LoadData()
        {
            Devices = deviceDataService.GetAllDevices().ToObservableCollection();
        }


        private void LoadCommands()
        {
            AddCommand = new CustomCommand(AddDevice, CanAddDevice);
            EditCommand = new CustomCommand(EditDevice, CanEditDevice);
        }

        private void FilterDataGrid()
        {
            ICollectionView DeviceTypesView = CollectionViewSource.GetDefaultView(Devices);
            var searchFilter = new Predicate<object>(item => ((Device)item).Name.ToLower().Contains(SearchInput.ToLower()));
            DeviceTypesView.Filter = searchFilter;
        }

        private void OnUpdateListMessageReceived(UpdateListMessage obj)
        {
            LoadData();
            FilterDataGrid();

            if (obj.CloseScreen == true)
                dialogService.CloseDialog();
        }

        private void OnDeviceOverviewOpened(OpenOverviewMessage obj)
        {
            // temporary .. use this code for ProblemOverviewViewModel and DeviceOverviewViewModel instead
            if (CurrentEmployee.AccountTypeOfCurrentEmployee() == "IT-manager")
            {
                ShowAddButton = false;
                ShowEditButton = false;
            }
            else
            {
                ShowAddButton = true;
                ShowEditButton = true;
            }
        }

        private void AddDevice(object obj)
        {
            Messenger.Default.Send("NewDevice");
            dialogService.ShowAddDialog();
        }

        private bool CanAddDevice(object obj)
        {
            return true;
        }

        private void EditDevice(object obj)
        {
            Messenger.Default.Send(selectedDevice);
            Messenger.Default.Send(CurrentEmployee, "DeviceDetailView");
            dialogService.ShowEditDialog();
        }

        private bool CanEditDevice(object obj)
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
