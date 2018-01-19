using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
namespace DevicesEnStoringen
{
    public partial class UCAlleStoringen : UserControl
    {
        DatabaseConnection conn = new DatabaseConnection();
        Employee employee;

        public UCAlleStoringen(Employee employee)
        {
            InitializeComponent();
            
            dgStoringen.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT StoringID AS ID, Beschrijving, Date(DatumToegevoegd) AS Datum, Prioriteit, Ernst, Status FROM Storing") });

            cboStatus.ItemsSource = Storing.FillCombobox(ComboboxType.StatusAll);

            this.employee = employee;

            if (employee.AccountTypeOfCurrentEmployee() == "IT-manager")
                btnRegistreerStoring.Visibility = Visibility.Hidden;              
        }

        // Ensures that the manage device button is placed at the end of the datagrid
        private void ChangeGridButtonPositionToEnd(object sender, EventArgs e)
        {
            var dgrd = sender as DataGrid;
            {
                var c = dgrd.Columns[0];
                dgrd.Columns.RemoveAt(0);

                if (employee.AccountTypeOfCurrentEmployee() == "IT-beheerder")
                    dgrd.Columns.Add(c);
            }
        }

        // When the IT administrator clicks on a malfunction, it will pass the ID to a new window
        private void RowButtonClick(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)dgStoringen.SelectedItems[0];
            Storing storing = new Storing(Convert.ToInt32(row["ID"]));
            storing.Show();
        }

        // Filters the datagrid based on a textbox and a combobox
        private void FilterDatagrid(object sender, EventArgs e)
        {
            if (cboStatus.SelectedIndex == 0 || cboStatus.SelectedIndex == -1)
                dgStoringen.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT StoringID AS ID, Beschrijving, Date(DatumToegevoegd) AS Datum, Prioriteit, Ernst, Status FROM Storing WHERE Beschrijving LIKE '%" + txtZoek.Text + "%'") });
            else
                dgStoringen.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT StoringID AS ID, Beschrijving, Date(DatumToegevoegd) AS Datum, Prioriteit, Ernst, Status FROM Storing WHERE Beschrijving LIKE '%" + txtZoek.Text + "%' AND Status='" + cboStatus.SelectedItem + "'") });
        }

        private void RegistreerStoringClick(object sender, RoutedEventArgs e)
        {
            Storing storing = new Storing(employee);
            storing.Show();
        }
    }
}
