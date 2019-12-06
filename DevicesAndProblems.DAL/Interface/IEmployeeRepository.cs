using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesAndProblems.DAL.Interface
{
    public interface IEmployeeRepository
    {
        bool CheckLoginDetails(string emailaddress, string password);
        string FirstNameOfCurrentEmployee(string emailAddress);
        int IDOfCurrentEmployee(string emailAddress);
        string AccountTypeOfCurrentEmployee(string emailAddress);
    }
}
