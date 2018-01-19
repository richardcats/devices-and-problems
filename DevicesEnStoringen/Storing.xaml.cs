using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace DevicesEnStoringen
{
    public partial class Storing : Window
    {
        DatabaseConnection conn = new DatabaseConnection();
        public static ObservableCollection<string> listStatus = FillCombobox(ComboboxType.Status);
        Dictionary<object, object> deviceList = new Dictionary<object, object>();
        int medewerkerID;
        int id;

        // When an existing malfunction is clicked
        public Storing(int id)
        {
            InitializeComponent();

            Title = "Storing bewerken";

            cboStatus.ItemsSource = FillCombobox(ComboboxType.Status);
            cboBehandeldDoor.ItemsSource = FillCombobox(ComboboxType.Medewerker);
            cboErnst.ItemsSource = FillCombobox(ComboboxType.PrioriteitErnst);
            cboPrioriteit.ItemsSource = FillCombobox(ComboboxType.PrioriteitErnst);

            FillTextBoxes(id);
            FillDataGrid();
            cvsRegistreerKnoppen.Visibility = Visibility.Hidden;
            cvsBewerkKnoppen.Visibility = Visibility.Visible;
            datDatumAfhandeling.IsEnabled = true;

            dgBetrokkenDevices.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT Device.DeviceID AS ID, Naam, Serienummer FROM Device INNER JOIN DeviceStoring ON DeviceStoring.DeviceID = Device.DeviceID WHERE DeviceStoring.StoringID ='" + id + "'") });

            dgOpmerkingen.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT Date(Datum) AS Datum, Beschrijving FROM Opmerking WHERE StoringID ='" + id + "'") });

            foreach (DataRowView row in dgBetrokkenDevices.Items)
            {
                deviceList.Add(row["ID"], row);
            }

            dgBetrokkenDevices.ItemsSource = null;
            dgBetrokkenDevices.ItemsSource = deviceList.Values;

            this.id = id;
        }

        // When a new malfunction is registered. It also passes an object of the employee, so this name will be displayed in the database
        public Storing(Employee employee)
        {
            InitializeComponent();
            Title = "Storing registreren";
            txtToegevoegdDoor.Text = employee.FirstNameOfCurrentEmployee();

            cboStatus.ItemsSource = FillCombobox(ComboboxType.Status);
            cboBehandeldDoor.ItemsSource = FillCombobox(ComboboxType.Medewerker);
            cboErnst.ItemsSource = FillCombobox(ComboboxType.PrioriteitErnst);
            cboPrioriteit.ItemsSource = FillCombobox(ComboboxType.PrioriteitErnst);

            FillDataGrid();

            cvsRegistreerKnoppen.Visibility = Visibility.Visible;
            cvsBewerkKnoppen.Visibility = Visibility.Hidden;

            medewerkerID = employee.IDOfCurrentEmployee();
        }

        private void FillTextBoxes(int id)
        {
            conn.OpenConnection();
            SQLiteDataReader dr = conn.DataReader("SELECT Storing.StoringID, Storing.*, Medewerker.* FROM Storing LEFT JOIN Medewerker ON Storing.MedewerkerGeregistreerd = Medewerker.MedewerkerID WHERE StoringID='" + id + "'");
            dr.Read();

            txtBeschrijving.Text = dr["Beschrijving"].ToString();
            txtToegevoegdDoor.Text = dr["Voornaam"].ToString();
            cboPrioriteit.SelectedValue = dr["Prioriteit"].ToString();
            cboErnst.SelectedValue = dr["Ernst"].ToString();
            cboStatus.SelectedValue = dr["Status"].ToString();
            datDatumAfhandeling.Text = dr["DatumAfhandeling"].ToString();
            if (!DBNull.Value.Equals(dr["MedewerkerBehandeld"])) cboBehandeldDoor.SelectedIndex = Convert.ToInt32(dr["MedewerkerBehandeld"]) - 1;

            conn.CloseConnection();
        }

        // Fill the combobox based on the combobox type 
        public static ObservableCollection<string> FillCombobox(ComboboxType type)
        {
            ObservableCollection<string> list = new ObservableCollection<string>();
            DatabaseConnection conn = new DatabaseConnection();
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
            conn.CloseConnection();
            return list;
        }

        private void FillDataGrid()
        {
            dgDevicesToevoegen.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT DeviceID AS ID, Naam, Serienummer FROM Device") });
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // Adds a device to a specific malfunction
        private void AddDevice(object sender, RoutedEventArgs e)
        {
            foreach (DataRowView row in dgDevicesToevoegen.SelectedItems)
            {
                if(!deviceList.ContainsKey(row["ID"])) // don't allow duplicate devices 
                    deviceList.Add(row["ID"],row);
            }

            dgBetrokkenDevices.ItemsSource = null;
            dgBetrokkenDevices.ItemsSource = deviceList.Values;
        }

        // Removes the device from the malfunction
        private void RemoveDevice(object sender, RoutedEventArgs e)
        {
            foreach (DataRowView row in dgBetrokkenDevices.SelectedItems)
                deviceList.Remove(row["ID"]); 

            dgBetrokkenDevices.ItemsSource = null;
            dgBetrokkenDevices.ItemsSource = deviceList.Values;
        }


        // Ensures that the manage device button is placed at the end of the datagrid
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
            dgDevicesToevoegen.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT DeviceID AS ID, Naam, Serienummer FROM Device WHERE Naam LIKE '%" + txtZoek.Text + "%'") });
        }

        // Ensures that all required fields are filled in before inserting the malfunction into the database
        private void AddStoring(object sender, RoutedEventArgs e)
        {
            if (txtBeschrijving.Text != "" && dgBetrokkenDevices.Items.Count > 0)
            {
                conn.OpenConnection();
                conn.ExecuteQueries("INSERT INTO Storing (Beschrijving, MedewerkerGeregistreerd, Prioriteit, Ernst, Status, DatumToegevoegd) VALUES ('" + txtBeschrijving.Text + "','" + medewerkerID + "','" + cboPrioriteit.SelectedValue + "','" + cboErnst.SelectedValue + "','" + cboStatus.SelectedValue + "', date('now'))");

                SQLiteDataReader dr = conn.DataReader("SELECT last_insert_rowid() AS LastID;");
                dr.Read();

                foreach (object deviceID in deviceList.Keys)
                    conn.ExecuteQueries("INSERT INTO DeviceStoring (StoringID, DeviceID) VALUES ('" + Convert.ToInt32(dr["LastID"]) + "','" + Convert.ToInt32(deviceID) + "')");

                conn.CloseConnection();
                Close();
            }
            else
            {
                MarkEmptyFieldsRed();
                MessageBox.Show("Niet alle verplichte velden zijn ingevuld", Title, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Ensures that all required fields are filled in before updating the malfunction in the database
        private void UpdateStoring(object sender, RoutedEventArgs e)
        {
            if (txtBeschrijving.Text != "" && dgBetrokkenDevices.Items.Count > 0)
            {
                conn.OpenConnection();

                if (datDatumAfhandeling.SelectedDate == null)
                    conn.ExecuteQueries("UPDATE Storing SET Beschrijving = '" + txtBeschrijving.Text + "', Prioriteit = '" + cboPrioriteit.SelectedValue + "', Ernst = '" + cboErnst.SelectedValue + "', Status = '" + cboStatus.SelectedValue + "', DatumAfhandeling = NULL, MedewerkerBehandeld = '" + Convert.ToInt32(cboBehandeldDoor.SelectedIndex + 1) + "' WHERE StoringID = '" + id + "'");
                else
                    conn.ExecuteQueries("UPDATE Storing SET Beschrijving = '" + txtBeschrijving.Text + "', Prioriteit = '" + cboPrioriteit.SelectedValue + "', Ernst = '" + cboErnst.SelectedValue + "', Status = '" + cboStatus.SelectedValue + "', DatumAfhandeling = '" + datDatumAfhandeling.SelectedDate.Value.ToString("yyyy-MM-dd") + "', MedewerkerBehandeld = '" + Convert.ToInt32(cboBehandeldDoor.SelectedIndex + 1) + "' WHERE StoringID = '" + id + "'");

                conn.ExecuteQueries("DELETE FROM DeviceStoring WHERE StoringID = '" + id + "'");

                foreach (object deviceID in deviceList.Keys)
                    conn.ExecuteQueries("INSERT INTO DeviceStoring (StoringID, DeviceID) VALUES ('" + id + "','" + Convert.ToInt32(deviceID) + "')");

                
                    

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

        // As soon as a change has occurred in one of the fields, the "submit" button will be enabled again
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

        // The user first receives a message before the malfunction is permanently removed from the database
        private void RemoveStoring(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Storing " + id + " wordt permanent verwijderd", "Storing", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                conn.OpenConnection();
                conn.ExecuteQueries("DELETE FROM DeviceStoring WHERE StoringID = '" + id + "'");
                conn.ExecuteQueries("DELETE FROM Storing WHERE StoringID = '" + id + "'");
                conn.CloseConnection();

                Close();
            }
        }

        // Allows the user to see which required fields must be filled
        private void MarkEmptyFieldsRed()
        {
            tbBeschrijving.Foreground = Brushes.Black;
            tbBetrokkenDevices.Foreground = Brushes.Black;

            if (txtBeschrijving.Text == "")
                tbBeschrijving.Foreground = Brushes.Red;

            if (dgBetrokkenDevices.Items.Count == 0)
                tbBetrokkenDevices.Foreground = Brushes.Red;
        }

        private void RemoveComment(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)dgOpmerkingen.SelectedItems[0];

            conn.OpenConnection();
                conn.ExecuteQueries("DELETE FROM Opmerking WHERE Beschrijving = '" + row["Beschrijving"] + "' AND StoringID = '" + id + "'");
            conn.CloseConnection();

            dgOpmerkingen.ItemsSource = null;
            dgOpmerkingen.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT Date(Datum) AS Datum, Beschrijving FROM Opmerking WHERE StoringID ='" + id + "'") });
        }

        private void AddComment(object sender, RoutedEventArgs e)
        {
            if (txtOpmerking.Text != "")
            {
                conn.OpenConnection();
                conn.ExecuteQueries("INSERT INTO Opmerking (StoringID, Datum, Beschrijving) VALUES ('" + id + "', date('now'), '" + txtOpmerking.Text + "')");
                conn.CloseConnection();
                txtOpmerking.Text = "";
            }

            dgOpmerkingen.ItemsSource = null;
            dgOpmerkingen.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT Date(Datum) AS Datum, Beschrijving FROM Opmerking WHERE StoringID ='" + id + "'") });
        }
    }
}
