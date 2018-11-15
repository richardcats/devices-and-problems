using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DevicesAndProblems.Model
{
    public class DeviceType : INotifyPropertyChanged, IEntity
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

            return Id == other.Id
                && Name == other.Name
                && Description == other.Description
                && DeviceAmount == other.DeviceAmount;
        }

        public override int GetHashCode()
        {
            var hashCode = 2089894652;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(description);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            hashCode = hashCode * -1521134295 + deviceAmount.GetHashCode();
            hashCode = hashCode * -1521134295 + DeviceAmount.GetHashCode();
            return hashCode;
        }
    }
}
