using DevicesEnStoringen.Extensions;
using DevicesEnStoringen.Services;
using Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DevicesEnStoringen
{
    public partial class ReportsView : UserControl
    {
        DatabaseConnection conn = new DatabaseConnection();
        private ProblemDataService problemDataService = new ProblemDataService();
        private EmployeeDataService currentEmployee;

        public ObservableCollection<int> SelectableYears { get; set; }
        public ObservableCollection<Problem> Problems { get; set; }
        public int AmountSolvedProblems { get; set; }

        public ReportsView(EmployeeDataService currentEmployee)
        {
            InitializeComponent();

            Problems = problemDataService.GetAllProblems().ToObservableCollection();
            SelectableYears = problemDataService.FillComboboxYears().ToObservableCollection();
            this.currentEmployee = currentEmployee;


            Loaded += ReportsView_Loaded;
            ShowExtraDatagridInformation();
        }

        void ReportsView_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = this;
        }

        // Shows the report for either the whole year or a specific month within a year
        private void ShowReport(object sender, RoutedEventArgs e)
        {
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

            ShowExtraDatagridInformation();

            btnExportToTxt.IsEnabled = true;
        }

        public void ShowExtraDatagridInformation()
        {

            AmountSolvedProblems = 0;

            foreach (Problem problem in Problems)
            {
                if (problem.Status == "Afgehandeld")
                    AmountSolvedProblems++;
            }


             tbAantalOpgelost.Text = AmountSolvedProblems.ToString();
            // tbPercentageAantalOpgelost.Text = Math.Round(amountSolvedMalfunctions * 100.0 / dgStoringen.Items.Count, MidpointRounding.AwayFromZero).ToString();
        }

        private void btnExportToTxt_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlgSave = new Microsoft.Win32.SaveFileDialog();
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

                foreach (DataRowView row in dgStoringen.Items)
                {
                    for (int i = 0; i < dgStoringen.Columns.Count; i++)
                    {
                        writer.Write(row[i] + " | ");
                    }
                    writer.WriteLine("");
                }

                writer.Close();
                MessageBox.Show("De gegevens zijn geëxporteerd");
            }
        }

        private void SendMail(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlgOpen = new Microsoft.Win32.OpenFileDialog();
            dlgOpen.Filter = "Text files (*.txt)|*.txt";
            bool? result = dlgOpen.ShowDialog();

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
