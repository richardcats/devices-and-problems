using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
namespace DevicesEnStoringen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class UCAlleDeviceTypes : UserControl
    {
        DatabaseConnectie conn = new DatabaseConnectie();

        public UCAlleDeviceTypes()
        {
            InitializeComponent();
            grdDevices.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT DeviceType.DeviceTypeID AS ID, DeviceType.Naam, COUNT(Device.DeviceTypeID) AS 'Aantal devices', Opmerkingen FROM DeviceType LEFT JOIN Device ON Device.DeviceTypeID = DeviceType.DeviceTypeID GROUP BY Device.DeviceTypeID ORDER BY ID") });
        }

        private void ChangeGridButtonPositionToEnd(object sender, EventArgs e)
        {
            var dgrd = sender as DataGrid;
            {
                var c = dgrd.Columns[0];
                dgrd.Columns.RemoveAt(0);
                dgrd.Columns.Add(c);
            }
        }

        private void RowButtonClick(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)grdDevices.SelectedItems[0];
            Storing storing = new Storing(Convert.ToInt32(row["ID"]));
            storing.Show();
        }

        private void FilterDatagrid(object sender, EventArgs e)
        {
            grdDevices.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT DeviceType.DeviceTypeID AS ID, DeviceType.Naam, COUNT(Device.DeviceTypeID) AS 'Aantal devices', Opmerkingen FROM DeviceType LEFT JOIN Device ON Device.DeviceTypeID = DeviceType.DeviceTypeID WHERE DeviceType.Naam LIKE '%" + txtZoek.Text + "%' GROUP BY Device.DeviceTypeID ORDER BY ID") });
        }

        private void RegistreerDeviceClick(object sender, RoutedEventArgs e)
        {
            Storing storing = new Storing();
            storing.Show();
        }
    }
}
