using DevicesAndProblems.App.Services;
using DevicesAndProblems.App.Utility;
using System.ComponentModel;

namespace DevicesAndProblems.App.ViewModel
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
