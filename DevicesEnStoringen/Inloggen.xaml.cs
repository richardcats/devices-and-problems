using System.Windows;

namespace DevicesEnStoringen
{
    public partial class Inloggen : Window
    {
        public Inloggen()
        {
            InitializeComponent();
        }

        private void btnInloggen_Click(object sender, RoutedEventArgs e)
        {
            Medewerker medewerker = new Medewerker(txtGebruikersnaam.Text); // The username of the employee will be saved throughout the application
            bool inloggegevensCorrect = medewerker.ControleerInlogGegevens(txtGebruikersnaam.Text, txtWachtwoord.Password); // checks whether the login details are correct

            if (inloggegevensCorrect)
            {
                Overzicht overzicht = new Overzicht(medewerker);
                overzicht.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Gebruikersnaam of wachtwoord is incorrect");
            }
        }
    }
}
