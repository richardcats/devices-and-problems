using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DevicesEnStoringen
{
    /// <summary>
    /// Interaction logic for UCRapportages.xaml
    /// </summary>
    public partial class UCRapportages : UserControl
    {
        public static ObservableCollection<string> listStoringYear;
        public static ObservableCollection<string> listStoringMonth;
        DatabaseConnectie conn = new DatabaseConnectie();

        public UCRapportages()
        {
            InitializeComponent();

            cboStoringJaar.ItemsSource = FillComboboxStoringYear();
        }

        public ObservableCollection<string> FillComboboxStoringYear()
        {
            listStoringYear = new ObservableCollection<string>();
            DatabaseConnectie conn = new DatabaseConnectie();
            conn.OpenConnection();
            SQLiteDataReader dr = conn.DataReader("SELECT strftime('%Y', DatumToegevoegd) as Year FROM Storing GROUP BY Year");

            while (dr.Read())
                listStoringYear.Add(dr["Year"].ToString());

            return listStoringYear;
        }

        public ObservableCollection<string> FillComboboxStoringMonth()
        {
            listStoringMonth = new ObservableCollection<string>();
            DatabaseConnectie conn = new DatabaseConnectie();
            conn.OpenConnection();
            SQLiteDataReader dr = conn.DataReader("SELECT strftime('%m', DatumToegevoegd) as Month, strftime('%Y', DatumToegevoegd) AS Year FROM Storing WHERE Year = '" + cboStoringJaar.SelectedValue + "' GROUP BY Month");

            while (dr.Read())
                listStoringMonth.Add(dr["Month"].ToString());

            return listStoringMonth;
        }

        private void cboStoringJaar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cboStoringMaand.IsEnabled = true;
            cboStoringMaand.ItemsSource = FillComboboxStoringMonth();
        }

        private void ShowStoringRapportage(object sender, RoutedEventArgs e)
        {
            dgStoringen.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT StoringID AS ID, Beschrijving, Date(DatumToegevoegd) AS Datum, Prioriteit, Ernst, Status FROM Storing WHERE strftime('%Y', DatumToegevoegd) = '" + cboStoringJaar.SelectedValue + "' AND strftime('%m', DatumToegevoegd) = '" + cboStoringMaand.SelectedValue + "'") });

            tbGeregistreerdeStoringen.Text = dgStoringen.Items.Count.ToString();

            int amountSolvedMalfunctions = 0;
            foreach (DataRowView row in dgStoringen.Items)
            {
                if ((string)row["Status"] == "Afgehandeld")
                    amountSolvedMalfunctions++;
            }

            tbWeergaveToelichting2.Text = amountSolvedMalfunctions.ToString();
            tbWeergaveToelichting3.Text = Math.Round(amountSolvedMalfunctions * 100.0 / dgStoringen.Items.Count, MidpointRounding.AwayFromZero).ToString();
        }

        private void cboStoringMaand_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnToonRapportage.IsEnabled = true;
        }
    }
}
