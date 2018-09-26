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
    public class DeviceTypeOverviewViewModel : INotifyPropertyChanged
    {
        private DeviceTypeDataService deviceTypeDataService = new DeviceTypeDataService();
        private EmployeeDataService currentEmployee;
        public static ObservableCollection<DeviceType> DeviceTypes { get; set; }

        public ICommand EditCommand { get; set; }

        public DeviceTypeOverviewViewModel()
        {
            LoadData();
            LoadCommands();

            Messenger.Default.Register<UpdateListMessage>(this, OnUpdateListMessageReceived);
        }

        private void OnUpdateListMessageReceived(UpdateListMessage obj)
        {
            LoadData();
        }

        private void LoadCommands()
        {
            EditCommand = new CustomCommand(EditDeviceType, CanEditDeviceType);
        }

        private void EditDeviceType(object obj)
        {
            Messenger.Default.Send<DeviceType>(selectedDeviceType);
        }

        private bool CanEditDeviceType(object obj)
        {
            return true;
        }

        private void LoadData()
        {
            DeviceTypes = deviceTypeDataService.GetAllDeviceTypes().ToObservableCollection();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        /* private ObservableCollection<DeviceType> deviceTypes;

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
       }*/
    }
}
