using DevicesEnStoringen.Services;
using System.Windows;

namespace DevicesEnStoringen
{
    public partial class Overzicht : Window
    {
        private EmployeeDataService currentEmployee;
        private ProblemOverviewView problemOverviewView;
        public Overzicht(EmployeeDataService currentEmployee)
        {
            InitializeComponent();

            problemOverviewView = new ProblemOverviewView(currentEmployee);
            stkOverzicht.Children.Add(problemOverviewView);
            txtIngelogdAls.Text = currentEmployee.FirstNameOfCurrentEmployee();
            this.currentEmployee = currentEmployee;

            if (currentEmployee.AccountTypeOfCurrentEmployee() == "IT-manager")
                btnRapportages.Visibility = Visibility.Visible;
        }

        private void StoringenClick(object sender, RoutedEventArgs e)
        {
            Title = "Alle storingen";
            stkOverzicht.Children.Clear();
            problemOverviewView = new ProblemOverviewView(currentEmployee);
            stkOverzicht.Children.Add(problemOverviewView);
        }

        private void DevicesClick(object sender, RoutedEventArgs e)
        {
            Title = "Alle devices";
            stkOverzicht.Children.Clear();
            DeviceOverviewView deviceOverviewView = new DeviceOverviewView(currentEmployee);
            stkOverzicht.Children.Add(deviceOverviewView);
        }

        private void DeviceTypesClick(object sender, RoutedEventArgs e)
        {
            Title = "Alle device-types";
            stkOverzicht.Children.Clear();
            DeviceTypeOverviewView deviceTypeOverviewView = new DeviceTypeOverviewView(currentEmployee);
            stkOverzicht.Children.Add(deviceTypeOverviewView);
        }

        private void RapportagesClick(object sender, RoutedEventArgs e)
        {
            Title = "Rapportage storingen";
            stkOverzicht.Children.Clear();
            ReportsView reportsView = new ReportsView(currentEmployee);
            stkOverzicht.Children.Add(reportsView);
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            LoginView loginView = new LoginView();
            loginView.Show();
            Close();
        }
    }
}
