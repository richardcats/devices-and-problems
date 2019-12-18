using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesAndProblems.App.Services
{
    public interface IEmployeeDataService
    {
        bool CheckLoginDetails(string emailaddress, string password);
        string FirstNameOfCurrentEmployee();
        int IdOfCurrentEmployee();
        string AccountTypeOfCurrentEmployee();
        ObservableCollection<string> GetAllEmployees();
    }
}
