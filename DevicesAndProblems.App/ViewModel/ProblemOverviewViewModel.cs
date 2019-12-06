using DevicesAndProblems.App.Extensions;
using DevicesAndProblems.App.Messages;
using DevicesAndProblems.App.Services;
using DevicesAndProblems.App.Utility;
using DevicesAndProblems.App.View;
using DevicesAndProblems.DAL.SQLite;
using DevicesAndProblems.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace DevicesAndProblems.App.ViewModel
{
    public class ProblemOverviewViewModel : OverviewViewModel, INotifyPropertyChanged
    {
        private IProblemDataService problemDataService;
        private IDialogService dialogService;

        public ObservableCollection<string> ComboboxProblemStatus { get; set; }

        private ObservableCollection<Problem> problems;

        public ObservableCollection<Problem> Problems
        {
            get
            {
                return problems;
            }
            set
            {
                problems = value;
                RaisePropertyChanged("Problems");
            }
        }

        private string searchInput = "";
        public string SearchInput
        {
            get
            {
                return searchInput;
            }
            set
            {
                searchInput = value;
                RaisePropertyChanged("SearchInput");
                FilterDataGrid();
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

        private string selectedProblemStatusName;

        public string SelectedProblemStatusName
        {
            get
            {
                return selectedProblemStatusName;
            }
            set
            {
                selectedProblemStatusName = value;
                RaisePropertyChanged("SelectedProblemStatusName");
                FilterDataGrid();
            }
        }

        private bool showEditButton;
        public bool ShowEditButton
        {
            get
            {
                return showEditButton;
            }
            set
            {
                showEditButton = value;
                RaisePropertyChanged("ShowEditButton");
            }
        }

        private bool showAddButton;
        public bool ShowAddButton
        {
            get
            {
                return showAddButton;
            }
            set
            {
                showAddButton = value;
                RaisePropertyChanged("ShowAddButton");
            }
        }


        public ICommand AddCommand { get; set; }

        public ICommand EditCommand { get; set; }

        public ProblemOverviewViewModel(IProblemDataService problemDataService, IDialogService dialogService)
        {
            this.problemDataService = problemDataService;
            this.dialogService = dialogService;

            LoadData();
            LoadCommands();

            Messenger.Default.Register<UpdateListMessage>(this, OnUpdateListMessageReceived, ViewType.Device);
            Messenger.Default.Register<OpenOverviewMessage>(this, OnProblemOverviewOpened, ViewType.Device);
        }

        private void LoadData()
        {
            Problems = problemDataService.GetAllProblems().ToObservableCollection();
            ComboboxProblemStatus = ProblemDetailView.FillCombobox(ComboboxType.StatusAll);
        }

        private void LoadCommands()
        {
            AddCommand = new CustomCommand(AddProblem, CanAddProblem);
            EditCommand = new CustomCommand(EditProblem, CanEditProblem);
        }

        private void FilterDataGrid()
        {
            ICollectionView ProblemsView = CollectionViewSource.GetDefaultView(Problems);
            if (SelectedProblemStatusName == null || SelectedProblemStatusName == "Alle storingen")
            {
                var searchFilter = new Predicate<object>(item => ((Problem)item).Description.ToLower().Contains(SearchInput.ToLower()));
                ProblemsView.Filter = searchFilter;
            }
            else
            {
                var searchFilter = new Predicate<object>(item => ((Problem)item).Description.ToLower().Contains(SearchInput.ToLower()) && ((Problem)item).Status == SelectedProblemStatusName);
                ProblemsView.Filter = searchFilter;
            }
        }

        private void OnUpdateListMessageReceived(UpdateListMessage obj)
        {
            LoadData();
            FilterDataGrid();

            if (obj.CloseScreen == true)
                dialogService.CloseDialog();
        }

        private void OnProblemOverviewOpened(OpenOverviewMessage obj)
        {
            // temporary .. use this code for ProblemOverviewViewModel and DeviceOverviewViewModel instead
            if (CurrentEmployee.AccountTypeOfCurrentEmployee() == "IT-manager")
            {
                ShowAddButton = false;
                ShowEditButton = false;
            }
            else
            {
                ShowAddButton = true;
                ShowEditButton = true;
            }
        }

        private void AddProblem(object obj)
        {
            Messenger.Default.Send(new OpenDetailViewMessage(), ViewType.Problem);
            dialogService.ShowAddDialog(ViewType.Problem);
        }

        private bool CanAddProblem(object obj)
        {
            return true;
        }

        private void EditProblem(object obj)
        {
            Messenger.Default.Send(selectedProblem, ViewType.Problem);
            dialogService.ShowEditDialog(ViewType.Problem);
        }

        private bool CanEditProblem(object obj)
        {
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
