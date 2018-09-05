using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesEnStoringen.Services
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
