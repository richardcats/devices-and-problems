using DevicesEnStoringen.Services;
using DevicesEnStoringen.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesEnStoringen.ViewModel
{
    public class OverviewViewModel : INotifyPropertyChanged
    {
        public EmployeeDataService CurrentEmployee { get; set; }
        private string loggedInAs;
        public string LoggedInAs
        {
            get
            {
                return loggedInAs;
            }
            set
            {
                loggedInAs = value;
                RaisePropertyChanged("LoggedInAs");
            }
        }

        public OverviewViewModel()
        {
            Messenger.Default.Register<EmployeeDataService>(this, OnCurrentEmployeeReceived);
        }

        private void OnCurrentEmployeeReceived(EmployeeDataService receivedEmployeeData)
        {
            CurrentEmployee = receivedEmployeeData;
            LoggedInAs = CurrentEmployee.FirstNameOfCurrentEmployee();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
