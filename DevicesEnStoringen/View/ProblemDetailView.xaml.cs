using DevicesEnStoringen.Extensions;
using DevicesEnStoringen.Services;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace DevicesEnStoringen
{
    public partial class ProblemDetailView : Window
    {
        DatabaseConnection conn = new DatabaseConnection();
        private ProblemDataService problemDataService = new ProblemDataService();
        private DeviceDataService deviceDataService = new DeviceDataService();
        public static ObservableCollection<string> listStatus = FillCombobox(ComboboxType.Status);
        private int currentEmployeeID;

        public Problem SelectedProblem { get; set; }
        public ObservableCollection<Device> DevicesOfCurrentProblem { get; set; }
        public ObservableCollection<Device> AllDevices { get; set; }

        void ProblemDetailView_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = SelectedProblem;
            dgBetrokkenDevices.DataContext = this;
            dgDevicesToevoegen.DataContext = this;
        }

        // When an existing problem is clicked
        public ProblemDetailView(Problem selectedProblem)
        {
            InitializeComponent();

            Title = "Storing bewerken";
            SelectedProblem = selectedProblem;

            cboStatus.ItemsSource = FillCombobox(ComboboxType.Status);
            cboBehandeldDoor.ItemsSource = FillCombobox(ComboboxType.Medewerker);
            cboErnst.ItemsSource = FillCombobox(ComboboxType.PrioriteitErnst);
            cboPrioriteit.ItemsSource = FillCombobox(ComboboxType.PrioriteitErnst);

            DevicesOfCurrentProblem = problemDataService.GetDevicesOfCurrentProblem(SelectedProblem.ProblemId).ToObservableCollection();
            AllDevices = deviceDataService.GetAllDevices().ToObservableCollection();

            cvsRegistreerKnoppen.Visibility = Visibility.Hidden;
            cvsBewerkKnoppen.Visibility = Visibility.Visible;
            datDatumAfhandeling.IsEnabled = true;

            dgOpmerkingen.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT Date(Datum) AS Datum, Beschrijving FROM Opmerking WHERE StoringID ='" + selectedProblem.ProblemId + "'") });

            Loaded += ProblemDetailView_Loaded;
            txtToegevoegdDoor.Text = SelectedProblem.RaisedBy; // tijdelijk
        }

        // When a new malfunction is registered. It also passes an object of the employee, so this name will be displayed in the database
        public ProblemDetailView(Employee currentEmployee)
        {
            InitializeComponent();

            Title = "Storing registreren";

            cboStatus.ItemsSource = FillCombobox(ComboboxType.Status);
            cboBehandeldDoor.ItemsSource = FillCombobox(ComboboxType.Medewerker);
            cboErnst.ItemsSource = FillCombobox(ComboboxType.PrioriteitErnst);
            cboPrioriteit.ItemsSource = FillCombobox(ComboboxType.PrioriteitErnst);

            DevicesOfCurrentProblem = new ObservableCollection<Device>();
            AllDevices = deviceDataService.GetAllDevices().ToObservableCollection();

            cvsRegistreerKnoppen.Visibility = Visibility.Visible;
            cvsBewerkKnoppen.Visibility = Visibility.Hidden;

            currentEmployeeID = currentEmployee.IDOfCurrentEmployee();

            Loaded += ProblemDetailView_Loaded;
            txtToegevoegdDoor.Text = currentEmployee.FirstNameOfCurrentEmployee(); // tijdelijk
        }


        // Fill the combobox based on the combobox type 
        public static ObservableCollection<string> FillCombobox(ComboboxType type)
        {
            ObservableCollection<string> list = new ObservableCollection<string>();
            DatabaseConnection conn = new DatabaseConnection();
            conn.OpenConnection();

            if (type == ComboboxType.Status)
            {
                list = new ObservableCollection<string>();
                list.Add("Open");
                list.Add("In behandeling");
                list.Add("Afgehandeld");
            }
            else if (type == ComboboxType.StatusAll)
            {
                list = new ObservableCollection<string>();
                list.Add("Alle storingen"); 
                list.Add("Open");
                list.Add("In behandeling");
                list.Add("Afgehandeld");
            }

            else if (type == ComboboxType.Medewerker)
            {
                SQLiteDataReader dr = conn.DataReader("SELECT Voornaam FROM Medewerker");

                while (dr.Read())
                    list.Add(dr["Voornaam"].ToString());
            }

            else if (type == ComboboxType.PrioriteitErnst)
            {
                list = new ObservableCollection<string>();
                list.Add("0");
                list.Add("1");
                list.Add("2");
                list.Add("3");
            }
            conn.CloseConnection();
            return list;
        }


        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // Adds a device to a specific malfunction
        private void AddDevice(object sender, RoutedEventArgs e)
        {     
             if (!DevicesOfCurrentProblem.Any(d => d.DeviceId == ((Device)dgDevicesToevoegen.SelectedItem).DeviceId)) // don't allow duplicate devices 
                 DevicesOfCurrentProblem.Add((Device)dgDevicesToevoegen.SelectedItem);
        }

        // Removes the device from the malfunction
        private void RemoveDevice(object sender, RoutedEventArgs e)
        {
             DevicesOfCurrentProblem.Remove((Device)dgBetrokkenDevices.SelectedItem);
        }


        private void FilterDatagrid(object sender, TextChangedEventArgs e)
        {
            var _itemSourceList = new CollectionViewSource() { Source = AllDevices };

            // ICollectionView the View/UI part 
            ICollectionView Itemlist = _itemSourceList.View;

            // create and apply the filter
            var searchFilter = new Predicate<object>(item => ((Device)item).DeviceName.ToLower().Contains(txtZoek.Text.ToLower()));
            Itemlist.Filter = searchFilter;

            dgDevicesToevoegen.ItemsSource = Itemlist;
        }

        // Ensures that all required fields are filled in before inserting the malfunction into the database
        private void AddProblem(object sender, RoutedEventArgs e)
        {
            if (txtBeschrijving.Text != "" && dgBetrokkenDevices.Items.Count > 0)
            {
                Problem newProblem = new Problem
                {
                    Description = txtBeschrijving.Text,
                    Priority = cboPrioriteit.SelectedIndex,
                    Severity = cboErnst.SelectedIndex,
                    RaisedByID = currentEmployeeID,
                    Status = cboStatus.Text,
                    ClosureDate = datDatumAfhandeling.SelectedDate,
                    HandledBy = cboBehandeldDoor.SelectedIndex + 1
                };

                problemDataService.AddProblem(newProblem, DevicesOfCurrentProblem);
                DialogResult = true;   
            }
            else
            {
                MarkEmptyFieldsRed();
                MessageBox.Show("Niet alle verplichte velden zijn ingevuld", Title, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Ensures that all required fields are filled in before updating the malfunction in the database
        private void UpdateProblem(object sender, RoutedEventArgs e)
        {
            if (txtBeschrijving.Text != "" && dgBetrokkenDevices.Items.Count > 0)
            {
                Problem newProblem = new Problem
                {
                    Description = txtBeschrijving.Text,
                    Priority = cboPrioriteit.SelectedIndex,
                    Severity = cboErnst.SelectedIndex,
                    RaisedByID = currentEmployeeID,
                    Status = cboStatus.Text,
                    HandledBy = cboBehandeldDoor.SelectedIndex + 1,
                    ClosureDate = datDatumAfhandeling.SelectedDate
                };

                problemDataService.UpdateProblem(SelectedProblem, newProblem, DevicesOfCurrentProblem);
      
                
                btnToepassen.IsEnabled = false;
                Button button = (Button)sender;

                if (button.Name == "btnOK")
                    DialogResult = true;
            }
            else
            {
                MarkEmptyFieldsRed();
                MessageBox.Show("Niet alle verplichte velden zijn ingevuld", Title, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // As soon as a change has occurred in one of the fields, the "submit" button will be enabled again
        private void EnableToepassen(object sender, TextChangedEventArgs e)
        {
            if (btnToepassen.IsEnabled == false)
                btnToepassen.IsEnabled = true;
        }

        private void EnableToepassen(object sender, SelectionChangedEventArgs e)
        {
            if (btnToepassen.IsEnabled == false)
                btnToepassen.IsEnabled = true;
        }

        // The user first receives a message before the malfunction is permanently removed from the database
        private void RemoveProblem(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Storing " + SelectedProblem.ProblemId + " wordt permanent verwijderd", "Storing", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                problemDataService.DeleteProblem(SelectedProblem); // Delete from the database
                ProblemOverviewView.Problems.Remove(ProblemOverviewView.Problems.Where(i => i.ProblemId == SelectedProblem.ProblemId).Single()); // Delete from the ObservableCollection

                DialogResult = true;
            }
        }

        // Allows the user to see which required fields must be filled
        private void MarkEmptyFieldsRed()
        {
            tbBeschrijving.Foreground = Brushes.Black;
            tbBetrokkenDevices.Foreground = Brushes.Black;

            if (txtBeschrijving.Text == "")
                tbBeschrijving.Foreground = Brushes.Red;

            if (dgBetrokkenDevices.Items.Count == 0)
                tbBetrokkenDevices.Foreground = Brushes.Red;
        }

        private void RemoveComment(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)dgOpmerkingen.SelectedItems[0];

            conn.OpenConnection();
                conn.ExecuteQueries("DELETE FROM Opmerking WHERE Beschrijving = '" + row["Beschrijving"] + "' AND StoringID = '" + SelectedProblem.ProblemId + "'");
            conn.CloseConnection();

            dgOpmerkingen.ItemsSource = null;
            dgOpmerkingen.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT Date(Datum) AS Datum, Beschrijving FROM Opmerking WHERE StoringID ='" + SelectedProblem.ProblemId + "'") });
        }

        private void AddComment(object sender, RoutedEventArgs e)
        {
            if (txtOpmerking.Text != "")
            {
                conn.OpenConnection();
                conn.ExecuteQueries("INSERT INTO Opmerking (StoringID, Datum, Beschrijving) VALUES ('" + SelectedProblem.ProblemId + "', date('now'), '" + txtOpmerking.Text + "')");
                conn.CloseConnection();
                txtOpmerking.Text = "";
            }

            dgOpmerkingen.ItemsSource = null;
            dgOpmerkingen.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT Date(Datum) AS Datum, Beschrijving FROM Opmerking WHERE StoringID ='" + SelectedProblem.ProblemId + "'") });
        }
    }
}
