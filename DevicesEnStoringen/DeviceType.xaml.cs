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
        public static ObservableCollection<string> listDeviceTypes;

        public DeviceType(int id)
        {
            InitializeComponent();

            Title = "Device-type bewerken";

            FillTextBoxes(id);
            grdDevices.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT DeviceID AS ID, Naam, Afdeling, Date(DatumToegevoegd) AS Datum FROM Device WHERE DeviceTypeID = '" + id + "'") });

            cvsRegistreerKnoppen.Visibility = Visibility.Hidden;
            cvsBewerkKnoppen.Visibility = Visibility.Visible;
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
            conn.OpenConnection();
            conn.ExecuteQueries("INSERT INTO DeviceType (Naam, Opmerkingen) VALUES ( '" + txtNaam.Text + "','" + txtOpmerkingen.Text +"')");
            //devicesGrid.Items.Refresh();
            //alleDeviceTypes.grdDevices.ItemsSource = null;
           
            ((Overzicht)Owner).l
            Close();
        }
    }
}
