using DevicesEnStoringen.Extensions;
using DevicesEnStoringen.Messages;
using DevicesEnStoringen.Services;
using DevicesEnStoringen.Utility;
using JoeCoffeeStore.StockManagement.App.Utility;
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
        public ICommand DeleteCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public DeviceTypeDetailViewModel()
        {
            LoadCommands();
            deviceTypeDataService = new DeviceTypeDataService();
            Messenger.Default.Register<DeviceType>(this, OnDeviceTypeReceived);
        }

        private void OnDeviceTypeReceived(DeviceType deviceType)
        {
            SelectedDeviceType = deviceType;
            SelectedDeviceTypeCopy = SelectedDeviceType.Copy(); // make a copy in case the user wants to cancel the change
            DevicesOfCurrentDeviceType = deviceTypeDataService.GetDevicesOfDeviceType(SelectedDeviceType.DeviceTypeId).ToObservableCollection();
        }

        private void LoadCommands()
        {
            SaveCommand = new CustomCommand(SaveDeviceType, CanSaveDeviceType);
            DeleteCommand = new CustomCommand(DeleteDeviceType, CanDeleteDeviceType);
            CancelCommand = new CustomCommand(Cancel, CanCancel);
        }

        private bool CanCancel(object obj)
        {
            return true;
        }

        private void Cancel(object obj)
        {
            Messenger.Default.Send(new UpdateListMessage());
        }

        private bool CanDeleteDeviceType(object obj)
        {
            return true;
        }

        private void DeleteDeviceType(object obj)
        {
            deviceTypeDataService.DeleteDeviceType(SelectedDeviceType);
            Messenger.Default.Send(new UpdateListMessage());
        }

        private bool CanSaveDeviceType(object obj)
        {
            return true;
        }

        private void SaveDeviceType(object obj)
        {
            SelectedDeviceType = SelectedDeviceTypeCopy;
            deviceTypeDataService.UpdateDeviceType(SelectedDeviceType, SelectedDeviceType.DeviceTypeId);
            Messenger.Default.Send(new UpdateListMessage());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
