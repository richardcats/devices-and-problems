using DevicesEnStoringen.Extensions;
using DevicesEnStoringen.Services;
using Microsoft.Win32;
using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DevicesEnStoringen
{
    public partial class ReportsView : UserControl, INotifyPropertyChanged
    {
        private ProblemDataService problemDataService = new ProblemDataService();
        private EmployeeDataService currentEmployee;

        public ObservableCollection<int> SelectableYears { get; set; }
        public ObservableCollection<Problem> Problems { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private int _amountProblems;
        public int AmountProblems
        {
            get { return _amountProblems; }
            set { _amountProblems = value; OnPropertyChanged("AmountProblems"); }
        }

        private int _amountSolvedProblems;
        public int AmountSolvedProblems {
            get { return _amountSolvedProblems; }
            set { _amountSolvedProblems = value; OnPropertyChanged("AmountSolvedProblems"); }
        }

        private int _percentageAmountSolvedProblems;
        public int PercentageAmountSolvedProblems
        {
            get { return _percentageAmountSolvedProblems; }
            set { _percentageAmountSolvedProblems = value; OnPropertyChanged("PercentageAmountSolvedProblems"); }
        }
        public ReportsView(EmployeeDataService currentEmployee)
        {
            InitializeComponent();

            this.currentEmployee = currentEmployee;
            Problems = problemDataService.GetAllProblems().ToObservableCollection();
            SelectableYears = problemDataService.FillComboboxYears().ToObservableCollection();
            Loaded += ReportsView_Loaded;
        }

        void ReportsView_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = this;
        }

        protected virtual void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        // Shows the report for either the whole year or a specific month within a year
        private void ShowReport(object sender, RoutedEventArgs e)
        {
            btnExport.IsEnabled = true;
            cboStoringMaand.IsEnabled = true;
            cboStoringMaand.ItemsSource = problemDataService.FillComboboxMonthsBasedOnYear((Convert.ToInt32(cboStoringJaar.SelectedValue)));

            var _itemSourceList = new CollectionViewSource() { Source = Problems };

            // ICollectionView the View/UI part 
            ICollectionView Itemlist = _itemSourceList.View;
            Predicate<object> searchFilter;

            if (cboStoringMaand.SelectedIndex == -1)
            {
                searchFilter = new Predicate<object>(item => ((Problem)item).DateRaised.Year == (int)cboStoringJaar.SelectedValue);
                Itemlist.Filter = searchFilter;
            }
            else
            {
                searchFilter = new Predicate<object>(item => ((Problem)item).DateRaised.Year == (int)cboStoringJaar.SelectedValue && ((Problem)item).DateRaised.Month == (int)cboStoringMaand.SelectedValue);
                Itemlist.Filter = searchFilter;
            }

            dgStoringen.ItemsSource = Itemlist;

            AmountSolvedProblems = 0;
            AmountProblems = 0;

            foreach (Problem problem in Itemlist)
            {
                AmountProblems++;

                if (problem.Status == "Afgehandeld")
                    AmountSolvedProblems++;
            }

            PercentageAmountSolvedProblems = (int)Math.Round(AmountSolvedProblems * 100.0 / dgStoringen.Items.Count, MidpointRounding.AwayFromZero);
        }

        public IEnumerable<DataGridRow> GetDataGridRows(DataGrid grid)
        {
            var itemsSource = grid.ItemsSource as IEnumerable;
            if (null == itemsSource) yield return null;
            foreach (var item in itemsSource)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (null != row) yield return row;
            }
        }

        private void ExportToTxtClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlgSave = new SaveFileDialog();
            dlgSave.Filter = "Text files (*.txt)|*.txt";
            bool? result = dlgSave.ShowDialog();

            if (result == true)
            {
                TextWriter writer = new StreamWriter(dlgSave.FileName);
                
                writer.Write("Rapportage " + cboStoringJaar.SelectedValue); 
                if (cboStoringMaand.SelectedIndex != -1) writer.Write(" - " + cboStoringMaand.SelectedValue);

                writer.WriteLine(Environment.NewLine);

                writer.WriteLine("Totaal aantal geregistreerde storingen: " + tbGeregistreerdeStoringen.Text);
                writer.WriteLine("Totaal aantal opgeloste storingen: " + tbAantalOpgelost.Text);
                writer.WriteLine("Percentage opgeloste storingen: " + tbPercentageAantalOpgelost.Text + "%" + Environment.NewLine);

                writer.WriteLine("----------------------------------------------------------------------------" + Environment.NewLine);

                var rows = GetDataGridRows(dgStoringen);

                foreach (DataGridRow row in rows)
                {
                    foreach (DataGridColumn column in dgStoringen.Columns)
                    {
                        TextBlock cellContent = column.GetCellContent(row) as TextBlock;
                        writer.Write(cellContent.Text + " | ");
                    }
                    writer.WriteLine("");
                }

                writer.Close();
                MessageBox.Show("De gegevens zijn geëxporteerd");
            }
        }

        private void SendMailClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlgOpen = new OpenFileDialog();
            dlgOpen.Filter = "Text files (*.txt)|*.txt";
            bool? result = dlgOpen.ShowDialog();

            // this function is temporarily disabled
            if (result == true)
            {
                /*string emailadress = medewerker.Emailadres;
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress(emailadress);
                mail.To.Add(emailadress);
                mail.Subject = "Rapportage storingen";
                mail.Body = "Verzonden vanuit de applicatie: devices en storingen";

                Attachment attachment;
                attachment = new Attachment(dlgOpen.FileName);
                mail.Attachments.Add(attachment);

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new NetworkCredential(emailadress, "password");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                */
                MessageBox.Show("De bijlage is verstuurd naar " + currentEmployee.EmailAddress);
            }
        }
    }
}
