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
    public partial class Device : Window
    {
        DatabaseConnectie conn = new DatabaseConnectie();
        public static ObservableCollection<string> listDeviceTypes;
        int id;

        public Device(int id)
        {
            InitializeComponent();

            Title = "Device bewerken";

            FillTextBoxes(id);
            lstDeviceType.ItemsSource = FillComboboxDeviceType();
            lstAfdeling.ItemsSource = FillComboboxAfdeling();
            grdOpenstaandeStoringen.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT Storing.StoringID AS ID, Beschrijving, Date(DatumToegevoegd) AS Datum FROM DeviceStoring LEFT JOIN Storing ON Storing.StoringID = DeviceStoring.StoringID WHERE DeviceID = '" + id + "' AND Status = 'Open'") });

            cvsRegistreerKnoppen.Visibility = Visibility.Hidden;
            cvsBewerkKnoppen.Visibility = Visibility.Visible;

            this.id = id;
        }

        public Device()
        {
            InitializeComponent();
            Title = "Device registreren";
            lstDeviceType.ItemsSource = FillComboboxDeviceType();
            lstAfdeling.ItemsSource = FillComboboxAfdeling();

            cvsRegistreerKnoppen.Visibility = Visibility.Visible;
            cvsBewerkKnoppen.Visibility = Visibility.Hidden;
            cvsOpenstaandeStoringen.Visibility = Visibility.Hidden;
            Height = 270;
        }

        private void FillTextBoxes(int id)
        {
            conn.OpenConnection();
            SQLiteDataReader dr = conn.DataReader("SELECT DeviceType.Naam AS DeviceTypeNaam, Device.* FROM Device INNER JOIN DeviceType ON DeviceType.DeviceTypeID = Device.DeviceTypeID WHERE DeviceID='" + id + "'");
            dr.Read();

            txtNaam.Text = dr["Naam"].ToString();
            lstDeviceType.SelectedValue = dr["DeviceTypeNaam"].ToString();
            lstAfdeling.SelectedValue = dr["Afdeling"].ToString();
            txtSerienummer.Text = dr["Serienummer"].ToString();
            txtOpmerkingen.Text = dr["Opmerkingen"].ToString();
        }

        public static ObservableCollection<string> FillComboboxDeviceType()
        {
            listDeviceTypes = new ObservableCollection<string>();
            DatabaseConnectie conn = new DatabaseConnectie();
            conn.OpenConnection();
            SQLiteDataReader dr = conn.DataReader("SELECT Naam FROM DeviceType");

            while (dr.Read())
                listDeviceTypes.Add(dr["Naam"].ToString());

            return listDeviceTypes;
        }

        public static ObservableCollection<string> FillComboboxAfdeling()
        {
            ObservableCollection<string> listAfdelingen = new ObservableCollection<string>();
            DatabaseConnectie conn = new DatabaseConnectie();
            conn.OpenConnection();
            SQLiteDataReader dr = conn.DataReader("SELECT Afdeling FROM Device GROUP BY Afdeling");

            while (dr.Read())
                listAfdelingen.Add(dr["Afdeling"].ToString());

            return listAfdelingen;
        }

        private void FillDataGrid()
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

        private void RowButtonClick(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)grdOpenstaandeStoringen.SelectedItems[0];
            Storing storing = new Storing(Convert.ToInt32(row["ID"]));
            storing.Show();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void AddDevice(object sender, RoutedEventArgs e)
        {
            conn.OpenConnection();
            conn.ExecuteQueries("INSERT INTO Device (DeviceTypeID, Naam, Serienummer, Afdeling, Opmerkingen, DatumToegevoegd) VALUES ( '" + Convert.ToInt32(lstDeviceType.SelectedIndex + 1) + "','" + txtNaam.Text + "','" + txtSerienummer.Text + "','" + lstAfdeling.SelectedValue + "','" + txtOpmerkingen.Text + "', date('now'))");
            Close();
        }

        private void UpdateDevice(object sender, RoutedEventArgs e)
        {
            if (grdOpenstaandeStoringen.Items.Count == 0)
            {
                conn.OpenConnection();
                conn.ExecuteQueries("UPDATE Device SET DeviceTypeID = '" + Convert.ToInt32(lstDeviceType.SelectedIndex + 1) + "', Naam = '" + txtNaam.Text + "', Serienummer = '" + txtSerienummer.Text + "', Afdeling = '" + lstAfdeling.SelectedValue + "', Opmerkingen = '" + txtOpmerkingen.Text + "' WHERE DeviceID = '" + id + "'");
                btnToepassen.IsEnabled = false;

                Button button = (Button)sender;

                if (button.Name == "btnOK")
                    Close();
            }
            else
            {
                MessageBox.Show("Het is niet mogelijk om dit device aan te passen. Zorg dat er geen storingen gekoppeld zijn aan dit device.", "Device-type", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EnableToepassen(object sender, TextChangedEventArgs e)
        {
            if (btnToepassen.IsEnabled == false)
                btnToepassen.IsEnabled = true;
        }

        private void EnableToepassen(object sender, SelectionChangedEventArgs e)
        {
            if (btnToepassen.IsEnabled == false)
                btnToepassen.IsEnabled = true;
        }

        private void RemoveDevice(object sender, RoutedEventArgs e)
        {
            if (grdOpenstaandeStoringen.Items.Count == 0)
            {
                if (MessageBox.Show("Device " + id + " wordt permanent verwijderd", "Device", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    conn.OpenConnection();
                    conn.ExecuteQueries("DELETE FROM Device WHERE DeviceID = '" + id + "'");
                    Close();
                }
            }
            else
            {
                MessageBox.Show("Het is niet mogelijk om dit device te verwijderen. Zorg dat er geen storingen gekoppeld zijn aan dit device.", "Device-type", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
