using DevicesAndProblems.DAL.SQLite;
using DevicesAndProblems.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SQLite;

namespace DevicesAndProblems.App
{
    // temporary - remove this class!
    class DatabaseConnection
    {
        private readonly string connString = ConfigurationManager.ConnectionStrings["Default"].ToString();


        public List<Problem> GetCurrentProblemsOfDevice(int id)
        {
            List<Problem> problems = new List<Problem>();

            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "SELECT Storing.Id AS Id, Description, Date(DateRaised) AS Datum FROM DeviceStoring LEFT JOIN Storing ON Storing.Id = DeviceStoring.StoringID WHERE DeviceID = '" + id + "' AND Status = 'Open'";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Problem problem = new Problem()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Description = Convert.ToString(reader["Description"]),
                            DateRaised = Convert.ToDateTime(reader["Datum"]),
                        };
                        problems.Add(problem);
                    }
                }
                
            }
            return problems;
        }


        // Fill the combobox based on the combobox type 
        public ObservableCollection<string> FillCombobox(ComboboxType type)
        {
            ObservableCollection<string> comboboxItems = new ObservableCollection<string>();
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();

                if (type == ComboboxType.Afdeling)
                {
                    string query = "SELECT Department FROM Device GROUP BY Department";
                    SQLiteCommand command = new SQLiteCommand(query, connection);
                    SQLiteDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                        comboboxItems.Add(dr["Department"].ToString());
                }
                else if (type == ComboboxType.DeviceType)
                {
                    string query = "SELECT Name FROM DeviceType";
                    SQLiteCommand command = new SQLiteCommand(query, connection);
                    SQLiteDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                        comboboxItems.Add(dr["Name"].ToString());
                }
                else if (type == ComboboxType.DeviceTypeAll)
                {
                    string query = "SELECT Name FROM DeviceType";
                    SQLiteCommand command = new SQLiteCommand(query, connection);
                    SQLiteDataReader dr = command.ExecuteReader();

                    comboboxItems.Add("Alle device-types");

                    while (dr.Read())
                        comboboxItems.Add(dr["Name"].ToString());
                }
                else if (type == ComboboxType.Medewerker)
                {
                    string query = "SELECT Voornaam FROM Medewerker";
                    SQLiteCommand command = new SQLiteCommand(query, connection);
                    SQLiteDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                        comboboxItems.Add(dr["Voornaam"].ToString());
                }

            }
            return comboboxItems;
        }

        public ObservableCollection<int> FillComboboxYears()
        {
            ObservableCollection<int> comboboxItems = new ObservableCollection<int>();
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();

                string query = "SELECT strftime('%Y', DateRaised) as Year FROM Storing GROUP BY Year";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                SQLiteDataReader dr = command.ExecuteReader();

                while (dr.Read())
                    comboboxItems.Add(Convert.ToInt32(dr["Year"]));
            }
            return comboboxItems;
        }

        public ObservableCollection<int> FillComboboxMonthsBasedOnYear(int selectedYear)
        {
            ObservableCollection<int> comboboxItems = new ObservableCollection<int>();
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();

                string query = "SELECT strftime('%m', DateRaised) as Month, strftime('%Y', DateRaised) AS Year FROM Storing WHERE Year = '" + selectedYear + "' GROUP BY Month";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                SQLiteDataReader dr = command.ExecuteReader();

                while (dr.Read())
                    comboboxItems.Add(Convert.ToInt32(dr["Month"]));
            }
            return comboboxItems;
        }



        public List<Device> GetDevicesOfCurrentProblem(int id)
        {
            List<Device> devices = new List<Device>();

            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "SELECT Device.Id AS DeviceId, Naam, Serienummer FROM Device INNER JOIN DeviceStoring ON DeviceStoring.DeviceID = Device.Id WHERE DeviceStoring.StoringID ='" + id + "'";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Device device = new Device()
                        {
                            Id = Convert.ToInt32(reader["DeviceId"]),
                            Name = Convert.ToString(reader["Naam"]),
                            SerialNumber = Convert.ToString(reader["Serienummer"])
                        };
                        devices.Add(device);
                    }
                }
            }
            return devices;
        }


        public void UpdateProblem(Problem selectedProblem, Problem newProblem, ObservableCollection<Device> DevicesOfCurrentProblem)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "UPDATE Storing SET Description = '" + newProblem.Description + "', Priority = '" + newProblem.Priority + "', Severity = '" + newProblem.Severity + "', Status = '" + newProblem.Status + "', ClosureDate = '" + newProblem.ClosureDate + "', HandledByEmployeeId = '" + newProblem.HandledByEmployeeId + "' WHERE Id = '" + selectedProblem.Id + "'";
                SQLiteCommand command = new SQLiteCommand(query, connection);

                command.ExecuteNonQuery();

                command.CommandText = "DELETE FROM DeviceStoring WHERE StoringID = '" + selectedProblem.Id + "'";

                command.ExecuteNonQuery();

                foreach (Device device in DevicesOfCurrentProblem)
                {
                    command.CommandText = "INSERT INTO DeviceStoring (StoringID, DeviceID) VALUES ('" + selectedProblem.Id + "','" + device.Id + "')";
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteProblem(Problem selectedProblem)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "DELETE FROM DeviceStoring WHERE StoringID = '" + selectedProblem.Id + "'";
                SQLiteCommand command = new SQLiteCommand(query, connection);

                command.ExecuteNonQuery();

                command.CommandText = "DELETE FROM Storing WHERE Id = '" + selectedProblem.Id + "'";

                command.ExecuteNonQuery();
            }
        }
    }
}
