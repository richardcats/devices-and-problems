using System;
using System.ComponentModel;

namespace DevicesAndProblems.Model
{
    public class Device : INotifyPropertyChanged
    {
        private int id;

        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                RaisePropertyChanged("Id");
            }
        }


        private string name;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                RaisePropertyChanged("Name");
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
