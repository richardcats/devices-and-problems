using DevicesAndProblems.App.View;
using System.Collections.ObjectModel;

namespace DevicesAndProblems.App.Services
{
    public class EmployeeDataService
    {
        DatabaseConnection conn = new DatabaseConnection();
        public string EmailAddress { get; }

        public EmployeeDataService(string emailaddress)
        {
            EmailAddress = emailaddress;
        }

        public EmployeeDataService()
        {
        }

        public bool CheckLoginDetails(string emailaddress, string password)
        {
            return conn.CheckLoginDetails(emailaddress, password);
        }

        public string FirstNameOfCurrentEmployee()
        {
            return conn.FirstNameOfCurrentEmployee(EmailAddress);
        }

        public int IDOfCurrentEmployee()
        {
            return conn.IDOfCurrentEmployee(EmailAddress);
        }

        public string AccountTypeOfCurrentEmployee()
        {
            return conn.AccountTypeOfCurrentEmployee(EmailAddress);
        }

        public ObservableCollection<string> GetAllEmployees()
        {
            return conn.FillCombobox(ComboboxType.Medewerker);
        }
    }
}
