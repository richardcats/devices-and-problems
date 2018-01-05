using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
        public static ObservableCollection<string> list;
        List<object> destList = new List<object>();

        public Storing(int id)
        {
            InitializeComponent();

            Title = "Storing bewerken";

            FillTextBoxes(id);
            lstStatus.ItemsSource = FillCombobox();
            FillDataGrid();
            cvsRegistreerKnoppen.Visibility = Visibility.Hidden;
            cvsBewerkKnoppen.Visibility = Visibility.Visible;

            grdBetrokkenDevices.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT Device.DeviceID AS ID, Naam, Serienummer FROM Device INNER JOIN DeviceStoring ON DeviceStoring.DeviceID = Device.DeviceID WHERE DeviceStoring.StoringID ='" + id + "'") });

            foreach (DataRowView row in grdBetrokkenDevices.Items)
            {
                destList.Add(row);
            }

            grdBetrokkenDevices.ItemsSource = null;
            grdBetrokkenDevices.ItemsSource = destList;
        }

        public Storing()
        {
            InitializeComponent();
            Title = "Storing registreren";
            lstStatus.ItemsSource = FillCombobox();
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

        public static ObservableCollection<string> FillCombobox()
        {
            list = new ObservableCollection<string>();
            list.Add("Open");
            list.Add("In behandeling");
            list.Add("Afgehandeld");
            
            return list;
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
            /*foreach (DataRowView row in grdDevicesToevoegen.SelectedItems)
            {
                if (!destList.Contains(row["ID"]))
                    destList.Add(row);
            }*/

            DataRowView row = (DataRowView)grdDevicesToevoegen.SelectedItems[0];
            MessageBox.Show(row["ID"].ToString());
            if (!destList.Contains(row["ID"]))
                destList.Add(row);


            grdBetrokkenDevices.ItemsSource = null;
            grdBetrokkenDevices.ItemsSource = destList;
        }

        private void RemoveDevice(object sender, RoutedEventArgs e)
        {
            foreach (DataRowView row in grdBetrokkenDevices.SelectedItems)
            {
                destList.Remove(row);
            }

            grdBetrokkenDevices.ItemsSource = null;
            grdBetrokkenDevices.ItemsSource = destList;
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

        private void FilterDatagrid(object sender, TextChangedEventArgs e)
        {
            grdDevicesToevoegen.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT DeviceID AS ID, Naam, Serienummer FROM Device WHERE Naam LIKE '%" + txtZoek.Text + "%'") });
        }
    }
}
