using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DevicesAndProblems.Model
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

        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;

            DeviceType other = obj as DeviceType;
            if ((Object)other == null)
                return false;

            return DeviceTypeId == other.DeviceTypeId
                && DeviceTypeName == other.DeviceTypeName
                && Description == other.Description
                && DeviceAmount == other.DeviceAmount;
        }

        public override int GetHashCode()
        {
            var hashCode = 2089894652;
            hashCode = hashCode * -1521134295 + deviceTypeId.GetHashCode();
            hashCode = hashCode * -1521134295 + DeviceTypeId.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(deviceTypeName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DeviceTypeName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(description);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            hashCode = hashCode * -1521134295 + deviceAmount.GetHashCode();
            hashCode = hashCode * -1521134295 + DeviceAmount.GetHashCode();
            return hashCode;
        }
    }
}
