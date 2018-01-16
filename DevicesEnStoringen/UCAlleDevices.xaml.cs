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
    public partial class UCAlleDevices : UserControl
    {
        DatabaseConnectie conn = new DatabaseConnectie();
        Medewerker medewerker;

        public UCAlleDevices(Medewerker medewerker)
        {
            InitializeComponent();
            grdDevices.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT Device.DeviceID AS ID, Device.Naam, DeviceType.Naam AS Type, Serienummer, Date(Device.DatumToegevoegd) AS Toegevoegd, COUNT(Storing.StoringID) AS Storingen FROM Device LEFT JOIN DeviceStoring ON DeviceStoring.DeviceID = Device.DeviceID LEFT JOIN Storing ON DeviceStoring.StoringID = Storing.StoringID AND Status='Open' LEFT JOIN DeviceType ON DeviceType.DeviceTypeID = Device.DeviceTypeID GROUP BY Device.DeviceID") });

            lstType.ItemsSource = Device.FillCombobox(ComboboxType.DeviceTypeAll);

            this.medewerker = medewerker;

            if (medewerker.accountTypeHuidigeMedewerkerIngelogd() == "IT-manager")
                btnRegistreerDevice.IsEnabled = false;
        }

        private void ChangeGridButtonPositionToEnd(object sender, EventArgs e)
        {
            var dgrd = sender as DataGrid;
            {
                var c = dgrd.Columns[0];
                dgrd.Columns.RemoveAt(0);

                if (medewerker.accountTypeHuidigeMedewerkerIngelogd() == "IT-beheerder")
                    dgrd.Columns.Add(c);
            }
        }

        private void RowButtonClick(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)grdDevices.SelectedItems[0];
            Device device = new Device(Convert.ToInt32(row["ID"]));
            device.Show();
        }

        private void FilterDatagrid(object sender, EventArgs e)
        {
            if (lstType.SelectedIndex == 0 || lstType.SelectedIndex == -1)
                grdDevices.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT Device.DeviceID AS ID, Device.Naam, DeviceType.Naam AS Type, Serienummer, Date(Device.DatumToegevoegd) AS Toegevoegd, COUNT(Storing.StoringID) AS Storingen FROM Device LEFT JOIN DeviceStoring ON DeviceStoring.DeviceID = Device.DeviceID LEFT JOIN Storing ON DeviceStoring.StoringID = Storing.StoringID AND Status='Open' LEFT JOIN DeviceType ON DeviceType.DeviceTypeID = Device.DeviceTypeID WHERE Device.Naam LIKE '%" + txtZoek.Text + "%' GROUP BY Device.DeviceID") });
            else
                grdDevices.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT Device.DeviceID AS ID, Device.Naam, DeviceType.Naam AS Type, Serienummer, Date(Device.DatumToegevoegd) AS Toegevoegd, COUNT(Storing.StoringID) AS Storingen FROM Device LEFT JOIN DeviceStoring ON DeviceStoring.DeviceID = Device.DeviceID LEFT JOIN Storing ON DeviceStoring.StoringID = Storing.StoringID AND Status='Open' LEFT JOIN DeviceType ON DeviceType.DeviceTypeID = Device.DeviceTypeID WHERE Device.Naam LIKE '%" + txtZoek.Text + "%' AND DeviceType.Naam='" + lstType.SelectedItem + "' GROUP BY Device.DeviceID") });
        }

        private void RegistreerDeviceClick(object sender, RoutedEventArgs e)
        {
            Device device = new Device();
            device.Show();
        }
    }
}
