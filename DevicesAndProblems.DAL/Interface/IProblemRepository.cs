using DevicesAndProblems.DAL.SQLite;
using DevicesAndProblems.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DevicesAndProblems.DAL.Interface
{
    public interface IProblemRepository
    {
        List<Problem> GetAll();
        List<Problem> GetById(int problemId);
        List<Problem> GetProblemsByDeviceId(int deviceId);
        void Add(Problem problem, ObservableCollection<Device> devicesOfProblem);
        void Update(Problem problem, ObservableCollection<Device> devicesOfProblem, int problemId);
        void Delete(Problem problem);


        ObservableCollection<string> GetComboboxItemsByComboboxType(ComboboxType type);
        ObservableCollection<int> GetComboboxMonthsByYear(int year);
        ObservableCollection<int> GetComboboxYears();
    }
}
