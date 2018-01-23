using System;
using System.Data;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace DevicesEnStoringen
{
    public partial class DeviceType : Window
    {
        DatabaseConnection conn = new DatabaseConnection();
        int id;
        Employee employee;

        // When an existing device-type is clicked
        public DeviceType(int id, Employee employee)
        {
            InitializeComponent();

            Title = "Device-type bewerken";

            FillTextBoxes(id);

            grdDevices.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT DeviceID AS ID, Naam, Afdeling, Date(DatumToegevoegd) AS Datum FROM Device WHERE DeviceTypeID = '" + id + "'") });

            cvsRegistreerKnoppen.Visibility = Visibility.Hidden;
            cvsBewerkKnoppen.Visibility = Visibility.Visible;

            this.id = id;
            this.employee = employee;
        }

        // When a new device-type is registered
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

        // Ensures that the manage device button is placed at the end of the datagrid. Only the IT Administrator will see this.
        private void ChangeGridButtonPositionToEnd(object sender, EventArgs e)
        {
            var dgrd = sender as DataGrid;
            {
                var c = dgrd.Columns[0];
                dgrd.Columns.RemoveAt(0);

                if (employee.AccountTypeOfCurrentEmployee() == "IT-beheerder")
                    dgrd.Columns.Add(c);
            }
        }

        // When the IT administrator clicks on a device, it will pass the ID to a new window
        private void RowButtonClick(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)grdDevices.SelectedItems[0];
            Device device = new Device(Convert.ToInt32(row["ID"]));
            device.Show();
        }

        // Ensures that all required fields are filled in before inserting the device-type into the database
        private void AddDeviceType(object sender, RoutedEventArgs e)
        {
            if (txtNaam.Text != "")
            {
                conn.OpenConnection();
                conn.ExecuteQueries("INSERT INTO DeviceType (Naam, Opmerkingen) VALUES ( '" + txtNaam.Text + "','" + txtOpmerkingen.Text + "')");
                conn.CloseConnection();
                DialogResult = true;
            }
            else
            {
                MarkEmptyFieldsRed();
                MessageBox.Show("Niet alle verplichte velden zijn ingevuld", Title, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Ensures that all required fields are filled in before updating the device-type in the database
        private void UpdateDeviceType(object sender, RoutedEventArgs e)
        {
            if (txtNaam.Text != "")
            {
                try
                {
                    conn.OpenConnection();
                    conn.ExecuteQueries("UPDATE DeviceType SET Naam = '" + txtNaam.Text + "', Opmerkingen = '" + txtOpmerkingen.Text + "' WHERE DeviceTypeID = '" + id + "'");
                    btnToepassen.IsEnabled = false;

                    Button button = (Button)sender;

                    if (button.Name == "btnOK")
                        DialogResult = true;
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

        // The user first receives a message before the device-type is permanently removed from the database
        private void RemoveDeviceType(object sender, RoutedEventArgs e)
        {
            if (grdDevices.Items.Count == 0)
            {

                if (MessageBox.Show("Device-type " + id + " wordt permanent verwijderd", "Device-type", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    try
                    {
                        conn.OpenConnection();
                        conn.ExecuteQueries("DELETE FROM DeviceType WHERE DeviceTypeID = '" + id + "'");
                        DialogResult = true;
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
                
            else
            {
                MessageBox.Show("Het is niet mogelijk om dit device-type te verwijderen. Zorg dat er geen devices gekoppeld zijn aan dit device-type.", "Device-type", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Allows the user to see which required fields must be filled
        private void MarkEmptyFieldsRed()
        {
            tbNaam.Foreground = Brushes.Black;

            if (txtNaam.Text == "")
                tbNaam.Foreground = Brushes.Red;
        }
    }
}
