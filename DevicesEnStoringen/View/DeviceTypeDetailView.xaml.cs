using DevicesEnStoringen.Extensions;
using DevicesEnStoringen.Services;
using DevicesEnStoringen.ViewModel;
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
        private EmployeeDataService currentEmployee;
        public DeviceType SelectedDeviceType { get; set; }
        public ObservableCollection<Device> DevicesOfCurrentDeviceType { get; set; }


        // As soon as a change has occurred in one of the fields, the "submit" and "OK" button will either be enabled or disabled
        private void InputChanged(object sender, TextChangedEventArgs e)
        {
            var binding = ((TextBox)sender).GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();
        }

        // When an existing device-type is clicked
        public DeviceTypeDetailView(EmployeeDataService currentEmployee)
        {
            InitializeComponent();

            Title = "Device-type bewerken";
            //SelectedDeviceType = selectedDeviceType;
            //this.currentEmployee = currentEmployee;
            
            cvsRegistreerKnoppen.Visibility = Visibility.Hidden;
            cvsBewerkKnoppen.Visibility = Visibility.Visible;

            //if (currentEmployee.AccountTypeOfCurrentEmployee() == "IT-manager")
            //    dgDevices.Columns[4].Visibility = Visibility.Hidden;  // to do: fix dat je het juiste employee mee geeft (maak extra service?)
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

            txtNaam.Text = ""; // tijdelijk
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
        //private void UpdateDeviceType(object sender, RoutedEventArgs e)
        //{
        //    if (txtNaam.Text != "")
        //    {
        //        try
        //        {
        //            btnToepassen.IsEnabled = false;

        //            DeviceType newDeviceType = new DeviceType
        //            {
        //                DeviceTypeName = txtNaam.Text,
        //                Description = txtOpmerkingen.Text
        //            };

        //            //deviceTypeDataService.UpdateDeviceType(SelectedDeviceType, newDeviceType);

        //            Button button = (Button)sender;

        //            if (button.Name == "btnOK")
        //                DialogResult = true;
        //        }
        //        catch (Exception)
        //        {
        //            MessageBox.Show("Er is iets misgegaan bij het updaten van de database. Excuses voor het ongemak.");
        //        }
        //    }
        //    else
        //    {
        //        MarkEmptyFieldsRed();
        //        MessageBox.Show("Niet alle verplichte velden zijn ingevuld", Title, MessageBoxButton.OK, MessageBoxImage.Warning);
        //    }
        //}



        // The user first receives a message before the device-type is permanently removed from the database
        //private void RemoveDeviceType(object sender, RoutedEventArgs e)
        //{
        //    if (dgDevices.Items.Count == 0)
        //    {
        //        if (MessageBox.Show("Device-type " + SelectedDeviceType.DeviceTypeId + " wordt permanent verwijderd", "Device-type", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        //        {
        //            try
        //            {
        //                deviceTypeDataService.DeleteDeviceType(SelectedDeviceType); // Delete from the database
        //                DeviceTypeOverviewView.DeviceTypes.Remove(DeviceTypeOverviewView.DeviceTypes.Where(i => i.DeviceTypeId == SelectedDeviceType.DeviceTypeId).Single()); // Delete from the ObservableCollection

        //                DialogResult = true;
        //            }
        //            catch (Exception)
        //            {
        //                MessageBox.Show("Er is iets misgegaan bij het updaten van de database. Excuses voor het ongemak.");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Het is niet mogelijk om dit device-type te verwijderen. Zorg dat er geen devices gekoppeld zijn aan dit device-type.", "Device-type", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        // Allows the user to see which required fields must be filled
        private void MarkEmptyFieldsRed()
        {
            tbNaam.Foreground = Brushes.Black;

            if (txtNaam.Text == "")
                tbNaam.Foreground = Brushes.Red;
        }
    }
}
