using System;
using System.ComponentModel;

namespace DevicesAndProblems.Model
{
    public class Problem : INotifyPropertyChanged
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

        private string raisedBy;
        public string RaisedBy
        {
            get
            {
                return raisedBy;
            }
            set
            {
                raisedBy = value;
                RaisePropertyChanged("RaisedBy");
            }
        }

        private int raisedByEmployeeId;
        public int RaisedByEmployeeId
        {
            get
            {
                return raisedByEmployeeId;
            }
            set
            {
                raisedByEmployeeId = value;
                RaisePropertyChanged("RaisedByEmployeeId");
            }
        }

        private int handledByEmployeeId;
        public int HandledByEmployeeId
        {
            get
            {
                return handledByEmployeeId;
            }
            set
            {
                handledByEmployeeId = value;
                RaisePropertyChanged("HandledByEmployeeId");
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

        private int priority;
        public int Priority
        {
            get
            {
                return priority;
            }
            set
            {
                priority = value;
                RaisePropertyChanged("Priority");
            }
        }

        private int severity;
        public int Severity
        {
            get
            {
                return severity;
            }
            set
            {
                severity = value;
                RaisePropertyChanged("Severity");
            }
        }

        private string status;
        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                RaisePropertyChanged("Status");
            }
        }

        public DateTime DateRaised
        {
            get;
            set;
        }

        private DateTime? closureDate;
        public DateTime? ClosureDate
        {
            get
            {
                return closureDate;
            }
            set
            {
                closureDate = value;
                RaisePropertyChanged("ClosureDate");
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
