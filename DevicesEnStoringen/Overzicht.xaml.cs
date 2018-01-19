using System.Windows;

namespace DevicesEnStoringen
{
    public partial class Overzicht : Window
    {
        Employee employee;
        public Overzicht(Employee employee)
        {
            InitializeComponent();

            UCAlleStoringen alleStoringen = new UCAlleStoringen(employee);
            stkOverzicht.Children.Add(alleStoringen);
            txtIngelogdAls.Text = employee.FirstNameOfCurrentEmployee();
            this.employee = employee;

            if (employee.AccountTypeOfCurrentEmployee() == "IT-manager")
                btnRapportages.Visibility = Visibility.Visible;
        }

        private void StoringenClick(object sender, RoutedEventArgs e)
        {
            Title = "Alle storingen";
            stkOverzicht.Children.Clear();
            UCAlleStoringen alleStoringen = new UCAlleStoringen(employee);
            stkOverzicht.Children.Add(alleStoringen);
        }

        private void DevicesClick(object sender, RoutedEventArgs e)
        {
            Title = "Alle devices";
            stkOverzicht.Children.Clear();
            UCAlleDevices alleDevices = new UCAlleDevices(employee);
            stkOverzicht.Children.Add(alleDevices);
        }

        private void DeviceTypesClick(object sender, RoutedEventArgs e)
        {
            Title = "Alle device-types";
            stkOverzicht.Children.Clear();
            UCAlleDeviceTypes alleDeviceTypes = new UCAlleDeviceTypes(employee);
            stkOverzicht.Children.Add(alleDeviceTypes);
        }

        private void RapportagesClick(object sender, RoutedEventArgs e)
        {
            Title = "Rapportage storingen";
            stkOverzicht.Children.Clear();
            UCRapportages rapportages = new UCRapportages(employee);
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
