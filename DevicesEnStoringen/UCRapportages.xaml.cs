using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public UCRapportages()
        {
            InitializeComponent();

            cboStoringJaar.ItemsSource = FillComboboxStoringYear();
            cboStoringMaand.ItemsSource = FillComboboxStoringMonth();
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
            SQLiteDataReader dr = conn.DataReader("SELECT strftime('%m', DatumToegevoegd) as Month FROM Storing WHERE strftime('%Y', DatumToegevoegd) ='" + cboStoringJaar.SelectedValue + "' GROUP BY Month");

            while (dr.Read())
                listStoringMonth.Add(dr["Month"].ToString());

            return listStoringMonth;
        }

        private void cboStoringJaar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cboStoringMaand.IsEnabled = true;
        }
    }
}
