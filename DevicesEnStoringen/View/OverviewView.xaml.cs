﻿using DevicesEnStoringen.Services;
using DevicesEnStoringen.Utility;
using System.Windows;

namespace DevicesEnStoringen
{
    public partial class OverviewView : Window
    {
        private ProblemOverviewView problemOverviewView;
        public OverviewView()
        {
            InitializeComponent();

            problemOverviewView = new ProblemOverviewView();
            overviewContainer.Children.Add(problemOverviewView);
            
            // tijdelijk 
         //   if (currentEmployee.AccountTypeOfCurrentEmployee() == "IT-manager")
         //       btnRapportages.Visibility = Visibility.Visible;
        }

        private void StoringenClick(object sender, RoutedEventArgs e)
        {
            Title = "Alle storingen";
            overviewContainer.Children.Clear();
            problemOverviewView = new ProblemOverviewView();
            overviewContainer.Children.Add(problemOverviewView);
        }

        private void DevicesClick(object sender, RoutedEventArgs e)
        {
            Title = "Alle devices";
            overviewContainer.Children.Clear();
            DeviceOverviewView deviceOverviewView = new DeviceOverviewView();
            overviewContainer.Children.Add(deviceOverviewView);
        }

        private void DeviceTypesClick(object sender, RoutedEventArgs e)
        {
            Title = "Alle device-types";
            overviewContainer.Children.Clear();
            DeviceTypeOverviewView deviceTypeOverviewView = new DeviceTypeOverviewView();
            overviewContainer.Children.Add(deviceTypeOverviewView);
            Messenger.Default.Send("OpenDeviceTypeOverview");
        }

        private void RapportagesClick(object sender, RoutedEventArgs e)
        {
            Title = "Rapportage storingen";
            overviewContainer.Children.Clear();
            ReportsView reportsView = new ReportsView();
            overviewContainer.Children.Add(reportsView);
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            LoginView loginView = new LoginView();
            loginView.Show();
            Close();
        }
    }
}