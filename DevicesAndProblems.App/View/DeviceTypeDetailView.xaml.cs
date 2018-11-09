using System.Windows;
using System.Windows.Controls;

namespace DevicesAndProblems.App.View
{
    public partial class DeviceTypeDetailView : Window
    {        
        public DeviceTypeDetailView(bool editMode)
        {
            InitializeComponent();

            if (editMode) // When an existing device-type is clicked
            {
                cvsRegisterButtons.Visibility = Visibility.Hidden;
                cvsEditButtons.Visibility = Visibility.Visible;
            }
            else // When a new device-type is registered
            {
                cvsRegisterButtons.Visibility = Visibility.Visible;
                cvsEditButtons.Visibility = Visibility.Hidden;
                cvsOpenProblems.Visibility = Visibility.Hidden;

                Height = 180;
            }
        }

        // As soon as a change has occurred in one of the fields, the "submit" and "OK" button will either be enabled or disabled
        private void InputChanged(object sender, TextChangedEventArgs e)
        {
            var binding = ((TextBox)sender).GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();
        }

        // When the IT administrator clicks on a device, it will pass the ID to a new window (temporary - remove as soon as there are view models for problems and devices)
        private void RowButtonClick(object sender, RoutedEventArgs e)
        {
           // Device selectedDevice = (Device)dgDevices.SelectedItems[0];
           // DeviceDetailView device = new DeviceDetailView(selectedDevice); // tijdelijk
           // device.Show();
        }
    }
}
