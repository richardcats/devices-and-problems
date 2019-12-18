
using DevicesAndProblems.App.View;
using DevicesAndProblems.DAL.Interface;
using DevicesAndProblems.DAL.SQLite;
using System.Collections.ObjectModel;

namespace DevicesAndProblems.App.Services
{
    public class EmployeeDataService : IEmployeeDataService
    {
        private IEmployeeRepository _employeeRepository;
        private IProblemRepository _problemRepository;
        public string EmailAddress { get; }

        public EmployeeDataService(IEmployeeRepository employeeRepository, IProblemRepository problemRepository, string emailaddress)
        {
            _employeeRepository = employeeRepository;
            _problemRepository = problemRepository;
            EmailAddress = emailaddress;
        }

        public EmployeeDataService(string emailaddress)
        {
            _employeeRepository = new EmployeeRepository();
            EmailAddress = emailaddress;
        }

        public bool CheckLoginDetails(string emailaddress, string password)
        {
            return _employeeRepository.CheckLoginDetails(emailaddress, password);
        }

        public string FirstNameOfCurrentEmployee()
        {
            return _employeeRepository.GetFirstNameByEmailAddress(EmailAddress);
        }

        public int IdOfCurrentEmployee()
        {
            return _employeeRepository.GetIdByEmailAddress(EmailAddress);
        }

        public string AccountTypeOfCurrentEmployee()
        {
            return _employeeRepository.GetAccountTypeByEmailAddress(EmailAddress);
        }

        public ObservableCollection<string> GetAllEmployees()
        {
            return _problemRepository.GetComboboxItemsByComboboxType(ComboboxType.Employee);
        }
    }
}
