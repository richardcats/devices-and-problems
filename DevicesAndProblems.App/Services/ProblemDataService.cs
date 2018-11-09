using DevicesAndProblems.App.View;
using DevicesAndProblems.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DevicesAndProblems.App.Services
{
    public class ProblemDataService
    {
        DatabaseConnection conn = new DatabaseConnection();

        public List<Problem> GetAllProblems()
        {
            return conn.GetProblems();
        }

        public List<Problem> GetCurrentProblemsOfDevice(int id)
        {
            return conn.GetCurrentProblemsOfDevice(id);
        }

        public List<Device> GetDevicesOfCurrentProblem(int id)
        {
            return conn.GetDevicesOfCurrentProblem(id);
        }
        
        public void AddProblem(Problem newProblem, ObservableCollection<Device> DevicesOfCurrentProblem)
        {
            conn.AddProblem(newProblem, DevicesOfCurrentProblem);
        }

        public void UpdateProblem(Problem selectedProblem, Problem newProblem, ObservableCollection<Device> DevicesOfCurrentProblem)
        {
            conn.UpdateProblem(selectedProblem, newProblem, DevicesOfCurrentProblem);
        }

        public void DeleteProblem(Problem selectedProblem)
        {
            conn.DeleteProblem(selectedProblem);
        }

        public void AddComment(Problem selectedProblem, Comment newComment)
        {
            conn.AddComment(selectedProblem, newComment);
        }

        public void RemoveComment(Comment selectedComment, Problem selectedProblem)
        {
            conn.RemoveComment(selectedComment, selectedProblem);
        }

        public List<Comment> GetCommentsOfCurrentProblem(Problem selectedProblem)
        {
            return conn.GetCommentsOfCurrentProblem(selectedProblem);
        }

        public ObservableCollection<string> FillCombobox(ComboboxType type)
        {
            return conn.FillCombobox(type);
        }

        public ObservableCollection<int> FillComboboxMonthsBasedOnYear(int selectedYear)
        {
            return conn.FillComboboxMonthsBasedOnYear(selectedYear);
        }

        public ObservableCollection<int> FillComboboxYears()
        {
            return conn.FillComboboxYears();
        }
    }
}
