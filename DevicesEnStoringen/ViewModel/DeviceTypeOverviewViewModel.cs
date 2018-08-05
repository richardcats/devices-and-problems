using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesEnStoringen.ViewModel
{
    public class DeviceTypeOverviewViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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

        public DeviceTypeOverviewViewModel()
        {
            LoadData();
        }

        private void LoadData()
        {
            throw new NotImplementedException();
        }
    }
}
