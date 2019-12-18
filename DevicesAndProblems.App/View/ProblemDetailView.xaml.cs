using DevicesAndProblems.App.Extensions;
using DevicesAndProblems.App.Services;
using DevicesAndProblems.DAL.SQLite;
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
        private ProblemDataService problemDataService; //= new ProblemDataService();
        //private DeviceDataService deviceDataService = new DeviceDataService();

        

        public Problem SelectedProblem { get; set; }
        public ObservableCollection<Device> DevicesOfCurrentProblem { get; set; }
        public ObservableCollection<Device> AllDevices { get; set; }
        public ObservableCollection<Comment> Comments { get; set; }

        void ProblemDetailView_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = SelectedProblem;
            dgBetrokkenDevices.DataContext = this;
            dgAddDevices.DataContext = this;
            dgOpmerkingen.DataContext = this;
        }


        // When a new malfunction is registered. It also passes an object of the employee, so this name will be displayed in the database
        public ProblemDetailView(EmployeeDataService currentEmployee)
        {
            InitializeComponent();

            Title = "Storing registreren";


        }

        public ProblemDetailView(bool editMode)
        {
            InitializeComponent();

            if (editMode) // When an existing problem is clicked
            {
                cvsRegisterButtons.Visibility = Visibility.Hidden;
                cvsEditButtons.Visibility = Visibility.Visible;
                datDatumAfhandeling.IsEnabled = true;
            }
            else // When a new problem is registered
            {
                cvsRegisterButtons.Visibility = Visibility.Visible;
                cvsEditButtons.Visibility = Visibility.Hidden;
                cvsActiveProblems.Visibility = Visibility.Hidden;

                Height = 270;
            }
        }





        // Adds a device to a specific malfunction
       /* private void AddDevice(object sender, RoutedEventArgs e)
        {     
             if (!DevicesOfCurrentProblem.Any(d => d.Id == ((Device)dgAddDevices.SelectedItem).Id)) // don't allow duplicate devices 
                 DevicesOfCurrentProblem.Add((Device)dgAddDevices.SelectedItem);
        }

        // Removes the device from the malfunction
        private void RemoveDevice(object sender, RoutedEventArgs e)
        {
             DevicesOfCurrentProblem.Remove((Device)dgBetrokkenDevices.SelectedItem);
        }*/


       /* private void FilterDatagrid(object sender, TextChangedEventArgs e)
        {
            var _itemSourceList = new CollectionViewSource() { Source = AllDevices };

            // ICollectionView the View/UI part 
            ICollectionView Itemlist = _itemSourceList.View;

            // create and apply the filter
            var searchFilter = new Predicate<object>(item => ((Device)item).Name.ToLower().Contains(txtZoek.Text.ToLower()));
            Itemlist.Filter = searchFilter;

            dgAddDevices.ItemsSource = Itemlist;
        }*/


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


        private void RemoveComment(object sender, RoutedEventArgs e)
        {
            Comment selectedComment = (Comment)dgOpmerkingen.SelectedItems[0];

            problemDataService.RemoveComment(selectedComment, SelectedProblem.Id); // Delete from the database
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

                problemDataService.AddComment(newComment, SelectedProblem.Id); // Add to the database
                Comments.Add(newComment); // Add to the ObservableCollection

                txtOpmerking.Text = "";
            }
        }
    }
}
