using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace DevicesEnStoringen
{
    public partial class Device : Window
    {
        DatabaseConnection conn = new DatabaseConnection();
        public static ObservableCollection<string> listDeviceTypes = FillCombobox(ComboboxType.DeviceType);
        int id;

        // When an existing device is clicked
        public Device(int id)
        {
            InitializeComponent();

            Title = "Device bewerken";

            FillTextBoxes(id);
            cboDeviceType.ItemsSource = FillCombobox(ComboboxType.DeviceType);
            cboAfdeling.ItemsSource = FillCombobox(ComboboxType.Afdeling);
            dgOpenstaandeStoringen.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT Storing.StoringID AS ID, Beschrijving, Date(DatumToegevoegd) AS Datum FROM DeviceStoring LEFT JOIN Storing ON Storing.StoringID = DeviceStoring.StoringID WHERE DeviceID = '" + id + "' AND Status = 'Open'") });

            cvsRegistreerKnoppen.Visibility = Visibility.Hidden;
            cvsBewerkKnoppen.Visibility = Visibility.Visible;

            this.id = id;
        }

        // When a new device is registered
        public Device()
        {
            InitializeComponent();
            Title = "Device registreren";
            cboDeviceType.ItemsSource = FillCombobox(ComboboxType.DeviceType);
            cboAfdeling.ItemsSource = FillCombobox(ComboboxType.Afdeling);

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
            cboDeviceType.SelectedValue = dr["DeviceTypeNaam"].ToString();
            cboAfdeling.SelectedValue = dr["Afdeling"].ToString();
            txtSerienummer.Text = dr["Serienummer"].ToString();
            txtOpmerkingen.Text = dr["Opmerkingen"].ToString();
            conn.CloseConnection();
        }

        // Fill the combobox based on the combobox type 
        public static ObservableCollection<string> FillCombobox(ComboboxType type)
        {
            ObservableCollection<string> list = new ObservableCollection<string>();
            DatabaseConnection conn = new DatabaseConnection();
            conn.OpenConnection();

            if (type == ComboboxType.Afdeling)
            {
                SQLiteDataReader dr = conn.DataReader("SELECT Afdeling FROM Device GROUP BY Afdeling");

                while (dr.Read())
                    list.Add(dr["Afdeling"].ToString());
            }
            else if (type == ComboboxType.DeviceType)
            {
                SQLiteDataReader dr = conn.DataReader("SELECT Naam FROM DeviceType");

                while (dr.Read())
                    list.Add(dr["Naam"].ToString());
            }
            else if (type == ComboboxType.DeviceTypeAll)
            {
                SQLiteDataReader dr = conn.DataReader("SELECT Naam FROM DeviceType");
                list.Add("Alle device-types");
                while (dr.Read())
                    list.Add(dr["Naam"].ToString());
            }
            conn.CloseConnection();
            return list;
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

        // When the IT administrator clicks on a malfunction, it will pass the ID to a new window
        private void RowButtonClick(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)dgOpenstaandeStoringen.SelectedItems[0];
            Storing storing = new Storing(Convert.ToInt32(row["ID"]));
            storing.Show();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // Ensures that all required fields are filled in before inserting the device into the database
        private void AddDevice(object sender, RoutedEventArgs e)
        {
            if (txtNaam.Text != "" && cboDeviceType.SelectedIndex != -1 && cboAfdeling.SelectedIndex != -1)
            {
                conn.OpenConnection();
                conn.ExecuteQueries("INSERT INTO Device (DeviceTypeID, Naam, Serienummer, Afdeling, Opmerkingen, DatumToegevoegd) VALUES ( '" + Convert.ToInt32(cboDeviceType.SelectedIndex + 1) + "','" + txtNaam.Text + "','" + txtSerienummer.Text + "','" + cboAfdeling.SelectedValue + "','" + txtOpmerkingen.Text + "', date('now'))");
                conn.CloseConnection();
                Close();
            }
            else
            {
                MarkEmptyFieldsRed();
                MessageBox.Show("Niet alle verplichte velden zijn ingevuld", Title, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Ensures that all required fields are filled in before updating the device in the database
        private void UpdateDevice(object sender, RoutedEventArgs e)
        {
            if (txtNaam.Text != "" && cboDeviceType.SelectedIndex != -1 && cboAfdeling.SelectedIndex != -1)
            {
                try
                {
                    conn.OpenConnection();
                    conn.ExecuteQueries("UPDATE Device SET DeviceTypeID = '" + Convert.ToInt32(cboDeviceType.SelectedIndex + 1) + "', Naam = '" + txtNaam.Text + "', Serienummer = '" + txtSerienummer.Text + "', Afdeling = '" + cboAfdeling.SelectedValue + "', Opmerkingen = '" + txtOpmerkingen.Text + "' WHERE DeviceID = '" + id + "'");
                    btnToepassen.IsEnabled = false;

                    Button button = (Button)sender;

                    if (button.Name == "btnOK")
                        Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Er is iets misgegaan bij het updaten van de database. Excuses voor het ongemak.");
                }
                finally
                {
                    conn.CloseConnection();
                }
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

        // The user first receives a message before the device is permanently removed from the database
        private void RemoveDevice(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Device " + id + " wordt permanent verwijderd", "Device", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    conn.OpenConnection();
                    conn.ExecuteQueries("DELETE FROM Device WHERE DeviceID = '" + id + "'");
                    Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Er is iets misgegaan bij het updaten van de database. Excuses voor het ongemak.");
                }
                finally
                {
                    conn.CloseConnection();
                }
            }
        }

        // Allows the user to see which required fields must be filled
        private void MarkEmptyFieldsRed()
        {
            tbNaam.Foreground = Brushes.Black;
            tbDeviceType.Foreground = Brushes.Black;
            tbAfdeling.Foreground = Brushes.Black;

            if (txtNaam.Text == "")
                tbNaam.Foreground = Brushes.Red;

            if (cboDeviceType.SelectedIndex == -1)
                tbDeviceType.Foreground = Brushes.Red;

            if (cboAfdeling.SelectedIndex == -1)
                tbAfdeling.Foreground = Brushes.Red;
        }
    }

    // Registers all types of comboboxes that are used throughout the application
    public enum ComboboxType
    {
        Afdeling, DeviceType, DeviceTypeAll, Status, StatusAll, Medewerker, PrioriteitErnst, Month, Year
    };
}