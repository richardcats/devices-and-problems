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
            FillDataGrid();

            cvsRegistreerKnoppen.Visibility = Visibility.Hidden;
            cvsBewerkKnoppen.Visibility = Visibility.Visible;
        }

        public Storing()
        {
            InitializeComponent();
            Title = "Storing registreren";
            FillCombobox();
            FillDataGrid();

            cvsRegistreerKnoppen.Visibility = Visibility.Visible;
            cvsBewerkKnoppen.Visibility = Visibility.Hidden;
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

        private void FillDataGrid()
        {
            grdDevicesToevoegen.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT DeviceID AS ID, Naam, Serienummer FROM Device") });
        }

        private void AddStoring(object sender, RoutedEventArgs e)
        {

        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddDevice(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveDevice(object sender, RoutedEventArgs e)
        {

        }

        private void ChangeGridButtonPositionToEnd(object sender, EventArgs e)
        {
            var dgrd = sender as DataGrid;
            {
                var c = dgrd.Columns[0];
                dgrd.Columns.RemoveAt(0);
                dgrd.Columns.Add(c);
            }
        }

        private void ChangeGridButtonPositionToEnd(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

        }
    }
}
