using DevicesAndProblems.App.Extensions;
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
        // When an existing device is clicked
        public DeviceDetailView(bool editMode)
        {
            InitializeComponent();

            if (editMode) // When an existing device is clicked
            {
                cvsRegisterButtons.Visibility = Visibility.Hidden;
                cvsEditButtons.Visibility = Visibility.Visible;
            }
            else // When a new device is registered
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


    }
    // Registers all types of comboboxes that are used throughout the application

}