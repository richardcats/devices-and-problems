using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for Storing.xaml
    /// </summary>
    public partial class Storing : Window
    {
        DatabaseConnectie conn = new DatabaseConnectie();
        public ObservableCollection<string> list = new ObservableCollection<string>();


        public Storing(int id)
        {
            InitializeComponent();

            Title = "Storing bewerken";

            FillTextBoxes(id);
            FillCombobox();
        }

        public Storing()
        {
            InitializeComponent();
            Title = "Storing registreren";
            FillCombobox();
        }

        private void FillTextBoxes(int id)
        {
            conn.OpenConnection();
            SQLiteDataReader dr = conn.DataReader("SELECT * FROM Storing WHERE StoringID='" + id + "'");
            dr.Read();

            txtBeschrijving.Text = dr["Beschrijving"].ToString();
            txtPrioriteit.Text = dr["Prioriteit"].ToString();
            txtErnst.Text = dr["Ernst"].ToString();
            lstStatus.SelectedValue = dr["Status"].ToString();
            txtDatumAfhandeling.Text = dr["DatumAfhandeling"].ToString();
            txtBehandeldDoor.Text = dr["MedewerkerBehandeld"].ToString();
        }

        private void FillCombobox()
        {
            list.Add("Alle storingen");
            list.Add("Open");
            list.Add("In behandeling");
            list.Add("Afgehandeld");
            lstStatus.ItemsSource = list;
        }

        private void AddStoring(object sender, RoutedEventArgs e)
        {

        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
