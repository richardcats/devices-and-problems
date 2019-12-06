using DevicesAndProblems.DAL.SQLite;
using DevicesAndProblems.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DevicesAndProblems.DAL.Interface
{
    public interface IProblemRepository
    {
        List<Problem> GetAll();
        List<Problem> SelectProblemsByDeviceId(int id);
        List<Device> SelectDevicesByProblemId(int id);
        void Add(Problem newProblem, ObservableCollection<Device> DevicesOfCurrentProblem);
        void Update(Problem selectedProblem, Problem newProblem, ObservableCollection<Device> DevicesOfCurrentProblem);
        void DeleteProblem(Problem selectedProblem);
        void AddComment(Problem selectedProblem, Comment newComment);
        void RemoveComment(Comment selectedComment, Problem selectedProblem);
        List<Comment> GetCommentsOfCurrentProblem(Problem selectedProblem);
        ObservableCollection<string> GetComboboxItemsByComboboxType(ComboboxType type);
        ObservableCollection<int> GetComboboxMonthsByYear(int selectedYear);
        ObservableCollection<int> GetComboboxYears();
    }
}
