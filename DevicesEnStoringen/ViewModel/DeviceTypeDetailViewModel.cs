using DevicesEnStoringen.Extensions;
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
using System.Windows.Input;

namespace DevicesEnStoringen.ViewModel
{
    public class DeviceTypeDetailViewModel : INotifyPropertyChanged
    {
        private DeviceTypeDataService deviceTypeDataService;
        private DialogService dialogService = new DialogService();

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

        public DeviceType SelectedDeviceTypeCopy { get; set; }
        public ObservableCollection<Device> DevicesOfCurrentDeviceType { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand SaveWithoutCloseCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public DeviceTypeDetailViewModel()
        {
            LoadCommands();
            deviceTypeDataService = new DeviceTypeDataService();
            Messenger.Default.Register<DeviceType>(this, OnDeviceTypeReceived);
            //SelectedDeviceType.DeviceTypeName = "";
            //SelectedDeviceTypeCopy.DeviceTypeName = "";
        }

        private void OnDeviceTypeReceived(DeviceType deviceType)
        {
            SelectedDeviceType = deviceType;
            SelectedDeviceTypeCopy = SelectedDeviceType.Copy(); // Creates a deep copy in case the user wants to cancel the change
            DevicesOfCurrentDeviceType = deviceTypeDataService.GetDevicesOfDeviceType(SelectedDeviceType.DeviceTypeId).ToObservableCollection();
        }

        private void LoadCommands()
        {
            SaveCommand = new CustomCommand(SaveDeviceType, CanSaveDeviceType);
            SaveWithoutCloseCommand = new CustomCommand(SaveDeviceTypeWithoutClose, CanSaveDeviceType);
            DeleteCommand = new CustomCommand(DeleteDeviceType, CanDeleteDeviceType);
            CancelCommand = new CustomCommand(Cancel, CanCancel);
        }

        private void SaveDeviceTypeWithoutClose(object obj)
        {
            SelectedDeviceType = SelectedDeviceTypeCopy.Copy(); // Creates a deep copy so that CanSaveDeviceType knows when a change is taking place in one of the fields again
            deviceTypeDataService.UpdateDeviceType(SelectedDeviceTypeCopy, SelectedDeviceTypeCopy.DeviceTypeId);
            Messenger.Default.Send(new UpdateListMessage(false));
        }

        private bool CanCancel(object obj)
        {
            return true;
        }

        private void Cancel(object obj)
        {
            Messenger.Default.Send(new UpdateListMessage(true));
        }

        private bool CanDeleteDeviceType(object obj)
        {
            return true;
        }

        private void DeleteDeviceType(object obj)
        {
            if (dialogService.ShowDeleteWarningMessageBox("Device-type", selectedDeviceType.DeviceTypeId))
            {
                deviceTypeDataService.DeleteDeviceType(SelectedDeviceType);
                Messenger.Default.Send(new UpdateListMessage(true));
            }
        }

        // As soon as a change has occurred in one of the fields, the "OK" and "submit" button will be enabled again
        private bool CanSaveDeviceType(object obj)
        {
            if (SelectedDeviceTypeCopy.DeviceTypeName != SelectedDeviceType.DeviceTypeName || SelectedDeviceTypeCopy.Description != SelectedDeviceType.Description)
                return true;
            return false;
        }

        private void SaveDeviceType(object obj)
        {
            SelectedDeviceType = SelectedDeviceTypeCopy;
            deviceTypeDataService.UpdateDeviceType(SelectedDeviceType, SelectedDeviceType.DeviceTypeId);
            Messenger.Default.Send(new UpdateListMessage(true));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
