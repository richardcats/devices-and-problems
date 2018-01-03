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

        public UCAlleDevices()
        {
            InitializeComponent();
            grdDevices.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT Device.DeviceID AS ID, Device.Naam, DeviceType.Naam AS Type, Serienummer, Date(Device.DatumToegevoegd) AS Toegevoegd, COUNT(Storing.DeviceID) AS Storingen FROM Device LEFT JOIN Storing on Device.DeviceID = Storing.DeviceID LEFT JOIN DeviceType ON DeviceType.DeviceTypeID = Device.DeviceTypeID GROUP BY Device.DeviceID") });

            lstType.ItemsSource = Storing.FillCombobox();
            Storing.list.Insert(0, "Alle storingen");

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
            if ((string)lstType.SelectedItem == "Alle storingen" || lstType.SelectedIndex == -1)
                grdDevices.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT DeviceID AS ID, Device.Naam, DeviceType.Naam AS Type, Serienummer, Date(DatumToegevoegd) AS Toegevoegd FROM Device INNER JOIN DeviceType ON DeviceType.DeviceTypeID = Device.DeviceTypeID WHERE Device.Naam LIKE '%" + txtZoek.Text + "%'") });
            else
                grdDevices.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT DeviceID AS ID, Device.Naam, DeviceType.Naam AS Type, Serienummer, Date(DatumToegevoegd) AS Toegevoegd FROM Device INNER JOIN DeviceType ON DeviceType.DeviceTypeID = Device.DeviceTypeID WHERE Device.Naam LIKE '%" + txtZoek.Text + "%' AND DeviceType.Naam='" + lstType.SelectedItem + "'") });
        }

        private void RegistreerDeviceClick(object sender, RoutedEventArgs e)
        {
            Storing storing = new Storing();
            storing.Show();
        }
    }
}
