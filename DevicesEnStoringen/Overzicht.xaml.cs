﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DevicesEnStoringen
{
    /// <summary>
    /// Interaction logic for Overzicht.xaml
    /// </summary>
    public partial class Overzicht : Window
    {
        public Overzicht()
        {
            InitializeComponent();

            UCAlleStoringen alleStoringen = new UCAlleStoringen();
            stkOverzicht.Children.Add(alleStoringen);
        }

        private void StoringenClick(object sender, RoutedEventArgs e)
        {
            Title = "Alle storingen";
            stkOverzicht.Children.Clear();
            UCAlleStoringen alleStoringen = new UCAlleStoringen();
            stkOverzicht.Children.Add(alleStoringen);
        }

        private void DevicesClick(object sender, RoutedEventArgs e)
        {
            Title = "Alle devices";
            stkOverzicht.Children.Clear();
            UCAlleDevices alleDevices = new UCAlleDevices();
            stkOverzicht.Children.Add(alleDevices);
        }

        private void DeviceTypesClick(object sender, RoutedEventArgs e)
        {
            Title = "Alle device-types";
            stkOverzicht.Children.Clear();
            UCAlleDeviceTypes alleDeviceTypes = new UCAlleDeviceTypes();
            stkOverzicht.Children.Add(alleDeviceTypes);
        }

        private void RapportagesClick(object sender, RoutedEventArgs e)
        {
            Title = "Rapportages";
            stkOverzicht.Children.Clear();
            UCRapportages rapportages = new UCRapportages();
            stkOverzicht.Children.Add(rapportages);
        }
    }
}