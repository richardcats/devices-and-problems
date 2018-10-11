using DevicesEnStoringen.View;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DevicesEnStoringen.Services
{
    public class DialogService
    {
        Window deviceTypeDetailView = null;

        public void ShowEditDialog(DeviceType selectedDevice, EmployeeDataService currentEmployee)
        {
            deviceTypeDetailView = new DeviceTypeDetailView(currentEmployee);
            deviceTypeDetailView.ShowDialog();
        }

        public void ShowAddDialog()
        {
            deviceTypeDetailView = new DeviceTypeDetailView();
            deviceTypeDetailView.ShowDialog();
        }

        public void CloseDialog()
        {
            if (deviceTypeDetailView != null)
                deviceTypeDetailView.Close();
        }

        public bool ShowDeleteWarningMessageBox(string type, int id)
        {
            return MessageBox.Show(type + " " + id + " wordt permanent verwijderd", "Device-type", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;
        }
    }
}
