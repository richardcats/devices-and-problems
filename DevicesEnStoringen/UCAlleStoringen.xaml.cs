﻿using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
namespace DevicesEnStoringen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class UCAlleStoringen : UserControl
    {
        DatabaseConnectie conn = new DatabaseConnectie();
        Medewerker medewerker;

        public UCAlleStoringen(Medewerker medewerker)
        {
            InitializeComponent();
            grdStoringen.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT StoringID AS ID, Beschrijving, Date(DatumToegevoegd) AS Datum, Prioriteit, Ernst, Status FROM Storing") });

            lstStatus.ItemsSource = Storing.FillCombobox(ComboboxType.StatusAll);

            this.medewerker = medewerker;

            if (medewerker.accountTypeHuidigeMedewerkerIngelogd() == "IT-manager")
                btnRegistreerStoring.IsEnabled = false;                
        }

        private void ChangeGridButtonPositionToEnd(object sender, EventArgs e)
        {
            var dgrd = sender as DataGrid;
            {
                var c = dgrd.Columns[0];
                dgrd.Columns.RemoveAt(0);

                if (medewerker.accountTypeHuidigeMedewerkerIngelogd() == "IT-beheerder")
                    dgrd.Columns.Add(c);
            }
        }

        private void RowButtonClick(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)grdStoringen.SelectedItems[0];
            Storing storing = new Storing(Convert.ToInt32(row["ID"]));
            storing.Show();
        }

        private void FilterDatagrid(object sender, EventArgs e)
        {
            if (lstStatus.SelectedIndex == 0 || lstStatus.SelectedIndex == -1)
                grdStoringen.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT StoringID AS ID, Beschrijving, Date(DatumToegevoegd) AS Datum, Prioriteit, Ernst, Status FROM Storing WHERE Beschrijving LIKE '%" + txtZoek.Text + "%'") });
            else
                grdStoringen.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT StoringID AS ID, Beschrijving, Date(DatumToegevoegd) AS Datum, Prioriteit, Ernst, Status FROM Storing WHERE Beschrijving LIKE '%" + txtZoek.Text + "%' AND Status='" + lstStatus.SelectedItem + "'") });
        }

        private void RegistreerStoringClick(object sender, RoutedEventArgs e)
        {
            Storing storing = new Storing(medewerker);
            storing.Show();
        }
    }
}
