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

        public void ShowEditDialog()
        {
            deviceTypeDetailView = new DeviceTypeDetailView();
            deviceTypeDetailView.ShowDialog();
        }

        public void ShowEditDialog(DeviceType selectedDevice, EmployeeDataService currentEmployee)
        {
            deviceTypeDetailView = new DeviceTypeDetailView(selectedDevice, currentEmployee);
            deviceTypeDetailView.ShowDialog();
        }

        public void CloseDialog()
        {
            if (deviceTypeDetailView != null)
                deviceTypeDetailView.Close();
        }
    }
}
