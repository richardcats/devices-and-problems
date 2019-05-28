using DevicesAndProblems.App.Extensions;
using DevicesAndProblems.App.Messages;
using DevicesAndProblems.App.Services;
using DevicesAndProblems.App.Utility;
using DevicesAndProblems.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;


namespace DevicesAndProblems.App.ViewModel
{
    public class DeviceDetailViewModel : INotifyPropertyChanged
    {
        private IDeviceTypeDataService deviceTypeDataService;
        private IDeviceDataService deviceDataService;
        private IDialogService dialogService;

        private string title;
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                RaisePropertyChanged("Title");
            }
        }

        private SolidColorBrush blackText;
        public SolidColorBrush BlackText
        {
            get
            {
                return blackText;
            }
            set
            {
                blackText = value;
                RaisePropertyChanged("BlackText");
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

        public bool markRedIfFieldEmptyName;
        public bool MarkRedIfFieldEmptyName
        {
            get
            {
                return markRedIfFieldEmptyName;
            }
            set
            {
                markRedIfFieldEmptyName = value;
                RaisePropertyChanged("MarkRedIfFieldEmptyName");
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

        public DeviceType SelectedDeviceTypeCopy { get; set; }
        public ObservableCollection<Device> DevicesOfCurrentDeviceType { get; set; }

        public ICommand AddCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand SaveWithoutCloseCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public DeviceDetailViewModel(IDeviceDataService deviceDataService, IDeviceTypeDataService deviceTypeDataService, IDialogService dialogService)
        {
            this.deviceDataService = deviceDataService;
            this.deviceTypeDataService = deviceTypeDataService;
            this.dialogService = dialogService;

           // LoadCommands();

            //Messenger.Default.Register<string>(this, OnNewDeviceTypeWindow);
            //Messenger.Default.Register<DeviceType>(this, OnDeviceTypeReceived);
           // Messenger.Default.Register<EmployeeDataService>(this, OnCurrentEmployeeReceived, "DeviceTypeDetailView");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
