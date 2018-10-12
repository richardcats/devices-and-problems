﻿using DevicesEnStoringen.View;
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
            deviceTypeDetailView = new DeviceTypeDetailView(true);
            deviceTypeDetailView.ShowDialog();
        }

        public void ShowAddDialog()
        {
            deviceTypeDetailView = new DeviceTypeDetailView(false);
            deviceTypeDetailView.ShowDialog();
        }

        public void CloseDialog()
        {
            if (deviceTypeDetailView != null)
                deviceTypeDetailView.Close();
        }

        public bool ShowRemoveWarningMessageBox(string type, int id)
        {
            return MessageBox.Show($"{type} {id} wordt permanent verwijderd", "Device-type", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;
        }

        public void CanNotRemoveMessageBox(string deletableType, string preventingTypes)
        {
            MessageBox.Show($"Het is niet mogelijk om dit {deletableType} te verwijderen. Zorg dat er geen {preventingTypes} gekoppeld zijn aan dit {deletableType}.", deletableType, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public void ShowEmptyFieldMessageBox()
        {
            MessageBox.Show("Niet alle verplichte velden zijn ingevuld", "Ontbrekende informatie", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
