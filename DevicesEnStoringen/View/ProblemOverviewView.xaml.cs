using DevicesEnStoringen.Extensions;
using DevicesEnStoringen.Services;
using Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
namespace DevicesEnStoringen
{
    public partial class ProblemOverviewView : UserControl
    {
        private ProblemDataService problemDataService = new ProblemDataService();
        private Employee currentEmployee;

        public ObservableCollection<string> ComboboxProblemStatus { get; set; }
        public static ObservableCollection<Problem> Problems { get; set; }

        public ProblemOverviewView(Employee currentEmployee)
        {
            InitializeComponent();

            this.currentEmployee = currentEmployee;
            Problems = problemDataService.GetAllProblems().ToObservableCollection();
            ComboboxProblemStatus = ProblemDetailView.FillCombobox(ComboboxType.StatusAll);
            Loaded += ProblemOverviewView_Loaded;

            if (currentEmployee.AccountTypeOfCurrentEmployee() == "IT-manager")
            {
                btnRegistreerStoring.Visibility = Visibility.Hidden;
                dgStoringen.Columns[6].Visibility = Visibility.Hidden;
            }
        }

        private void ProblemOverviewView_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = this;
        }

        // When the IT administrator clicks on a malfunction, it will pass the ID to a new window
        private void RowButtonClick(object sender, RoutedEventArgs e)
        {
            Problem selectedProblem = (Problem)dgStoringen.SelectedItems[0];
            ProblemDetailView problemDetailView = new ProblemDetailView(selectedProblem)
            {
                SelectedProblem = selectedProblem
            };

            // when the user clicks cancel, force the datagrid to refresh to show the old values (temporary)
            if (!problemDetailView.ShowDialog().Value)
                RefreshDatagrid();
        }

        // Filters the datagrid based on a textbox and a combobox
        private void FilterDatagrid(object sender, EventArgs e)
        {
            var _itemSourceList = new CollectionViewSource() { Source = Problems };

            // ICollectionView the View/UI part 
            ICollectionView Itemlist = _itemSourceList.View;
            Predicate<object> searchFilter;
            if (cboStatus.SelectedIndex == 0 || cboStatus.SelectedIndex == -1)
            {
                searchFilter = new Predicate<object>(item => ((Problem)item).Description.ToLower().Contains(txtZoek.Text.ToLower()));
                Itemlist.Filter = searchFilter;
            }
            else
            {
                searchFilter = new Predicate<object>(item => ((Problem)item).Description.ToLower().Contains(txtZoek.Text.ToLower()) && ((Problem)item).Status == (string)cboStatus.SelectedItem);
                Itemlist.Filter = searchFilter;
            }

            dgStoringen.ItemsSource = Itemlist;
        }

        private void RegistreerStoringClick(object sender, RoutedEventArgs e)
        {
            ProblemDetailView problemDetailView = new ProblemDetailView(currentEmployee);

            // Force the datagrid to refresh after a device is registered (temporary)
            if (problemDetailView.ShowDialog().Value)
                RefreshDatagrid();
        }

        private void RefreshDatagrid()
        {
            Problems = problemDataService.GetAllProblems().ToObservableCollection();
            dgStoringen.ItemsSource = Problems;
        }
    }
}
