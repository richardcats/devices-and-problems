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

        public List<Device> GetDevicesOfCurrentProblem(int id)
        {
            conn.OpenConnection();
            return conn.GetDevicesOfCurrentProblem(id);
        }
        
        public void AddProblem(Problem newProblem, ObservableCollection<Device> DevicesOfCurrentProblem)
        {
            conn.OpenConnection();
            conn.AddProblem(newProblem, DevicesOfCurrentProblem);
        }

        public void UpdateProblem(Problem selectedProblem, Problem newProblem, ObservableCollection<Device> DevicesOfCurrentProblem)
        {
            conn.OpenConnection();
            conn.UpdateProblem(selectedProblem, newProblem, DevicesOfCurrentProblem);
        }

        public void DeleteProblem(Problem selectedProblem)
        {
            conn.OpenConnection();
            conn.DeleteProblem(selectedProblem);
        }

        public static ObservableCollection<string> FillCombobox(ComboboxType type)
        {
            // conn.OpenConnection();
            return DatabaseConnection.FillCombobox(type);
        }
    }
}
