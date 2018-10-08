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
    public class DeviceTypeDetailViewModel : INotifyPropertyChanged, IEditableObject
    {
        private DeviceTypeDataService deviceTypeDataService;

        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public DeviceType backupCopy { get; set; }
        private bool inEdit;


        private DeviceType selectedDeviceType;
        public ObservableCollection<Device> DevicesOfCurrentDeviceType { get; set; }

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

        public DeviceTypeDetailViewModel()
        {
            LoadCommands();
            deviceTypeDataService = new DeviceTypeDataService();
            Messenger.Default.Register<DeviceType>(this, OnDeviceTypeReceived);
        }

        private void OnDeviceTypeReceived(DeviceType deviceType)
        {
            SelectedDeviceType = deviceType;
            BeginEdit();
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
            CancelEdit();
            Messenger.Default.Send<UpdateListMessage>(new UpdateListMessage());
        }

        private bool CanDeleteDeviceType(object obj)
        {
            return true;
        }

        private void DeleteDeviceType(object obj)
        {
            deviceTypeDataService.DeleteDeviceType(SelectedDeviceType);
            Messenger.Default.Send<UpdateListMessage>(new UpdateListMessage());
        }

        private bool CanSaveDeviceType(object obj)
        {
            return true;
        }

        private void SaveDeviceType(object obj)
        {
            deviceTypeDataService.UpdateDeviceType(SelectedDeviceType, SelectedDeviceType.DeviceTypeId);
            Messenger.Default.Send<UpdateListMessage>(new UpdateListMessage());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void BeginEdit()
        {
            if (inEdit) return;
            inEdit = true;
            backupCopy = selectedDeviceType.Copy();
        }

        public void EndEdit()
        {
        }

        public void CancelEdit()
        {
            if (!inEdit) return;
            inEdit = false;
            SelectedDeviceType = backupCopy;
        }
    }
}
