using DevicesEnStoringen.Extensions;
using DevicesEnStoringen.Services;
using Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DevicesEnStoringen.View
{
    public partial class DeviceTypeDetailView : Window
    {
        private DeviceTypeDataService deviceTypeDataService = new DeviceTypeDataService();
        private Employee currentEmployee;
        public DeviceType SelectedDeviceType { get; set; }
        public ObservableCollection<Device> DevicesOfCurrentDeviceType { get; set; }


        void DeviceTypeDetailView_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = SelectedDeviceType;
            dgDevices.DataContext = this;    
        }


        // When an existing device-type is clicked
        public DeviceTypeDetailView(DeviceType selectedDeviceType, Employee currentEmployee)
        {
            InitializeComponent();

            Title = "Device-type bewerken";
            SelectedDeviceType = selectedDeviceType;
            this.currentEmployee = currentEmployee;
            DevicesOfCurrentDeviceType = deviceTypeDataService.GetDevicesOfDeviceType(SelectedDeviceType.DeviceTypeId).ToObservableCollection();
            
            cvsRegistreerKnoppen.Visibility = Visibility.Hidden;
            cvsBewerkKnoppen.Visibility = Visibility.Visible;

            Loaded += DeviceTypeDetailView_Loaded;
    }

        // When a new device-type is registered
        public DeviceTypeDetailView()
        {
            InitializeComponent();

            Title = "Device-type registreren";

            cvsRegistreerKnoppen.Visibility = Visibility.Visible;
            cvsBewerkKnoppen.Visibility = Visibility.Hidden;
            cvsOpenstaandeStoringen.Visibility = Visibility.Hidden;

            Height = 180;
        }

        // When the IT administrator clicks on a device, it will pass the ID to a new window
        private void RowButtonClick(object sender, RoutedEventArgs e)
        {
            Device selectedDevice = (Device)dgDevices.SelectedItems[0];
            DeviceDetailView device = new DeviceDetailView(selectedDevice); // tijdelijk
            device.Show();
        }

        // Ensures that all required fields are filled in before inserting the device-type into the database
        private void AddDeviceType(object sender, RoutedEventArgs e)
        {
            if (txtNaam.Text != "")
            {
                DeviceType newDeviceType = new DeviceType
                {
                    DeviceTypeName = txtNaam.Text,
                    Description = txtOpmerkingen.Text
                };

                deviceTypeDataService.AddDeviceType(newDeviceType);
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
                    btnToepassen.IsEnabled = false;

                    DeviceType newDeviceType = new DeviceType
                    {
                        DeviceTypeName = txtNaam.Text,
                        Description = txtOpmerkingen.Text
                    };

                    deviceTypeDataService.UpdateDeviceType(SelectedDeviceType, newDeviceType);

                    Button button = (Button)sender;

                    if (button.Name == "btnOK")
                        DialogResult = true;
                }
                catch (Exception)
                {
                    MessageBox.Show("Er is iets misgegaan bij het updaten van de database. Excuses voor het ongemak.");
                }
            }
            else
            {
                MarkEmptyFieldsRed();
                MessageBox.Show("Niet alle verplichte velden zijn ingevuld", Title, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
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
            if (dgDevices.Items.Count == 0)
            {
                if (MessageBox.Show("Device-type " + SelectedDeviceType.DeviceTypeId + " wordt permanent verwijderd", "Device-type", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    try
                    {
                        deviceTypeDataService.DeleteDeviceType(SelectedDeviceType); // Delete from the database
                        DeviceTypeOverviewView.DeviceTypes.Remove(DeviceTypeOverviewView.DeviceTypes.Where(i => i.DeviceTypeId == SelectedDeviceType.DeviceTypeId).Single()); // Delete from the ObservableCollection

                        DialogResult = true;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Er is iets misgegaan bij het updaten van de database. Excuses voor het ongemak.");
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
