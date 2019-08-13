using DevicesAndProblems.App.Extensions;
using DevicesAndProblems.App.Services;
using DevicesAndProblems.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DevicesAndProblems.App.View
{
    public partial class DeviceOverviewView : UserControl
    {
       // private DeviceDataService deviceDataService = new DeviceDataService();



        public static ObservableCollection<Device> Devices { get; set; }

        public DeviceOverviewView()
        {
            InitializeComponent();
        }

        // As soon as a change has occurred in the search field, force the DataGrid to update
        private void SearchInputChanged(object sender, EventArgs e)
        {
            var binding = ((TextBox)sender).GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();
        }

        private void DeviceTypeChanged(object sender, EventArgs e)
        {
            var binding = ((ComboBox)sender).GetBindingExpression(ComboBox.SelectedIndexProperty);
            binding.UpdateSource();
        }
        

        // When the user clicks on a device, it will set the SelectedDevice of the new window
        private void RowButtonClick(object sender, RoutedEventArgs e)
        {
            //Device selectedDevice = (Device)dgDevices.SelectedItems[0];
           /* DeviceDetailView deviceDetailView = new DeviceDetailView(selectedDevice)
            {
                SelectedDevice = selectedDevice
            };*/

            // when the user clicks cancel, force the datagrid to refresh to show the old values (temporary)
            //if (!deviceDetailView.ShowDialog().Value)
            //    RefreshDatagrid();
        }


       /* private void RegistreerDeviceClick(object sender, RoutedEventArgs e)
        {
            DeviceDetailView deviceDetailView = new DeviceDetailView();

            // Force the datagrid to refresh after a device is registered (temporary)
            if (deviceDetailView.ShowDialog().Value)
                RefreshDatagrid();
        }*/

        private void RefreshDatagrid()
        {
           // Devices = deviceDataService.GetAllDevices().ToObservableCollection();
            //dgDevices.ItemsSource = Devices;
        }
    }
}
