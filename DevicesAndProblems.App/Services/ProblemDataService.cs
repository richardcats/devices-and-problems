using DevicesAndProblems.DAL.Interface;
using DevicesAndProblems.DAL.SQLite;
using DevicesAndProblems.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DevicesAndProblems.App.Services
{
    public class ProblemDataService : IProblemDataService
    {
        private IProblemRepository _problemRepository;
        private IDeviceRepository _deviceRepository;
        private ICommentRepository _commentRepository;

        public ProblemDataService(IProblemRepository problemRepository, IDeviceRepository deviceRepository, ICommentRepository commentRepository)
        {
            _problemRepository = problemRepository;
            _deviceRepository = deviceRepository;
            _commentRepository = commentRepository;
        }

        public List<Problem> GetAllProblems()
        {
            return _problemRepository.GetAll();
        }

        public List<Problem> GetProblemsOfDevice(int deviceId)
        {
            //return conn.GetCurrentProblemsOfDevice(id);
            return _problemRepository.GetProblemsByDeviceId(deviceId);
        }

        public List<Device> GetDevicesOfProblem(int problemId)
        {
            //return conn.GetDevicesOfCurrentProblem(id);
            return _deviceRepository.GetDevicesByProblemId(problemId);
        }
        
        public void AddProblem(Problem newProblem, ObservableCollection<Device> devicesOfCurrentProblem)
        {
            _problemRepository.Add(newProblem, devicesOfCurrentProblem);
            //conn.AddProblem(newProblem, DevicesOfCurrentProblem);
        }

        public void UpdateProblem(Problem newProblem, int selectedProblemId, ObservableCollection<Device> devicesOfCurrentProblem)
        {
            _problemRepository.Update(newProblem, devicesOfCurrentProblem, selectedProblemId);
            // conn.UpdateProblem(selectedProblem, newProblem, DevicesOfCurrentProblem);
        }

        public void DeleteProblem(Problem selectedProblem)
        {
            //conn.DeleteProblem(selectedProblem);
            _problemRepository.Delete(selectedProblem);
        }

        public void AddComment(Comment newComment, int selectedProblemId)
        {
            // conn.AddComment(selectedProblem, newComment);
            _commentRepository.Add(newComment, selectedProblemId);
        }

        public void RemoveComment(Comment selectedComment, int selectedProblemId)
        {
            // conn.RemoveComment(selectedComment, selectedProblem);
            _commentRepository.Delete(selectedComment, selectedProblemId);
        }

        public List<Comment> GetCommentsOfCurrentProblem(int selectedProblemId)
        {
            // return conn.GetCommentsOfCurrentProblem(selectedProblem);
            return _commentRepository.GetCommentsByProblemId(selectedProblemId);
        }

        public ObservableCollection<string> FillCombobox(ComboboxType type)
        {
            //return conn.FillCombobox(type);
            return _problemRepository.GetComboboxItemsByComboboxType(type);
        }

        public ObservableCollection<int> FillComboboxMonthsBasedOnYear(int selectedYear)
        {
            // return conn.FillComboboxMonthsBasedOnYear(selectedYear);
            return _problemRepository.GetComboboxMonthsByYear(selectedYear);
        }

        public ObservableCollection<int> FillComboboxYears()
        {
            // return conn.FillComboboxYears();
            return _problemRepository.GetComboboxYears();
        }
    }
}
