
using DevicesAndProblems.App.View;
using DevicesAndProblems.DAL.Interface;
using DevicesAndProblems.DAL.SQLite;
using System.Collections.ObjectModel;

namespace DevicesAndProblems.App.Services
{
    public class EmployeeDataService : IEmployeeDataService
    {
        IEmployeeRepository repository;

        public EmployeeDataService(IEmployeeRepository repository, string emailaddress)
        {
            this.repository = repository;
            EmailAddress = emailaddress;
        }

        public EmployeeDataService(string emailaddress)
        {
            this.repository = new EmployeeRepository();
            EmailAddress = emailaddress;
        }

        public string EmailAddress { get; }

        public bool CheckLoginDetails(string emailaddress, string password)
        {
            return repository.CheckLoginDetails(emailaddress, password);
        }

        public string FirstNameOfCurrentEmployee()
        {
            return repository.FirstNameOfCurrentEmployee(EmailAddress);
        }

        public int IDOfCurrentEmployee()
        {
            return repository.IDOfCurrentEmployee(EmailAddress);
        }

        public string AccountTypeOfCurrentEmployee()
        {
            return repository.AccountTypeOfCurrentEmployee(EmailAddress);
        }

        public ObservableCollection<string> GetAllEmployees()
        {
            // return repository.FillCombobox(ComboboxType.Medewerker);3
            return null;
        }
    }
}
