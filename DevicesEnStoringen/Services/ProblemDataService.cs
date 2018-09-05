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
            //conn.OpenConnection();
            conn.AddProblem(newProblem, DevicesOfCurrentProblem);
        }

        public void UpdateProblem(Problem selectedProblem, Problem newProblem, ObservableCollection<Device> DevicesOfCurrentProblem)
        {
            //conn.OpenConnection();
            conn.UpdateProblem(selectedProblem, newProblem, DevicesOfCurrentProblem);
        }

        public void DeleteProblem(Problem selectedProblem)
        {
            //conn.OpenConnection();
            conn.DeleteProblem(selectedProblem);
        }

        public void AddComment(Problem selectedProblem, Comment newComment)
        {
            //conn.OpenConnection();
            conn.AddComment(selectedProblem, newComment);
        }

        public void RemoveComment(Comment selectedComment, Problem selectedProblem)
        {
            //conn.OpenConnection();
            conn.RemoveComment(selectedComment, selectedProblem);
        }

        public List<Comment> GetCommentsOfCurrentProblem(Problem selectedProblem)
        {
            //conn.OpenConnection();
            return conn.GetCommentsOfCurrentProblem(selectedProblem);
        }

        public ObservableCollection<string> FillCombobox(ComboboxType type)
        {
            // conn.OpenConnection();
            return conn.FillCombobox(type);
        }
    }
}
