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
        private DeviceDataService deviceDataService = new DeviceDataService();
        private ProblemDataService problemDataService = new ProblemDataService();

        public ObservableCollection<string> ComboboxDeviceTypes { get; set; }
        public static ObservableCollection<Device> Devices { get; set; }

        public DeviceOverviewView()
        {
            InitializeComponent();

            Devices = deviceDataService.GetAllDevices().ToObservableCollection();
            ComboboxDeviceTypes = problemDataService.FillCombobox(ComboboxType.DeviceTypeAll);
            Loaded += DeviceOverviewView_Loaded;

           /* if (currentEmployee.AccountTypeOfCurrentEmployee() == "IT-manager")
            {
                btnRegistreerDevice.Visibility = Visibility.Hidden;
                dgDevices.Columns[6].Visibility = Visibility.Hidden;
            }*/
        }

        void DeviceOverviewView_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = this;
        }

        // When the user clicks on a device, it will set the SelectedDevice of the new window
        private void RowButtonClick(object sender, RoutedEventArgs e)
        {
            Device selectedDevice = (Device)dgDevices.SelectedItems[0];
            DeviceDetailView deviceDetailView = new DeviceDetailView(selectedDevice)
            {
                SelectedDevice = selectedDevice
            };

            // when the user clicks cancel, force the datagrid to refresh to show the old values (temporary)
            if (!deviceDetailView.ShowDialog().Value)
                RefreshDatagrid();
        }

        // Filters the datagrid based on a textbox and a combobox
        private void FilterDatagrid(object sender, EventArgs e)
        {
            var _itemSourceList = new CollectionViewSource() { Source = Devices };

            // ICollectionView the View/UI part 
            ICollectionView Itemlist = _itemSourceList.View;
            Predicate<object> searchFilter;
            if (cboType.SelectedIndex == 0 || cboType.SelectedIndex == -1)
            {
                searchFilter = new Predicate<object>(item => ((Device)item).DeviceName.ToLower().Contains(txtZoek.Text.ToLower()));
                Itemlist.Filter = searchFilter;
            }
            else
            {
                searchFilter = new Predicate<object>(item => ((Device)item).DeviceName.ToLower().Contains(txtZoek.Text.ToLower()) && ((Device)item).DeviceTypeName == (string)cboType.SelectedItem);
                Itemlist.Filter = searchFilter;
            }

            dgDevices.ItemsSource = Itemlist;
        }

        private void RegistreerDeviceClick(object sender, RoutedEventArgs e)
        {
            DeviceDetailView deviceDetailView = new DeviceDetailView();

            // Force the datagrid to refresh after a device is registered (temporary)
            if (deviceDetailView.ShowDialog().Value)
                RefreshDatagrid();
        }

        private void RefreshDatagrid()
        {
            Devices = deviceDataService.GetAllDevices().ToObservableCollection();
            dgDevices.ItemsSource = Devices;
        }
    }
}
