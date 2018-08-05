using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Device : INotifyPropertyChanged
    {

        private int deviceId;

        public int DeviceId
        {
            get
            {
                return deviceId;
            }
            set
            {
                deviceId = value;
                RaisePropertyChanged("DeviceId");
            }
        }

        private string deviceName;

        public string DeviceName
        {
            get
            {
                return deviceName;
            }
            set
            {
                deviceName = value;
                RaisePropertyChanged("DeviceName");
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

        private int deviceTypeValue;

        public int DeviceTypeValue
        {
            get
            {
                return deviceTypeValue;
            }
            set
            {
                deviceTypeValue = value;
                RaisePropertyChanged("DeviceTypeValue");
            }
        }

        

        private string serialNumber;
        public string SerialNumber
        {
            get
            {
                return serialNumber;
            }
            set
            {
                serialNumber = value;
                RaisePropertyChanged("DeviceName");
            }
        }

        private string department;
        public string Department
        {
            get
            {
                return department;
            }
            set
            {
                department = value;
                RaisePropertyChanged("Department");
            }
        }

        private string comments;
        public string Comments
        {
            get
            {
                return comments;
            }
            set
            {
                comments = value;
                RaisePropertyChanged("Comments");
            }
        }

        public DateTime FirstAddedDate
        {
            get;
            set;
        }

        public int NumberOfFaults
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
