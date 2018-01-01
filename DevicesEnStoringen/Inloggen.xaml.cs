using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DevicesEnStoringen
{
    /// <summary>
    /// Interaction logic for Inloggen.xaml
    /// </summary>
    public partial class Inloggen : Window
    {
        public Inloggen()
        {
            InitializeComponent();
        }

        private void btnInloggen_Click(object sender, RoutedEventArgs e)
        {
            Medewerker medewerker = new Medewerker();
            bool inloggegevensCorrect = medewerker.ControleerInlogGegevens(txtGebruikersnaam.Text, txtWachtwoord.Password);

            if (inloggegevensCorrect)
            {
                AlleStoringen alleStoringen = new AlleStoringen();
                alleStoringen.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Gebruikersnaam of wachtwoord is incorrect");
            }
        }
    }
}
