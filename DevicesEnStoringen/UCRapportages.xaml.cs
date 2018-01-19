using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DevicesEnStoringen
{
    public partial class UCRapportages : UserControl
    {
        DatabaseConnection conn = new DatabaseConnection();
        Employee employee;

        public UCRapportages(Employee employee)
        {
            InitializeComponent();

            cboStoringJaar.ItemsSource = FillCombobox(ComboboxType.Year);

            this.employee = employee;
        }

        // Fill the combobox based on the combobox type
        public ObservableCollection<string> FillCombobox(ComboboxType type)
        {
            ObservableCollection<string> list = new ObservableCollection<string>();
            DatabaseConnection conn = new DatabaseConnection();
            conn.OpenConnection();

            if (type == ComboboxType.Year)
            {
                SQLiteDataReader dr = conn.DataReader("SELECT strftime('%Y', DatumToegevoegd) as Year FROM Storing GROUP BY Year");

                while (dr.Read())
                    list.Add(dr["Year"].ToString());
            }
            else if (type == ComboboxType.Month)
            {
                SQLiteDataReader dr = conn.DataReader("SELECT strftime('%m', DatumToegevoegd) as Month, strftime('%Y', DatumToegevoegd) AS Year FROM Storing WHERE Year = '" + cboStoringJaar.SelectedValue + "' GROUP BY Month");

                while (dr.Read())
                    list.Add(dr["Month"].ToString());
            }
            conn.CloseConnection();
            return list;
        }

        // Shows the report for either the whole year or a specific month within a year
        private void ShowReport(object sender, RoutedEventArgs e)
        {
            cboStoringMaand.IsEnabled = true;
            cboStoringMaand.ItemsSource = FillCombobox(ComboboxType.Month);

            if (cboStoringMaand.SelectedIndex == -1)
                dgStoringen.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT StoringID AS ID, Date(DatumToegevoegd) AS Datum, Prioriteit, Ernst, Beschrijving, Status FROM Storing WHERE strftime('%Y', DatumToegevoegd) = '" + cboStoringJaar.SelectedValue + "'") });
            else
                dgStoringen.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = conn.ShowDataInGridView("SELECT StoringID AS ID, Date(DatumToegevoegd) AS Datum, Prioriteit, Ernst, Beschrijving, Status FROM Storing WHERE strftime('%Y', DatumToegevoegd) = '" + cboStoringJaar.SelectedValue + "' AND strftime('%m', DatumToegevoegd) = '" + cboStoringMaand.SelectedValue + "'") });

            tbGeregistreerdeStoringen.Text = dgStoringen.Items.Count.ToString();

            int amountSolvedMalfunctions = 0;
            foreach (DataRowView row in dgStoringen.Items)
            {
                if ((string)row["Status"] == "Afgehandeld")
                    amountSolvedMalfunctions++;
            }

            tbAantalOpgelost.Text = amountSolvedMalfunctions.ToString();
            tbPercentageAantalOpgelost.Text = Math.Round(amountSolvedMalfunctions * 100.0 / dgStoringen.Items.Count, MidpointRounding.AwayFromZero).ToString();

            btnExportToTxt.IsEnabled = true;
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
                MessageBox.Show("De bijlage is verstuurd naar " + employee.EmailAddress);
            }
        }
    }
}
