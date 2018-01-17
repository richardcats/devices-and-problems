using System.Windows;

namespace DevicesEnStoringen
{
    /// <summary>
    /// Interaction logic for Overzicht.xaml
    /// </summary>
    public partial class Overzicht : Window
    {
        Medewerker medewerker;
        public Overzicht(Medewerker medewerker)
        {
            InitializeComponent();

            UCAlleStoringen alleStoringen = new UCAlleStoringen(medewerker);
            stkOverzicht.Children.Add(alleStoringen);
            txtIngelogdAls.Text = medewerker.NaamHuidigeMedewerkerIngelogd();
            this.medewerker = medewerker;

            if (medewerker.AccountTypeHuidigeMedewerkerIngelogd() == "IT-manager")
                btnRapportages.Visibility = Visibility.Visible;
        }

        private void StoringenClick(object sender, RoutedEventArgs e)
        {
            Title = "Alle storingen";
            stkOverzicht.Children.Clear();
            UCAlleStoringen alleStoringen = new UCAlleStoringen(medewerker);
            stkOverzicht.Children.Add(alleStoringen);
        }

        private void DevicesClick(object sender, RoutedEventArgs e)
        {
            Title = "Alle devices";
            stkOverzicht.Children.Clear();
            UCAlleDevices alleDevices = new UCAlleDevices(medewerker);
            stkOverzicht.Children.Add(alleDevices);
        }

        private void DeviceTypesClick(object sender, RoutedEventArgs e)
        {
            Title = "Alle device-types";
            stkOverzicht.Children.Clear();
            UCAlleDeviceTypes alleDeviceTypes = new UCAlleDeviceTypes(medewerker);
            stkOverzicht.Children.Add(alleDeviceTypes);
        }

        private void RapportagesClick(object sender, RoutedEventArgs e)
        {
            Title = "Rapportage storingen";
            stkOverzicht.Children.Clear();
            UCRapportages rapportages = new UCRapportages(medewerker);
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
