using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
namespace DevicesEnStoringen
{
    public partial class UCAlleDeviceTypes : UserControl
    {
        DatabaseConnection conn = new DatabaseConnection();

        Employee employee;
        public UCAlleDeviceTypes(Employee employee)
        {
            InitializeComponent();
            conn.OpenConnection();
            dgDeviceTypes.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT DeviceType.DeviceTypeID AS ID, DeviceType.Naam, COUNT(Device.DeviceTypeID) AS 'Aantal devices', DeviceType.Opmerkingen FROM DeviceType LEFT JOIN Device ON Device.DeviceTypeID = DeviceType.DeviceTypeID GROUP BY DeviceType.DeviceTypeID ORDER BY ID") });
            conn.CloseConnection();
            this.employee = employee;
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

        // When the user clicks on a device-type, it will pass the ID to a new window
        private void RowButtonClick(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)dgDeviceTypes.SelectedItems[0];
            DeviceType deviceType = new DeviceType(Convert.ToInt32(row["ID"]), employee, this);

            if (deviceType.ShowDialog().Value)
            {
                dgDeviceTypes.ItemsSource = null;
                conn.OpenConnection();
                dgDeviceTypes.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT DeviceType.DeviceTypeID AS ID, DeviceType.Naam, COUNT(Device.DeviceTypeID) AS 'Aantal devices', DeviceType.Opmerkingen FROM DeviceType LEFT JOIN Device ON Device.DeviceTypeID = DeviceType.DeviceTypeID GROUP BY DeviceType.DeviceTypeID ORDER BY ID") });
                conn.CloseConnection();
            }
        }

        private void FilterDatagrid(object sender, EventArgs e)
        {
            dgDeviceTypes.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT DeviceType.DeviceTypeID AS ID, DeviceType.Naam, COUNT(Device.DeviceTypeID) AS 'Aantal devices', DeviceType.Opmerkingen FROM DeviceType LEFT JOIN Device ON Device.DeviceTypeID = DeviceType.DeviceTypeID WHERE DeviceType.Naam LIKE '%" + txtZoek.Text + "%' GROUP BY DeviceType.DeviceTypeID ORDER BY ID") });
        }

        private void RegistreerDeviceClick(object sender, RoutedEventArgs e)
        {
            DeviceType deviceType = new DeviceType();

            if (deviceType.ShowDialog().Value)
            {
                dgDeviceTypes.ItemsSource = null;
                dgDeviceTypes.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT DeviceType.DeviceTypeID AS ID, DeviceType.Naam, COUNT(Device.DeviceTypeID) AS 'Aantal devices', DeviceType.Opmerkingen FROM DeviceType LEFT JOIN Device ON Device.DeviceTypeID = DeviceType.DeviceTypeID GROUP BY DeviceType.DeviceTypeID ORDER BY ID") });
            }
        }
        public void ClearDatabaseConnection()
        {
            dgDeviceTypes.ItemsSource = null;
        }
    }
}
