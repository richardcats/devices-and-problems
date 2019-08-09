using DevicesAndProblems.App.View;
using DevicesAndProblems.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
