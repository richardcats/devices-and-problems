using DevicesEnStoringen.Messages;
using DevicesEnStoringen.Utility;
using JoeCoffeeStore.StockManagement.App.Utility;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DevicesEnStoringen.ViewModel
{
    public class DeviceTypeDetailViewModel : INotifyPropertyChanged
    {
        public DeviceTypeDetailViewModel()
        {
            LoadData();
            LoadCommands();

            Messenger.Default.Register<DeviceType>(this, OnDeviceTypeReceived);
        }

        private void OnDeviceTypeReceived(DeviceType deviceType)
        {
            SelectedDeviceType = deviceType;
        }

        private void LoadCommands()
        {
            SaveCommand = new CustomCommand(SaveDeviceType, CanSaveDeviceType);
            DeleteCommand = new CustomCommand(DeleteDeviceType, CanDeleteDeviceType);
        }

        private bool CanDeleteDeviceType(object obj)
        {
            return true;
        }

        private void DeleteDeviceType(object obj)
        {
            Messenger.Default.Send<UpdateListMessage>(new UpdateListMessage());
        }

        private bool CanSaveDeviceType(object obj)
        {
            return true;
        }

        private void SaveDeviceType(object obj)
        {
            Messenger.Default.Send<UpdateListMessage>(new UpdateListMessage());
        }

        private void LoadData()
        {
          //  DeviceTypes = deviceTypeDataService.GetAllDeviceTypes().ToObservableCollection();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

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
    }
}
