using System.ComponentModel;

namespace Model
{
    public class DeviceType : INotifyPropertyChanged
    {
        private int deviceTypeId;

        public int DeviceTypeId
        {
            get
            {
                return deviceTypeId;
            }
            set
            {
                deviceTypeId = value;
                RaisePropertyChanged("DeviceTypeId");
            }
        }

        private string deviceTypeName;

        public string DeviceTypeName
        {
            get
            {
                return deviceTypeName;
            }
            set
            {
                deviceTypeName = value;
                RaisePropertyChanged("DeviceTypeName");
            }
        }

        private string description;
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                RaisePropertyChanged("Description");
            }
        }

        private int deviceAmount;
        public int DeviceAmount
        {
            get
            {
                return deviceAmount;
            }
            set
            {
                deviceAmount = value;
                RaisePropertyChanged("DeviceAmount");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
