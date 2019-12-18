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
        string GetFirstNameByEmailAddress(string emailAddress);
        int GetIdByEmailAddress(string emailAddress);
        string GetAccountTypeByEmailAddress(string emailAddress);
    }
}
