using System;
using System.ComponentModel;

namespace DevicesAndProblems.Model
{
    public class Problem : INotifyPropertyChanged
    {

        public int ProblemId
        {
            get;
            set;
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

        private int raisedByID;
        public int RaisedByID
        {
            get
            {
                return raisedByID;
            }
            set
            {
                raisedByID = value;
                RaisePropertyChanged("RaisedByID");
            }
        }

        private int handledBy;
        public int HandledBy
        {
            get
            {
                return handledBy;
            }
            set
            {
                handledBy = value;
                RaisePropertyChanged("HandledBy");
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
