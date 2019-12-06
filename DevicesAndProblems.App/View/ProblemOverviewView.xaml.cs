using DevicesAndProblems.App.Extensions;
using DevicesAndProblems.App.Services;
using DevicesAndProblems.DAL.SQLite;
using DevicesAndProblems.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DevicesAndProblems.App.View
{
    public partial class ProblemOverviewView : UserControl
    {
        //private ProblemDataService problemDataService;// = new ProblemDataService();

        public ProblemOverviewView()
        {
            InitializeComponent();
        }

        // As soon as a change has occurred in the search field, force the DataGrid to update
        private void SearchInputChanged(object sender, EventArgs e)
        {
            var binding = ((TextBox)sender).GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();
        }

        private void ProblemStatusChanged(object sender, EventArgs e)
        {
            var binding = ((ComboBox)sender).GetBindingExpression(ComboBox.SelectedValueProperty);
            binding.UpdateSource();
        }

        // When the IT administrator clicks on a malfunction, it will pass the ID to a new window
        private void RowButtonClick(object sender, RoutedEventArgs e)
        {
            //TO DO ruim code in commentaar op

            /*Problem selectedProblem = (Problem)dgStoringen.SelectedItems[0];
            ProblemDetailView problemDetailView = new ProblemDetailView(selectedProblem)
            {
                SelectedProblem = selectedProblem
            };

            // when the user clicks cancel, force the datagrid to refresh to show the old values (temporary)
            if (!problemDetailView.ShowDialog().Value)
                RefreshDatagrid();*/
        }



        private void RegistreerStoringClick(object sender, RoutedEventArgs e)
        {
            //ProblemDetailView problemDetailView = new ProblemDetailView(currentEmployee);

            // Force the datagrid to refresh after a device is registered (temporary)
            //if (problemDetailView.ShowDialog().Value)
            //    RefreshDatagrid();
        }

        private void RefreshDatagrid()
        {
            //Problems = problemDataService.GetAllProblems().ToObservableCollection();
            //dgStoringen.ItemsSource = Problems;
        }
    }
}
