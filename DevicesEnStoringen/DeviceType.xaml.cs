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
    public partial class DeviceType : Window
    {
        DatabaseConnectie conn = new DatabaseConnectie();
        int id;
        Medewerker medewerker;

        public DeviceType(int id, Medewerker medewerker)
        {
            InitializeComponent();

            Title = "Device-type bewerken";

            FillTextBoxes(id);

            grdDevices.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT DeviceID AS ID, Naam, Afdeling, Date(DatumToegevoegd) AS Datum FROM Device WHERE DeviceTypeID = '" + id + "'") });

            cvsRegistreerKnoppen.Visibility = Visibility.Hidden;
            cvsBewerkKnoppen.Visibility = Visibility.Visible;

            this.id = id;
            this.medewerker = medewerker;
        }

        public DeviceType()
        {
            InitializeComponent();
            Title = "Device-type registreren";

            cvsRegistreerKnoppen.Visibility = Visibility.Visible;
            cvsBewerkKnoppen.Visibility = Visibility.Hidden;
            cvsOpenstaandeStoringen.Visibility = Visibility.Hidden;
            Height = 180;
        }

        private void FillTextBoxes(int id)
        {
            conn.OpenConnection();
            SQLiteDataReader dr = conn.DataReader("SELECT * FROM DeviceType WHERE DeviceTypeID='" + id + "'");
            dr.Read();

            txtNaam.Text = dr["Naam"].ToString();
            txtOpmerkingen.Text = dr["Opmerkingen"].ToString();
            conn.CloseConnection();
        }


        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void ChangeGridButtonPositionToEnd(object sender, EventArgs e)
        {
            var dgrd = sender as DataGrid;
            {
                var c = dgrd.Columns[0];
                dgrd.Columns.RemoveAt(0);

                if (medewerker.accountTypeHuidigeMedewerkerIngelogd() == "IT-beheerder")
                    dgrd.Columns.Add(c);
            }
        }

        private void RowButtonClick(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)grdDevices.SelectedItems[0];
            Device device = new Device(Convert.ToInt32(row["ID"]));
            device.Show();
        }

        private void AddDeviceType(object sender, RoutedEventArgs e)
        {
            if (txtNaam.Text != "")
            {
                conn.OpenConnection();
                conn.ExecuteQueries("INSERT INTO DeviceType (Naam, Opmerkingen) VALUES ( '" + txtNaam.Text + "','" + txtOpmerkingen.Text + "')");
                conn.CloseConnection();
                Close();
            }
            else
            {
                MarkEmptyFieldsRed();
                MessageBox.Show("Niet alle verplichte velden zijn ingevuld", Title, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void UpdateDeviceType(object sender, RoutedEventArgs e)
        {
            if (txtNaam.Text != "")
            {
                conn.OpenConnection();
                conn.ExecuteQueries("UPDATE DeviceType SET Naam = '" + txtNaam.Text + "', Opmerkingen = '" + txtOpmerkingen.Text + "' WHERE DeviceTypeID = '" + id + "'");
                conn.CloseConnection();
                btnToepassen.IsEnabled = false;

                Button button = (Button)sender;

                if (button.Name == "btnOK")
                    Close();
            }
            else
            {
                MarkEmptyFieldsRed();
                MessageBox.Show("Niet alle verplichte velden zijn ingevuld", Title, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        

        private void EnableToepassen(object sender, TextChangedEventArgs e)
        {
            if (btnToepassen.IsEnabled == false)
                btnToepassen.IsEnabled = true;
        }

        private void RemoveDeviceType(object sender, RoutedEventArgs e)
        {
            if (grdDevices.Items.Count == 0)
            {
                if (MessageBox.Show("Device-type " + id + " wordt permanent verwijderd", "Device-type", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    conn.OpenConnection();
                    conn.ExecuteQueries("DELETE FROM DeviceType WHERE DeviceTypeID = '" + id + "'");
                    conn.CloseConnection();
                    Close();
                }
            }
            else
            {
                MessageBox.Show("Het is niet mogelijk om dit device-type te verwijderen. Zorg dat er geen devices gekoppeld zijn aan dit device-type.", "Device-type", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void MarkEmptyFieldsRed()
        {
            tbNaam.Foreground = Brushes.Black;

            if (txtNaam.Text == "")
                tbNaam.Foreground = Brushes.Red;
        }
    }
}
