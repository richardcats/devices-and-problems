using DevicesEnStoringen.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Model;
using DevicesEnStoringen.Extensions;
using DevicesEnStoringen.View;
using System.ComponentModel;

namespace DevicesEnStoringen
{
    public partial class DeviceTypeOverviewView : UserControl
    {
        private DeviceTypeDataService deviceTypeDataService = new DeviceTypeDataService();
        private EmployeeDataService currentEmployee;
        public static ObservableCollection<DeviceType> DeviceTypes { get; set; }

        public DeviceTypeOverviewView(EmployeeDataService currentEmployee)
        {
            InitializeComponent();

            this.currentEmployee = currentEmployee;
            DeviceTypes = deviceTypeDataService.GetAllDeviceTypes().ToObservableCollection();
            Loaded += DeviceTypeOverviewView_Loaded;
        }


        void DeviceTypeOverviewView_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = this;
        }

        // When the user clicks on a device-type, it will set the SelectedDeviceType of the new window
        private void RowButtonClick(object sender, RoutedEventArgs e)
        {
            DeviceType selectedDeviceType = (DeviceType)dgDeviceTypes.SelectedItems[0];
            DeviceTypeDetailView deviceTypeDetailView = new DeviceTypeDetailView(selectedDeviceType, currentEmployee)
            {
                SelectedDeviceType = selectedDeviceType
            };

            // when the user clicks cancel, force the datagrid to refresh to show the old values (temporary)
            if (!deviceTypeDetailView.ShowDialog().Value)
                RefreshDatagrid();
        }

        private void FilterDatagrid(object sender, EventArgs e)
        {
            var _itemSourceList = new CollectionViewSource() { Source = DeviceTypes };

            // ICollectionView the View/UI part 
            ICollectionView Itemlist = _itemSourceList.View;

            // create and apply the filter
            var searchFilter = new Predicate<object>(item => ((DeviceType)item).DeviceTypeName.ToLower().Contains(txtZoek.Text.ToLower()));
            Itemlist.Filter = searchFilter;

            dgDeviceTypes.ItemsSource = Itemlist;
        }

        private void RegisterDeviceTypeClick(object sender, RoutedEventArgs e)
        {
            DeviceTypeDetailView deviceTypeDetailView = new DeviceTypeDetailView();

            // Force the datagrid to refresh after a device-type is registered (temporary)
            if (deviceTypeDetailView.ShowDialog().Value)
                RefreshDatagrid();
        }

        private void RefreshDatagrid()
        {
            DeviceTypes = deviceTypeDataService.GetAllDeviceTypes().ToObservableCollection();
            dgDeviceTypes.ItemsSource = DeviceTypes;
        }
    }
}
