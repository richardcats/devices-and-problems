﻿using DevicesAndProblems.App.Extensions;
using DevicesAndProblems.App.Services;
using DevicesAndProblems.Model;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DevicesAndProblems.App.View
{
    public partial class DeviceDetailView : Window
    {
        private ProblemDataService problemDataService = new ProblemDataService();
        //private DeviceDataService deviceDataService = new DeviceDataService();
        public Device SelectedDevice { get; set; }
        public ObservableCollection<Problem> CurrentProblems { get; set; }


        // When an existing device is clicked
        public DeviceDetailView(bool editMode)
        {
            InitializeComponent();

            if (editMode) // When an existing device-type is clicked
            {
                cvsRegisterButtons.Visibility = Visibility.Hidden;
                cvsEditButtons.Visibility = Visibility.Visible;
            }
            else // When a new device-type is registered
            {
                cvsRegisterButtons.Visibility = Visibility.Visible;
                cvsEditButtons.Visibility = Visibility.Hidden;
                cvsActiveProblems.Visibility = Visibility.Hidden;

                Height = 270;
            }
        }
        // As soon as a change has occurred in one of the fields, the "submit" and "OK" button will either be enabled or disabled
        private void InputChanged(object sender, TextChangedEventArgs e)
        {
            var binding = ((TextBox)sender).GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();
        }

        // As soon as a change has occurred in one of the fields, the "submit" and "OK" button will either be enabled or disabled
        private void InputChanged(object sender, SelectionChangedEventArgs e)
        {
            var binding = ((ComboBox)sender).GetBindingExpression(ComboBox.SelectedValueProperty);
            binding.UpdateSource();
        }

        // When the IT administrator clicks on a malfunction, it will pass the ID to a new window
        /*private void RowButtonClick(object sender, RoutedEventArgs e)
        {
            Problem row = (Problem)dgOpenstaandeStoringen.SelectedItems[0];
            //Storing storing = new Storing(Convert.ToInt32(row["ID"]));
            // storing.Show();
        }*/

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // Ensures that all required fields are filled in before inserting the device into the database
       /* private void AddDevice(object sender, RoutedEventArgs e)
        {
            if (txtNaam.Text != "" && cboDeviceType.SelectedIndex != -1 && cboAfdeling.Text != "")
            {
                Device newDevice = new Device
                {
                    Name = txtNaam.Text,
                    DeviceTypeValue = cboDeviceType.SelectedIndex + 1,
                    DeviceTypeName = cboDeviceType.Text,
                    Department = cboAfdeling.Text,
                    SerialNumber = txtSerienummer.Text,
                    Comments = txtOpmerkingen.Text
                };

                //deviceDataService.AddDevice(newDevice);
                DialogResult = true;
            }
            else
            {
                MarkEmptyFieldsRed();
                MessageBox.Show("Niet alle verplichte velden zijn ingevuld", Title, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }*/

        // Ensures that all required fields are filled in before updating the device in the database
        /*private void UpdateDevice(object sender, RoutedEventArgs e)
        {
            if (txtNaam.Text != "" && cboDeviceType.SelectedIndex != -1 && cboAfdeling.Text != "")
            {
                try
                {
                    Device newDevice = new Device
                    {
                        Name = txtNaam.Text,
                        DeviceTypeValue = cboDeviceType.SelectedIndex + 1,
                        DeviceTypeName = cboDeviceType.Text,
                        Department = cboAfdeling.Text,
                        SerialNumber = txtSerienummer.Text,
                        Comments = txtOpmerkingen.Text
                    };

                    //deviceDataService.UpdateDevice(SelectedDevice, newDevice);

                    btnToepassen.IsEnabled = false;
                    Button button = (Button)sender;

                    if (button.Name == "btnOK")
                        DialogResult = true;
                }
                catch (Exception)
                {
                    MessageBox.Show("Er is iets misgegaan bij het updaten van de database. Excuses voor het ongemak.");
                }
            }
            else
            {
                MarkEmptyFieldsRed();
                MessageBox.Show("Niet alle verplichte velden zijn ingevuld", Title, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }*/

        // As soon as a change has occurred in one of the fields, the "submit" button will be enabled again
       /* private void EnableToepassen(object sender, TextChangedEventArgs e)
        {
            if (btnToepassen.IsEnabled == false)
                btnToepassen.IsEnabled = true;
        }

        private void EnableToepassen(object sender, SelectionChangedEventArgs e)
        {
            if (btnToepassen.IsEnabled == false)
                btnToepassen.IsEnabled = true;
        }*/

        // The user first receives a message before the device is permanently removed from the database
        private void RemoveDevice(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Device " + SelectedDevice.Id + " wordt permanent verwijderd", "Device", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    //deviceDataService.DeleteDeviceType(SelectedDevice); // Delete from the database
                    DeviceOverviewView.Devices.Remove(DeviceOverviewView.Devices.Where(i => i.Id == SelectedDevice.Id).Single()); // Delete from the ObservableCollection

                    DialogResult = true;
                }
                catch (Exception)
                {
                    MessageBox.Show("Er is iets misgegaan bij het updaten van de database. Excuses voor het ongemak.");
                }
            }
        }

        // Allows the user to see which required fields must be filled
        /* private void MarkEmptyFieldsRed()
         {
             tbNaam.Foreground = Brushes.Black;
             tbDeviceType.Foreground = Brushes.Black;
             tbAfdeling.Foreground = Brushes.Black;

             if (txtNaam.Text == "")
                 tbNaam.Foreground = Brushes.Red;

             if (cboDeviceType.SelectedIndex == -1)
                 tbDeviceType.Foreground = Brushes.Red;

             if (cboAfdeling.SelectedIndex == -1)
                 tbAfdeling.Foreground = Brushes.Red;
         }*/
        //}
    }
    // Registers all types of comboboxes that are used throughout the application
    public enum ComboboxType
    {
        Afdeling, DeviceType, DeviceTypeAll, Status, StatusAll, Medewerker, PrioriteitErnst, Month, Year
    };
}