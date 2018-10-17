using DevicesEnStoringen.Services;
using Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

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

        public DeviceTypeDetailView(bool editMode)
        {
            InitializeComponent();

            // When an existing device-type is clicked
            if (editMode)
            {
                //this.currentEmployee = currentEmployee;

                cvsRegistreerKnoppen.Visibility = Visibility.Hidden;
                cvsBewerkKnoppen.Visibility = Visibility.Visible;

                //if (currentEmployee.AccountTypeOfCurrentEmployee() == "IT-manager")
                //    dgDevices.Columns[4].Visibility = Visibility.Hidden;  // to do: fix dat je het juiste employee mee geeft (maak extra service?)
            }
            // When a new device-type is registered
            else
            {
                cvsRegistreerKnoppen.Visibility = Visibility.Visible;
                cvsBewerkKnoppen.Visibility = Visibility.Hidden;
                cvsOpenstaandeStoringen.Visibility = Visibility.Hidden;

                Height = 180;
            }
        }

        // When the IT administrator clicks on a device, it will pass the ID to a new window (tijdelijk)
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
                //MarkEmptyFieldsRed();
                MessageBox.Show("Niet alle verplichte velden zijn ingevuld", Title, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
