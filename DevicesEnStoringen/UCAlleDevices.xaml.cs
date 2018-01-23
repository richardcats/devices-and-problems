using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
namespace DevicesEnStoringen
{
    public partial class UCAlleDevices : UserControl
    {
        DatabaseConnection conn = new DatabaseConnection();
        Employee employee;

        public UCAlleDevices(Employee employee)
        {
            InitializeComponent();


            dgDevices.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT Device.DeviceID AS ID, Device.Naam, DeviceType.Naam AS Type, Serienummer, Date(Device.DatumToegevoegd) AS Toegevoegd, COUNT(Storing.StoringID) AS Storingen FROM Device LEFT JOIN DeviceStoring ON DeviceStoring.DeviceID = Device.DeviceID LEFT JOIN Storing ON DeviceStoring.StoringID = Storing.StoringID AND Status='Open' LEFT JOIN DeviceType ON DeviceType.DeviceTypeID = Device.DeviceTypeID GROUP BY Device.DeviceID") });

            cboType.ItemsSource = Device.FillCombobox(ComboboxType.DeviceTypeAll);

            this.employee = employee;

            if (employee.AccountTypeOfCurrentEmployee() == "IT-manager")
                btnRegistreerDevice.Visibility = Visibility.Hidden;
        }

        // Ensures that the manage device button is placed at the end of the datagrid
        private void ChangeGridButtonPositionToEnd(object sender, EventArgs e)
        {
            var dgrd = sender as DataGrid;
            {
                var c = dgrd.Columns[0];
                dgrd.Columns.RemoveAt(0);

                if (employee.AccountTypeOfCurrentEmployee   () == "IT-beheerder")
                    dgrd.Columns.Add(c);
            }
        }

        // When the IT administrator clicks on a device, it will pass the ID to a new window
        private void RowButtonClick(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)dgDevices.SelectedItems[0];
            Device device = new Device(Convert.ToInt32(row["ID"]));

            if (device.ShowDialog().Value)
            {
                dgDevices.ItemsSource = null;
                dgDevices.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT Device.DeviceID AS ID, Device.Naam, DeviceType.Naam AS Type, Serienummer, Date(Device.DatumToegevoegd) AS Toegevoegd, COUNT(Storing.StoringID) AS Storingen FROM Device LEFT JOIN DeviceStoring ON DeviceStoring.DeviceID = Device.DeviceID LEFT JOIN Storing ON DeviceStoring.StoringID = Storing.StoringID AND Status='Open' LEFT JOIN DeviceType ON DeviceType.DeviceTypeID = Device.DeviceTypeID GROUP BY Device.DeviceID") });
            }
        }

        // Filters the datagrid based on a textbox and a combobox
        private void FilterDatagrid(object sender, EventArgs e)
        {
            if (cboType.SelectedIndex == 0 || cboType.SelectedIndex == -1)
                dgDevices.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT Device.DeviceID AS ID, Device.Naam, DeviceType.Naam AS Type, Serienummer, Date(Device.DatumToegevoegd) AS Toegevoegd, COUNT(Storing.StoringID) AS Storingen FROM Device LEFT JOIN DeviceStoring ON DeviceStoring.DeviceID = Device.DeviceID LEFT JOIN Storing ON DeviceStoring.StoringID = Storing.StoringID AND Status='Open' LEFT JOIN DeviceType ON DeviceType.DeviceTypeID = Device.DeviceTypeID WHERE Device.Naam LIKE '%" + txtZoek.Text + "%' GROUP BY Device.DeviceID") });
            else
                dgDevices.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT Device.DeviceID AS ID, Device.Naam, DeviceType.Naam AS Type, Serienummer, Date(Device.DatumToegevoegd) AS Toegevoegd, COUNT(Storing.StoringID) AS Storingen FROM Device LEFT JOIN DeviceStoring ON DeviceStoring.DeviceID = Device.DeviceID LEFT JOIN Storing ON DeviceStoring.StoringID = Storing.StoringID AND Status='Open' LEFT JOIN DeviceType ON DeviceType.DeviceTypeID = Device.DeviceTypeID WHERE Device.Naam LIKE '%" + txtZoek.Text + "%' AND DeviceType.Naam='" + cboType.SelectedItem + "' GROUP BY Device.DeviceID") });
        }


        private void RegistreerDeviceClick(object sender, RoutedEventArgs e)
        {
            Device device = new Device();

            if (device.ShowDialog().Value)
            {
                dgDevices.ItemsSource = null;
                dgDevices.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT Device.DeviceID AS ID, Device.Naam, DeviceType.Naam AS Type, Serienummer, Date(Device.DatumToegevoegd) AS Toegevoegd, COUNT(Storing.StoringID) AS Storingen FROM Device LEFT JOIN DeviceStoring ON DeviceStoring.DeviceID = Device.DeviceID LEFT JOIN Storing ON DeviceStoring.StoringID = Storing.StoringID AND Status='Open' LEFT JOIN DeviceType ON DeviceType.DeviceTypeID = Device.DeviceTypeID GROUP BY Device.DeviceID") });
            }
        }
    }
}
