using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesAndProblems.App.Services
{
    public interface IEmployeeDataService
    {
        bool CheckLoginDetails(string emailaddress, string password);
        string FirstNameOfCurrentEmployee();
        int IDOfCurrentEmployee();
        string AccountTypeOfCurrentEmployee();
    }
}
