using DevicesAndProblems.DAL.SQLite;
using DevicesAndProblems.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DevicesAndProblems.App.Services
{
    public interface IProblemDataService
    {
        List<Problem> GetAllProblems();
        List<Problem> GetCurrentProblemsOfDevice(int id);
        List<Device> GetDevicesOfCurrentProblem(int id);
        ObservableCollection<string> FillCombobox(ComboboxType type);
    }
}
