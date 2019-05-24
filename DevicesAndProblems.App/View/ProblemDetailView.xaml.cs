using DevicesAndProblems.App.Extensions;
using DevicesAndProblems.App.Services;
using DevicesAndProblems.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace DevicesAndProblems.App.View
{
    public partial class ProblemDetailView : Window
    {
        private ProblemDataService problemDataService = new ProblemDataService();
        //private DeviceDataService deviceDataService = new DeviceDataService();
        public static ObservableCollection<string> listStatus = FillCombobox(ComboboxType.Status);
        private int currentEmployeeID;

        public Problem SelectedProblem { get; set; }
        public ObservableCollection<Device> DevicesOfCurrentProblem { get; set; }
        public ObservableCollection<Device> AllDevices { get; set; }
        public ObservableCollection<Comment> Comments { get; set; }

        void ProblemDetailView_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = SelectedProblem;
            dgBetrokkenDevices.DataContext = this;
            dgDevicesToevoegen.DataContext = this;
            dgOpmerkingen.DataContext = this;
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
            //AllDevices = deviceDataService.GetAllDevices().ToObservableCollection();
            Comments = problemDataService.GetCommentsOfCurrentProblem(SelectedProblem).ToObservableCollection();

            cvsRegistreerKnoppen.Visibility = Visibility.Hidden;
            cvsBewerkKnoppen.Visibility = Visibility.Visible;
            datDatumAfhandeling.IsEnabled = true;

            Loaded += ProblemDetailView_Loaded;
            txtToegevoegdDoor.Text = SelectedProblem.RaisedBy; // tijdelijk
        }

        // When a new malfunction is registered. It also passes an object of the employee, so this name will be displayed in the database
        public ProblemDetailView(EmployeeDataService currentEmployee)
        {
            InitializeComponent();

            Title = "Storing registreren";

            cboStatus.ItemsSource = FillCombobox(ComboboxType.Status);
            cboBehandeldDoor.ItemsSource = FillCombobox(ComboboxType.Medewerker);
            cboErnst.ItemsSource = FillCombobox(ComboboxType.PrioriteitErnst);
            cboPrioriteit.ItemsSource = FillCombobox(ComboboxType.PrioriteitErnst);

            DevicesOfCurrentProblem = new ObservableCollection<Device>();
           // AllDevices = deviceDataService.GetAllDevices().ToObservableCollection();

            cvsRegistreerKnoppen.Visibility = Visibility.Visible;
            cvsBewerkKnoppen.Visibility = Visibility.Hidden;

            currentEmployeeID = currentEmployee.IDOfCurrentEmployee();

            Loaded += ProblemDetailView_Loaded;
            txtToegevoegdDoor.Text = currentEmployee.FirstNameOfCurrentEmployee(); // tijdelijk
        }


        // Fill the combobox based on the combobox type 
        public static ObservableCollection<string> FillCombobox(ComboboxType type)
        {
            ObservableCollection<string> comboboxItems = new ObservableCollection<string>();
            DatabaseConnection conn = new DatabaseConnection();

            if (type == ComboboxType.Status)
            {
                comboboxItems = new ObservableCollection<string>();
                comboboxItems.Add("Open");
                comboboxItems.Add("In behandeling");
                comboboxItems.Add("Afgehandeld");
            }
            else if (type == ComboboxType.StatusAll)
            {
                comboboxItems = new ObservableCollection<string>();
                comboboxItems.Add("Alle storingen");
                comboboxItems.Add("Open");
                comboboxItems.Add("In behandeling");
                comboboxItems.Add("Afgehandeld");
            }

            else if (type == ComboboxType.Medewerker)
            {
                EmployeeDataService employeeDataService = new EmployeeDataService();
                comboboxItems = employeeDataService.GetAllEmployees();
            }

            else if (type == ComboboxType.PrioriteitErnst)
            {
                comboboxItems = new ObservableCollection<string>();
                comboboxItems.Add("0");
                comboboxItems.Add("1");
                comboboxItems.Add("2");
                comboboxItems.Add("3");
            }
            return comboboxItems;
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
            var searchFilter = new Predicate<object>(item => ((Device)item).Name.ToLower().Contains(txtZoek.Text.ToLower()));
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
            Comment selectedComment = (Comment)dgOpmerkingen.SelectedItems[0];

            problemDataService.RemoveComment(selectedComment, SelectedProblem); // Delete from the database
            Comments.Remove(Comments.Where(i => i.CommentID == selectedComment.CommentID).Single()); // Delete from the ObservableCollection
        }

        private void AddComment(object sender, RoutedEventArgs e)
        {
            if (txtOpmerking.Text != "")
            {
                Comment newComment = new Comment
                {
                    Date = DateTime.Now,
                    Text = txtOpmerking.Text
                };

                problemDataService.AddComment(SelectedProblem, newComment); // Add to the database
                Comments.Add(newComment); // Add to the ObservableCollection

                txtOpmerking.Text = "";
            }
        }
    }
}
