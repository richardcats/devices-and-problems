using DevicesAndProblems.App.Extensions;
using DevicesAndProblems.App.Messages;
using DevicesAndProblems.App.Services;
using DevicesAndProblems.App.Utility;
using DevicesAndProblems.DAL.SQLite;
using DevicesAndProblems.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;


namespace DevicesAndProblems.App.ViewModel
{
    public class DeviceDetailViewModel : INotifyPropertyChanged
    {
        private IProblemDataService problemDataService;
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

        public bool markRedIfFieldEmptyDeviceType;
        public bool MarkRedIfFieldEmptyDeviceType
        {
            get
            {
                return markRedIfFieldEmptyDeviceType;
            }
            set
            {
                markRedIfFieldEmptyDeviceType = value;
                RaisePropertyChanged("MarkRedIfFieldEmptyDeviceType");
            }
        }

        public bool markRedIfFieldEmptyDepartment;
        public bool MarkRedIfFieldEmptyDepartment
        {
            get
            {
                return markRedIfFieldEmptyDepartment;
            }
            set
            {
                markRedIfFieldEmptyDepartment = value;
                RaisePropertyChanged("MarkRedIfFieldEmptyDepartment");
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

        public Device SelectedDeviceCopy { get; set; }
        public ObservableCollection<Problem> CurrentProblems { get; set; }

        public ObservableCollection<string> ComboBoxDeviceTypes { get; set; }
        public ObservableCollection<string> ComboBoxDepartments { get; set; }

        public ICommand AddCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand SaveWithoutCloseCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public DeviceDetailViewModel(IDeviceDataService deviceDataService, IProblemDataService problemDataService, IDialogService dialogService)
        {
            this.deviceDataService = deviceDataService;
            this.problemDataService = problemDataService;
            this.dialogService = dialogService;

            LoadCommands();

            Messenger.Default.Register<OpenDetailViewMessage>(this, OnNewDeviceWindow, ViewType.Device);
            Messenger.Default.Register<Device>(this, OnDeviceReceived, ViewType.DeviceType);
        }

        // When showing the add new device-type window, set the title, make all TextBlocks black and make sure all fields are empty
        private void OnNewDeviceWindow(OpenDetailViewMessage message)
        {
            Title = "Device registreren";

            MarkTextBlocksBlack();

            ComboBoxDeviceTypes = problemDataService.FillCombobox(ComboboxType.DeviceType);
            ComboBoxDepartments = problemDataService.FillCombobox(ComboboxType.Department);

            SelectedDeviceCopy = new Device()
            {
                Name = "",
                DeviceTypeName = "",
                Department = "",
                SerialNumber = "",
                Comments = ""
            };
        }

        // When showing the edit device-type window, set the title, make all TextBlocks black and set the selected devicetype
        private void OnDeviceReceived(Device device)
        {
            Title = "Device bewerken";

            MarkTextBlocksBlack();

            ComboBoxDeviceTypes = problemDataService.FillCombobox(ComboboxType.DeviceType);
            ComboBoxDepartments = problemDataService.FillCombobox(ComboboxType.Department);

            SelectedDevice = device;
            SelectedDeviceCopy = SelectedDevice.Copy(); // Creates a deep copy in case the user wants to cancel the change
            CurrentProblems = problemDataService.GetCurrentProblemsOfDevice(SelectedDevice.Id).ToObservableCollection();
        }

        private void LoadCommands()
        {
            AddCommand = new CustomCommand(AddDevice, CanAddDevice);
            SaveCommand = new CustomCommand(SaveDevice, CanSaveDevice);
            SaveWithoutCloseCommand = new CustomCommand(SaveDeviceWithoutClose, CanSaveDeviceWithoutClose);
            DeleteCommand = new CustomCommand(DeleteDevice, CanDeleteDevice);
            CancelCommand = new CustomCommand(Cancel, CanCancel);
        }

        private bool CanAddDevice(object obj)
        {
            return true;
        }

        private void AddDevice(object obj)
        {
            if (!CheckIfFieldsNotEmpty()) // Ensures that all required fields are filled in before inserting the device into the database
            {
                dialogService.ShowEmptyFieldMessageBox();
                return;
            }
            else
            {
                deviceDataService.AddDevice(SelectedDeviceCopy);
                Messenger.Default.Send(new UpdateListMessage(true), ViewType.Device);
            }
        }

        private void SaveDeviceWithoutClose(object obj)
        {
            if (!CheckIfFieldsNotEmpty()) // Ensures that all required fields are filled in before updating the device in the database
            {
                dialogService.ShowEmptyFieldMessageBox();
                return;
            }
            else
            {
                SelectedDevice = SelectedDeviceCopy.Copy(); // Creates a deep copy so that CanSaveDeviceWithoutClose knows when a change is taking place in one of the fields again
                deviceDataService.UpdateDevice(SelectedDeviceCopy, SelectedDeviceCopy.Id);
                Messenger.Default.Send(new UpdateListMessage(false), ViewType.Device);
            }
        }

        // As soon as a change has occurred in one of the fields, the "submit" button will be enabled again
        private bool CanSaveDeviceWithoutClose(object obj)
        {
            if (SelectedDevice != null && SelectedDeviceCopy != null)
            {
                if (SelectedDeviceCopy.Name != SelectedDevice.Name || SelectedDeviceCopy.DeviceTypeName != SelectedDevice.DeviceTypeName
                    || SelectedDeviceCopy.Department != SelectedDevice.Department || SelectedDeviceCopy.SerialNumber != SelectedDevice.SerialNumber ||
                    SelectedDeviceCopy.Comments != SelectedDevice.Comments)
                    return true;
            }
            return false;
        }

        private void SaveDevice(object obj)
        {
            if (!CheckIfFieldsNotEmpty()) // Ensures that all required fields are filled in before updating the device in the database
            {
                dialogService.ShowEmptyFieldMessageBox();
                return;
            }
            else
            {
                SelectedDevice = SelectedDeviceCopy;
                deviceDataService.UpdateDevice(SelectedDevice, SelectedDevice.Id);
                Messenger.Default.Send(new UpdateListMessage(true), ViewType.Device);
            }
        }

        private bool CanSaveDevice(object obj)
        {
            return true;
        }

        private bool CanCancel(object obj)
        {
            return true;
        }

        private void Cancel(object obj)
        {
            Messenger.Default.Send(new UpdateListMessage(true), ViewType.Device);
        }

        private bool CanDeleteDevice(object obj)
        {
            return true;
        }

        private void DeleteDevice(object obj)
        {
            // Prevent problems by having the user first remove the coupled problems
            if (selectedDevice.NumberOfFaults > 0)
            {
                dialogService.CanNotRemoveMessageBox("device", "problems");
                return;
            }

            // The user first receives a message before the device is permanently removed from the database
            if (dialogService.ShowRemoveWarningMessageBox("Device", selectedDevice.Id))
            {
                deviceDataService.DeleteDevice(SelectedDevice);
                Messenger.Default.Send(new UpdateListMessage(true), ViewType.Device);
            }
        }


        public bool CheckIfFieldsNotEmpty()
        {
            MarkTextBlocksBlack();
            bool noEmptyFields = true;
            if (SelectedDeviceCopy.Name.Length == 0)
            {
                MarkRedIfFieldEmptyName = true; // By coloring it red, it allows the user to see which required fields must be filled 
                noEmptyFields = false;
            }

            if (SelectedDeviceCopy.DeviceTypeName.Length == 0)
            {
                MarkRedIfFieldEmptyDeviceType = true; // By coloring it red, it allows the user to see which required fields must be filled 
                noEmptyFields = false;
            }

            if (SelectedDeviceCopy.Department.Length == 0)
            {
                MarkRedIfFieldEmptyDepartment = true; // By coloring it red, it allows the user to see which required fields must be filled 
                noEmptyFields = false;
            }

            if (noEmptyFields)
                return true;

            return false;
        }

        public void MarkTextBlocksBlack()
        {
            MarkRedIfFieldEmptyName = false;
            MarkRedIfFieldEmptyDeviceType = false;
            MarkRedIfFieldEmptyDepartment = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
