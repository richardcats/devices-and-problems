using DevicesAndProblems.App.Extensions;
using DevicesAndProblems.App.Messages;
using DevicesAndProblems.App.Services;
using DevicesAndProblems.App.Utility;
using DevicesAndProblems.DAL.SQLite;
using DevicesAndProblems.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace DevicesAndProblems.App.ViewModel
{
    public class ProblemDetailViewModel : INotifyPropertyChanged
    {
        private IProblemDataService problemDataService;
        private IDeviceDataService deviceDataService;
        private IDialogService dialogService;

        private string title;
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                RaisePropertyChanged("Title");
            }
        }

        private SolidColorBrush blackText;
        public SolidColorBrush BlackText
        {
            get
            {
                return blackText;
            }
            set
            {
                blackText = value;
                RaisePropertyChanged("BlackText");
            }
        }


        private Problem selectedProblem;
        public Problem SelectedProblem
        {
            get
            {
                return selectedProblem;
            }
            set
            {
                selectedProblem = value;
                RaisePropertyChanged("SelectedProblem");
            }
        }

        public bool markRedIfFieldEmptyDescription;
        public bool MarkRedIfFieldEmptyDescription
        {
            get
            {
                return markRedIfFieldEmptyDescription;
            }
            set
            {
                markRedIfFieldEmptyDescription = value;
                RaisePropertyChanged("MarkRedIfFieldEmptyDescription");
            }
        }

        public bool markRedIfFieldEmptyDevices;
        public bool MarkRedIfFieldEmptyDevices
        {
            get
            {
                return markRedIfFieldEmptyDevices;
            }
            set
            {
                markRedIfFieldEmptyDevices = value;
                RaisePropertyChanged("MarkRedIfFieldEmptyDevices");
            }
        }

        public Problem SelectedProblemCopy { get; set; }
        public ObservableCollection<Device> AllDevices { get; set; }
        public ObservableCollection<Device> DevicesOfCurrentProblem { get; set; }
        public ObservableCollection<Comment> Comments { get; set; }

        public ObservableCollection<string> ComboBoxPriority { get; set; }
        public ObservableCollection<string> ComboBoxSeverity { get; set; }
        public ObservableCollection<string> ComboBoxStatus { get; set; }
        public ObservableCollection<string> ComboBoxHandledByEmployeeId { get; set; }

        private int currentEmployeeID;
        public static ObservableCollection<string> listStatus = FillCombobox(ComboboxType.Status);

        public ICommand AddCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand SaveWithoutCloseCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand CancelCommand { get; set; }


        public ProblemDetailViewModel(IDeviceDataService deviceDataService, IProblemDataService problemDataService, IDialogService dialogService)
        {
            this.deviceDataService = deviceDataService;
            this.problemDataService = problemDataService;
            this.dialogService = dialogService;

            LoadCommands();

            Messenger.Default.Register<OpenDetailViewMessage>(this, OnNewProblemWindow, ViewType.Problem);
            Messenger.Default.Register<Problem>(this, OnProblemReceived, ViewType.Problem);
        }

        // When showing the add new problem window, set the title, make all TextBlocks black and make sure all fields are empty
        private void OnNewProblemWindow(OpenDetailViewMessage message)
        {
            Title = "Storing registreren";

            MarkTextBlocksBlack();

            ComboBoxPriority = problemDataService.FillCombobox(ComboboxType.Department);
            ComboBoxSeverity = problemDataService.FillCombobox(ComboboxType.DeviceType);
            ComboBoxStatus = problemDataService.FillCombobox(ComboboxType.Department);
            ComboBoxHandledByEmployeeId = problemDataService.FillCombobox(ComboboxType.DeviceType);

            DevicesOfCurrentProblem = new ObservableCollection<Device>();
            AllDevices = deviceDataService.GetAllDevices().ToObservableCollection();

            currentEmployeeID = currentEmployee.IDOfCurrentEmployee();

            SelectedProblemCopy = new Problem()
            {
                Description = "",
                Priority = 0,
                Severity = 0,
                Status = "Open",
                HandledByEmployeeId = 0
            };
        }

        // When showing the edit problem window, set the title, make all TextBlocks black and set the selected problem
        private void OnProblemReceived(Problem problem)
        {
            Title = "Storing bewerken";

            MarkTextBlocksBlack();


            ComboBoxPriority = problemDataService.FillCombobox(ComboboxType.Department);
            ComboBoxSeverity = problemDataService.FillCombobox(ComboboxType.DeviceType);
            ComboBoxStatus = problemDataService.FillCombobox(ComboboxType.Department);
            ComboBoxHandledByEmployeeId = problemDataService.FillCombobox(ComboboxType.DeviceType);

            SelectedProblem = problem;
            SelectedProblemCopy = SelectedProblem.Copy(); // Creates a deep copy in case the user wants to cancel the change
                                                          

            Comments = problemDataService.GetCommentsOfCurrentProblem(SelectedProblem).ToObservableCollection();
            AllDevices = deviceDataService.GetAllDevices().ToObservableCollection();
            DevicesOfCurrentProblem = problemDataService.GetDevicesOfCurrentProblem(SelectedProblem.Id).ToObservableCollection();

        }

        private void LoadCommands()
        {
            AddCommand = new CustomCommand(AddProblem, CanAddProblem);
            SaveCommand = new CustomCommand(SaveProblem, CanSaveProblem);
            SaveWithoutCloseCommand = new CustomCommand(SaveProblemWithoutClose, CanSaveProblemWithoutClose);
            DeleteCommand = new CustomCommand(DeleteProblem, CanDeleteProblem);
            CancelCommand = new CustomCommand(Cancel, CanCancel);
        }

        private bool CanAddProblem(object obj)
        {
            return true;
        }

        private void AddProblem(object obj)
        {
            if (!CheckIfFieldsNotEmpty()) // Ensures that all required fields are filled in before inserting the problem into the database
            {
                dialogService.ShowEmptyFieldMessageBox();
                return;
            }
            else
            {
                problemDataService.AddProblem(SelectedProblemCopy, DevicesOfCurrentProblem);
                Messenger.Default.Send(new UpdateListMessage(true), ViewType.Problem);
            }
        }

        private void SaveProblemWithoutClose(object obj)
        {
            if (!CheckIfFieldsNotEmpty()) // Ensures that all required fields are filled in before updating the problem in the database
            {
                dialogService.ShowEmptyFieldMessageBox();
                return;
            }
            else
            {
                SelectedProblem = SelectedProblemCopy.Copy(); // Creates a deep copy so that CanSaveDeviceWithoutClose knows when a change is taking place in one of the fields again
                problemDataService.UpdateProblem(SelectedProblemCopy, SelectedProblemCopy.Id, DevicesOfCurrentProblem);
                Messenger.Default.Send(new UpdateListMessage(false), ViewType.Problem);
            }
        }

        // As soon as a change has occurred in one of the fields, the "submit" button will be enabled again
        private bool CanSaveProblemWithoutClose(object obj)
        {
            if (SelectedProblem != null && SelectedProblemCopy != null)
            {
                if (SelectedProblemCopy.Description != SelectedProblem.Description || SelectedProblemCopy.Priority != SelectedProblem.Priority
                    || SelectedProblemCopy.Severity != SelectedProblem.Severity || SelectedProblemCopy.Status != SelectedProblem.Status
                    || SelectedProblemCopy.ClosureDate != SelectedProblem.ClosureDate || SelectedProblemCopy.HandledByEmployeeId != SelectedProblem.HandledByEmployeeId)
                    return true;
            }
            return false;
        }

        private void SaveProblem(object obj)
        {
            if (!CheckIfFieldsNotEmpty()) // Ensures that all required fields are filled in before updating the problem in the database
            {
                dialogService.ShowEmptyFieldMessageBox();
                return;
            }
            else
            {
                SelectedProblem = SelectedProblemCopy;
                problemDataService.UpdateProblem(SelectedProblem, SelectedProblem.Id, DevicesOfCurrentProblem);
                Messenger.Default.Send(new UpdateListMessage(true), ViewType.Problem);
            }
        }

        private bool CanSaveProblem(object obj)
        {
            return true;
        }

        private bool CanCancel(object obj)
        {
            return true;
        }

        private void Cancel(object obj)
        {
            Messenger.Default.Send(new UpdateListMessage(true), ViewType.Problem);
        }

        private bool CanDeleteProblem(object obj)
        {
            return true;
        }

        private void DeleteProblem(object obj)
        {
            // The user first receives a message before the problem is permanently removed from the database
            if (dialogService.ShowRemoveWarningMessageBox("Problem", selectedProblem.Id))
            {
                problemDataService.DeleteProblem(SelectedProblem);
                Messenger.Default.Send(new UpdateListMessage(true), ViewType.Problem);
            }
        }

        // Fill the combobox based on the combobox type 
        public static ObservableCollection<string> FillCombobox(ComboboxType type)
        {
            ObservableCollection<string> comboboxItems = new ObservableCollection<string>();

            if (type == ComboboxType.Status)
            {
                comboboxItems = new ObservableCollection<string>();
                comboboxItems.Add("Open");
                comboboxItems.Add("In behandeling");
                comboboxItems.Add("Afgehandeld");
            }
            else if (type == ComboboxType.StatusAll)
            {
                comboboxItems = new ObservableCollection<string>();
                comboboxItems.Add("Alle storingen");
                comboboxItems.Add("Open");
                comboboxItems.Add("In behandeling");
                comboboxItems.Add("Afgehandeld");
            }

            else if (type == ComboboxType.Employee)
            {
                // EmployeeDataService employeeDataService = new EmployeeDataService();
                // comboboxItems = employeeDataService.GetAllEmployees();
            }

            else if (type == ComboboxType.PrioritySeverity)
            {
                comboboxItems = new ObservableCollection<string>();
                comboboxItems.Add("0");
                comboboxItems.Add("1");
                comboboxItems.Add("2");
                comboboxItems.Add("3");
            }
            return comboboxItems;
        }

        public bool CheckIfFieldsNotEmpty()
        {
            MarkTextBlocksBlack();
            bool noEmptyFields = true;
            if (SelectedProblemCopy.Description.Length == 0)
            {
                MarkRedIfFieldEmptyDescription = true; // By coloring it red, it allows the user to see which required fields must be filled 
                noEmptyFields = false;
            }

            if (DevicesOfCurrentProblem.Count == 0)
            {
                MarkRedIfFieldEmptyDevices = true; // By coloring it red, it allows the user to see which required fields must be filled 
                noEmptyFields = false;
            }


            if (noEmptyFields)
                return true;

            return false;
        }

        public void MarkTextBlocksBlack()
        {
            MarkRedIfFieldEmptyDescription = false;
            MarkRedIfFieldEmptyDevices = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
