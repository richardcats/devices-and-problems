using System.Windows;

namespace DevicesEnStoringen
{
    public partial class Overzicht : Window
    {
        Employee currentEmployee;
        ProblemOverviewView problemOverviewView;
        public Overzicht(Employee currentEmployee)
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
            problemOverviewView.ClearDatabaseConnection();
            DeviceOverviewView deviceOverviewView = new DeviceOverviewView(currentEmployee);
            stkOverzicht.Children.Add(deviceOverviewView);
        }

        private void DeviceTypesClick(object sender, RoutedEventArgs e)
        {
            Title = "Alle device-types";
            stkOverzicht.Children.Clear();
            problemOverviewView.ClearDatabaseConnection();
            DeviceTypeOverviewView deviceTypeOverviewView = new DeviceTypeOverviewView(currentEmployee);
            stkOverzicht.Children.Add(deviceTypeOverviewView);
        }

        private void RapportagesClick(object sender, RoutedEventArgs e)
        {
            Title = "Rapportage storingen";
            stkOverzicht.Children.Clear();
            UCRapportages rapportages = new UCRapportages(currentEmployee);
            stkOverzicht.Children.Add(rapportages);
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            Inloggen inloggen = new Inloggen();
            inloggen.Show();
            Close();
        }
    }
}
