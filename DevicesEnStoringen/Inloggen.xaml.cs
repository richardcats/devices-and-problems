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
            DatabaseConnectie conn = new DatabaseConnectie();
            //conn.OpenConection();

            SQLiteConnection con = new SQLiteConnection(@"Data Source=C:\Users\Richard\Documents\Visual Studio 2017\Projects\devices-en-storingen\DevicesEnStoringen\Data\DevicesEnStoringen.sqlite;Version=3");
            try
            {
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();
                string query = "SELECT COUNT(1) FROM Medewerker WHERE Emailadres=@Emailadres AND Wachtwoord=@Wachtwoord";
                SQLiteCommand sqlCmd = new SQLiteCommand(query, con);
                sqlCmd.CommandType = System.Data.CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@Emailadres", txtGebruikersnaam.Text);
                sqlCmd.Parameters.AddWithValue("@Wachtwoord", txtWachtwoord.Password);
                int count = Convert.ToInt32(sqlCmd.ExecuteScalar());

                if (count == 1)
                {
                    MainWindow mainwindow = new MainWindow();
                    mainwindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Gebruikersnaam of wachtwoord is incorrect");
                }
            }
            catch(Exception)
            {

            }
            finally
            {

            }
        }
    }
}
