using DevicesEnStoringen.Extensions;
using DevicesEnStoringen.Services;
using Model;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
namespace DevicesEnStoringen
{
    public partial class ProblemOverviewView : UserControl
    {
        DatabaseConnection conn = new DatabaseConnection();

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
                btnRegistreerStoring.Visibility = Visibility.Hidden;              
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

            if (problemDetailView.ShowDialog().Value)
            {
                dgStoringen.ItemsSource = null;
                conn.OpenConnection();
                dgStoringen.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT StoringID AS ID, Beschrijving, Date(DatumToegevoegd) AS Datum, Prioriteit, Ernst, Status FROM Storing") });
                conn.CloseConnection();
            }
        }

        // Filters the datagrid based on a textbox and a combobox
        private void FilterDatagrid(object sender, EventArgs e)
        {
            if (cboStatus.SelectedIndex == 0 || cboStatus.SelectedIndex == -1)
                dgStoringen.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT StoringID AS ID, Beschrijving, Date(DatumToegevoegd) AS Datum, Prioriteit, Ernst, Status FROM Storing WHERE Beschrijving LIKE '%" + txtZoek.Text + "%'") });
            else
                dgStoringen.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT StoringID AS ID, Beschrijving, Date(DatumToegevoegd) AS Datum, Prioriteit, Ernst, Status FROM Storing WHERE Beschrijving LIKE '%" + txtZoek.Text + "%' AND Status='" + cboStatus.SelectedItem + "'") });
        }

        private void RegistreerStoringClick(object sender, RoutedEventArgs e)
        {
            ProblemDetailView problemDetailView = new ProblemDetailView(currentEmployee);

            if (problemDetailView.ShowDialog().Value)
            {
                dgStoringen.ItemsSource = null;
                dgStoringen.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT StoringID AS ID, Beschrijving, Date(DatumToegevoegd) AS Datum, Prioriteit, Ernst, Status FROM Storing") });
            }
        }

        public void ClearDatabaseConnection()
        {
            dgStoringen.ItemsSource = null;
        }
    }
}
