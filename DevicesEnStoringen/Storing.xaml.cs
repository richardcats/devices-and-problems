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
        public static ObservableCollection<string> listStatus = FillCombobox(ComboboxType.Status);
        Dictionary<object, object> deviceList = new Dictionary<object, object>();
        int medewerkerID;
        int id;

        public Storing(int id)
        {
            InitializeComponent();

            Title = "Storing bewerken";

            lstStatus.ItemsSource = FillCombobox(ComboboxType.Status);
            lstBehandeldDoor.ItemsSource = FillCombobox(ComboboxType.Medewerker);
            lstErnst.ItemsSource = FillCombobox(ComboboxType.PrioriteitErnst);
            lstPrioriteit.ItemsSource = FillCombobox(ComboboxType.PrioriteitErnst);

            FillTextBoxes(id);
            FillDataGrid();
            cvsRegistreerKnoppen.Visibility = Visibility.Hidden;
            cvsBewerkKnoppen.Visibility = Visibility.Visible;
            txtDatumAfhandeling.IsEnabled = true;

            grdBetrokkenDevices.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT Device.DeviceID AS ID, Naam, Serienummer FROM Device INNER JOIN DeviceStoring ON DeviceStoring.DeviceID = Device.DeviceID WHERE DeviceStoring.StoringID ='" + id + "'") });

            foreach (DataRowView row in grdBetrokkenDevices.Items)
            {
                deviceList.Add(row["ID"], row);
            }

            grdBetrokkenDevices.ItemsSource = null;
            grdBetrokkenDevices.ItemsSource = deviceList.Values;
            this.id = id;
        }

        public Storing(Medewerker medewerker)
        {
            InitializeComponent();
            Title = "Storing registreren";
            txtToegevoegdDoor.Text = medewerker.naamHuidigeMedewerkerIngelogd();

            lstStatus.ItemsSource = FillCombobox(ComboboxType.Status);
            lstBehandeldDoor.ItemsSource = FillCombobox(ComboboxType.Medewerker);
            lstErnst.ItemsSource = FillCombobox(ComboboxType.PrioriteitErnst);
            lstPrioriteit.ItemsSource = FillCombobox(ComboboxType.PrioriteitErnst);

            FillDataGrid();

            cvsRegistreerKnoppen.Visibility = Visibility.Visible;
            cvsBewerkKnoppen.Visibility = Visibility.Hidden;

            medewerkerID = medewerker.idHuidigeMedewerkerIngelogd();
        }

        private void FillTextBoxes(int id)
        {
            conn.OpenConnection();
            SQLiteDataReader dr = conn.DataReader("SELECT Storing.StoringID, Storing.*, Medewerker.* FROM Storing LEFT JOIN Medewerker ON Storing.MedewerkerGeregistreerd = Medewerker.MedewerkerID WHERE StoringID='" + id + "'");
            dr.Read();

            txtBeschrijving.Text = dr["Beschrijving"].ToString();
            txtToegevoegdDoor.Text = dr["Voornaam"].ToString();
            lstPrioriteit.SelectedValue = dr["Prioriteit"].ToString();
            lstErnst.SelectedValue = dr["Ernst"].ToString();
            lstStatus.SelectedValue = dr["Status"].ToString();
            txtDatumAfhandeling.Text = dr["DatumAfhandeling"].ToString();
            if (!DBNull.Value.Equals(dr["MedewerkerBehandeld"])) lstBehandeldDoor.SelectedIndex = Convert.ToInt32(dr["MedewerkerBehandeld"]) - 1;

            conn.CloseConnection();
        }

        public static ObservableCollection<string> FillCombobox(ComboboxType type)
        {
            ObservableCollection<string> list = new ObservableCollection<string>();
            DatabaseConnectie conn = new DatabaseConnectie();
            conn.OpenConnection();

            if (type == ComboboxType.Status)
            {
                list = new ObservableCollection<string>();
                list.Add("Open");
                list.Add("In behandeling");
                list.Add("Afgehandeld");
            }
            else if (type == ComboboxType.StatusAll)
            {
                list = new ObservableCollection<string>();
                list.Add("Alle storingen"); 
                list.Add("Open");
                list.Add("In behandeling");
                list.Add("Afgehandeld");
            }

            else if (type == ComboboxType.Medewerker)
            {
                SQLiteDataReader dr = conn.DataReader("SELECT Voornaam FROM Medewerker");

                while (dr.Read())
                    list.Add(dr["Voornaam"].ToString());
            }

            else if (type == ComboboxType.PrioriteitErnst)
            {
                list = new ObservableCollection<string>();
                list.Add("0");
                list.Add("1");
                list.Add("2");
                list.Add("3");
            }
            return list;
        }

        private void FillDataGrid()
        {
            grdDevicesToevoegen.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT DeviceID AS ID, Naam, Serienummer FROM Device") });
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddDevice(object sender, RoutedEventArgs e)
        {
            foreach (DataRowView row in grdDevicesToevoegen.SelectedItems)
            {
                if(!deviceList.ContainsKey(row["ID"]))  //(!destList.Any((prod => prod.GetHashCode() == row.GetHashCode())))
                    deviceList.Add(row["ID"],row);
            }

            //DataRowView row = (DataRowView)grdDevicesToevoegen.SelectedItems[0];
            //MessageBox.Show(row["ID"].ToString());

            grdBetrokkenDevices.ItemsSource = null;
            grdBetrokkenDevices.ItemsSource = deviceList.Values;
        }

        private void RemoveDevice(object sender, RoutedEventArgs e)
        {
            foreach (DataRowView row in grdBetrokkenDevices.SelectedItems)
                deviceList.Remove(row["ID"]); 


            grdBetrokkenDevices.ItemsSource = null;
            grdBetrokkenDevices.ItemsSource = deviceList.Values;
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

        private void FilterDatagrid(object sender, TextChangedEventArgs e)
        {
            grdDevicesToevoegen.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT DeviceID AS ID, Naam, Serienummer FROM Device WHERE Naam LIKE '%" + txtZoek.Text + "%'") });
        }

        private void AddStoring(object sender, RoutedEventArgs e)
        {
            conn.OpenConnection();
            conn.ExecuteQueries("INSERT INTO Storing (Beschrijving, MedewerkerGeregistreerd, Prioriteit, Ernst, Status, DatumToegevoegd) VALUES ('" + txtBeschrijving.Text + "','" + medewerkerID +  "','" + lstPrioriteit.SelectedValue + "','" + lstErnst.SelectedValue + "','" + lstStatus.SelectedValue + "', date('now'))");

            SQLiteDataReader dr = conn.DataReader("SELECT last_insert_rowid() AS LastID;");
            dr.Read();

            foreach (object deviceID in deviceList.Keys)
               conn.ExecuteQueries("INSERT INTO DeviceStoring (StoringID, DeviceID) VALUES ('" + Convert.ToInt32(dr["LastID"]) + "','" + Convert.ToInt32(deviceID) + "')");

            conn.CloseConnection();
            Close();
        }

        private void UpdateStoring(object sender, RoutedEventArgs e)
        {
            conn.OpenConnection();

            if (txtDatumAfhandeling.SelectedDate == null)
                conn.ExecuteQueries("UPDATE Storing SET Beschrijving = '" + txtBeschrijving.Text + "', Prioriteit = '" + lstPrioriteit.SelectedValue + "', Ernst = '" + lstErnst.SelectedValue + "', Status = '" + lstStatus.SelectedValue + "', DatumAfhandeling = NULL, MedewerkerBehandeld = '" + Convert.ToInt32(lstBehandeldDoor.SelectedIndex + 1) + "' WHERE StoringID = '" + id + "'");
            else
                conn.ExecuteQueries("UPDATE Storing SET Beschrijving = '" + txtBeschrijving.Text + "', Prioriteit = '" + lstPrioriteit.SelectedValue + "', Ernst = '" + lstErnst.SelectedValue + "', Status = '" + lstStatus.SelectedValue + "', DatumAfhandeling = '" + txtDatumAfhandeling.SelectedDate.Value.ToString("yyyy-MM-dd") + "', MedewerkerBehandeld = '" + Convert.ToInt32(lstBehandeldDoor.SelectedIndex + 1) + "' WHERE StoringID = '" + id + "'");

            conn.ExecuteQueries("DELETE FROM DeviceStoring WHERE StoringID = '" + id + "'");

            foreach (object deviceID in deviceList.Keys)
                conn.ExecuteQueries("INSERT INTO DeviceStoring (StoringID, DeviceID) VALUES ('" + id + "','" + Convert.ToInt32(deviceID) + "')");

            btnToepassen.IsEnabled = false;

            Button button = (Button)sender;

            if (button.Name == "btnOK")
                Close();
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

        private void RemoveStoring(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Storing " + id + " wordt permanent verwijderd", "Storing", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                conn.OpenConnection();
                conn.ExecuteQueries("DELETE FROM DeviceStoring WHERE StoringID = '" + id + "'");
                conn.ExecuteQueries("DELETE FROM Storing WHERE StoringID = '" + id + "'");

                Close();
            }
        }
    }
}
