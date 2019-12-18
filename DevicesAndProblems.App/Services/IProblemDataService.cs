using DevicesAndProblems.DAL.SQLite;
using DevicesAndProblems.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DevicesAndProblems.App.Services
{
    public interface IProblemDataService
    {
        List<Problem> GetAllProblems();
        List<Problem> GetProblemsOfDevice(int deviceId);
        List<Device> GetDevicesOfProblem(int problemId);
        void AddProblem(Problem newProblem, ObservableCollection<Device> DevicesOfCurrentProblem);
        void UpdateProblem(Problem newProblem, int selectedProblemId, ObservableCollection<Device> DevicesOfCurrentProblem);
        void DeleteProblem(Problem selectedProblem);
        List<Comment> GetCommentsOfCurrentProblem(int id);
        void AddComment(Comment newComment, int selectedProblemId);
        void RemoveComment(Comment selectedComment, int selectedProblemId);
        ObservableCollection<string> FillCombobox(ComboboxType type);
        ObservableCollection<int> FillComboboxMonthsBasedOnYear(int selectedYear);
        ObservableCollection<int> FillComboboxYears();
    }
}
