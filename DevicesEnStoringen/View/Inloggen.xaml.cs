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
            Employee employee = new Employee(txtGebruikersnaam.Text); // The username of the employee will be saved throughout the application
            bool loginDetailsCorrect = employee.CheckLoginDetails(txtGebruikersnaam.Text, txtWachtwoord.Password); // checks whether the login details are correct

            if (loginDetailsCorrect)
            {
                Overzicht overzicht = new Overzicht(employee);
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
