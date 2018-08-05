using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesEnStoringen.Services
{
    public class ProblemDataService
    {
        DatabaseConnection conn = new DatabaseConnection();

        public List<Problem> GetAllProblems()
        {
            conn.OpenConnection();
            return conn.GetProblems();
        }

        public List<Problem> GetCurrentProblemsOfDevice(int id)
        {
            conn.OpenConnection();
            return conn.GetCurrentProblemsOfDevice(id);
        }

        public static ObservableCollection<string> FillCombobox(ComboboxType type)
        {
            // conn.OpenConnection();
            return DatabaseConnection.FillCombobox(type);
        }
    }
}
