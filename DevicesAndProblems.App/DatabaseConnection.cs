using DevicesAndProblems.App.View;
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

        public bool CheckLoginDetails(string emailaddress, string password)
        {
            bool loginDetailsCorrect;
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "SELECT COUNT(1) FROM Medewerker WHERE Emailadres=@Emailaddress AND Wachtwoord=@Password";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.AddWithValue("@Emailaddress", emailaddress);
                command.Parameters.AddWithValue("@Password", password);
                loginDetailsCorrect = Convert.ToBoolean(command.ExecuteScalar());
            }
            return loginDetailsCorrect;
        }

        public string FirstNameOfCurrentEmployee(string emailAddress)
        {
            string firstName;
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "SELECT * FROM Medewerker WHERE Emailadres='" + emailAddress + "'";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                SQLiteDataReader dr = command.ExecuteReader();
                dr.Read();
                firstName = dr["Voornaam"].ToString();
            }
            return firstName;
        }

        public int IDOfCurrentEmployee(string emailAddress)
        {
            int id;
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "SELECT * FROM Medewerker WHERE Emailadres='" + emailAddress + "'";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                SQLiteDataReader dr = command.ExecuteReader();
                dr.Read();
                id = Convert.ToInt32(dr["MedewerkerID"]);
            }
            return id;
        }

        public string AccountTypeOfCurrentEmployee(string emailAddress)
        {
            string accountTypeName;
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();

                string query = "SELECT Naam FROM Medewerker INNER JOIN AccountType ON Medewerker.AccountTypeID = AccountType.AccountTypeID WHERE Emailadres='" + emailAddress + "'";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                SQLiteDataReader dr = command.ExecuteReader();
                dr.Read();
                accountTypeName = dr["Naam"].ToString();
            }
            return accountTypeName;
        }




        public List<Problem> GetCurrentProblemsOfDevice(int id)
        {
            List<Problem> problems = new List<Problem>();

            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "SELECT Storing.StoringID AS StoringID, Beschrijving, Date(DatumToegevoegd) AS Datum FROM DeviceStoring LEFT JOIN Storing ON Storing.StoringID = DeviceStoring.StoringID WHERE DeviceID = '" + id + "' AND Status = 'Open'";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Problem problem = new Problem()
                        {
                            Id = Convert.ToInt32(reader["StoringID"]),
                            Description = Convert.ToString(reader["Beschrijving"]),
                            DateRaised = Convert.ToDateTime(reader["Datum"]),
                        };
                        problems.Add(problem);
                    }
                }
                
            }
            return problems;
        }

       /* public List<Device> GetDevices()
        {
            List<Device> devices = new List<Device>();

            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "SELECT Device.DeviceID AS ID, Device.Name AS DeviceName, " +
                    "DeviceType.ID AS DeviceTypeValue, DeviceType.Name AS DeviceTypeName, " +
                    "SerialNumber, Department, Device.Comments AS DeviceComments, " +
                    "Date(Device.FirstAddedDate) AS FirstAddedDate, COUNT(Storing.StoringID) AS Storingen " +
                    "FROM Device " +
                    "LEFT JOIN DeviceStoring ON DeviceStoring.DeviceID = Device.DeviceID " +
                    "LEFT JOIN Storing ON DeviceStoring.StoringID = Storing.StoringID AND Status='Open' " +
                    "LEFT JOIN DeviceType ON DeviceType.ID = Device.DeviceTypeID " +
                    "GROUP BY Device.DeviceID";
                SQLiteCommand command = new SQLiteCommand(query, connection);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Device device = new Device()
                        {
                            DeviceId = Convert.ToInt32(reader["ID"]),
                            DeviceName = Convert.ToString(reader["DeviceName"]),
                            DeviceTypeValue = Convert.ToInt32(reader["DeviceTypeValue"]),
                            DeviceTypeName = Convert.ToString(reader["DeviceTypeName"]),
                            SerialNumber = Convert.ToString(reader["SerialNumber"]),
                            Department = Convert.ToString(reader["Department"]),
                            Comments = Convert.ToString(reader["DeviceComments"]),
                            FirstAddedDate = Convert.ToDateTime(reader["FirstAddedDate"]),
                            NumberOfFaults = Convert.ToInt32(reader["Storingen"])
                        };
                        devices.Add(device);
                    }
                }
                
            }
            return devices;
        }*/

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

                string query = "SELECT strftime('%Y', DatumToegevoegd) as Year FROM Storing GROUP BY Year";
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

                string query = "SELECT strftime('%m', DatumToegevoegd) as Month, strftime('%Y', DatumToegevoegd) AS Year FROM Storing WHERE Year = '" + selectedYear + "' GROUP BY Month";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                SQLiteDataReader dr = command.ExecuteReader();

                while (dr.Read())
                    comboboxItems.Add(Convert.ToInt32(dr["Month"]));
            }
            return comboboxItems;
        }

        public void AddDevice(Device newDevice)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "INSERT INTO Device (DeviceTypeID, Naam, Serienummer, Afdeling, Opmerkingen, DatumToegevoegd) VALUES ( '" + newDevice.DeviceTypeValue + "','" + newDevice.Name + "','" + newDevice.SerialNumber + "','" + newDevice.Department + "','" + newDevice.Comments + "', date('now'))";
                SQLiteCommand command = new SQLiteCommand(query, connection);

                command.ExecuteNonQuery();
            }
        }


        public void DeleteDevice(Device selectedDevice)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "DELETE FROM Device WHERE DeviceID = '" + selectedDevice.Id + "'";
                SQLiteCommand command = new SQLiteCommand(query, connection);

                command.ExecuteNonQuery();
            }
        }

        public List<Problem> GetProblems()
        {
            List<Problem> problems = new List<Problem>();

            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "SELECT Storing.StoringID AS StoringID, MedewerkerGeregistreerd, Beschrijving, Date(DatumToegevoegd) AS DatumToegevoegd, Date(DatumAfhandeling) AS DatumAfhandeling, Prioriteit, Ernst, Status, MedewerkerBehandeld, Medewerker.* FROM Storing LEFT JOIN Medewerker ON Storing.MedewerkerGeregistreerd = Medewerker.MedewerkerID";

                SQLiteCommand command = new SQLiteCommand(query, connection);
                
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Problem problem = new Problem()
                        {
                            Id = Convert.ToInt32(reader["StoringID"]),
                            Description = Convert.ToString(reader["Beschrijving"]),
                            DateRaised = Convert.ToDateTime(reader["DatumToegevoegd"]),
                            RaisedBy = Convert.ToString(reader["Voornaam"]),
                            
                            Priority = Convert.ToInt32(reader["Prioriteit"]),
                            Severity = Convert.ToInt32(reader["Ernst"]),
                            Status = Convert.ToString(reader["Status"])
                         };


                        if (!DBNull.Value.Equals(reader["MedewerkerBehandeld"]))
                            problem.HandledBy = Convert.ToInt32(reader["MedewerkerBehandeld"]) - 1;

                        if (!DBNull.Value.Equals(reader["DatumAfhandeling"]))
                            problem.ClosureDate = Convert.ToDateTime(reader["DatumAfhandeling"]).Date;

                        problems.Add(problem);
                    }
                }
            }
            return problems;
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

        public void AddProblem(Problem newProblem, ObservableCollection<Device> DevicesOfCurrentProblem)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "INSERT INTO Storing(Beschrijving, MedewerkerGeregistreerd, MedewerkerBehandeld, Prioriteit, Ernst, Status, DatumToegevoegd) VALUES('" + newProblem.Description + "', '" + newProblem.RaisedByID + "', '" + newProblem.HandledBy + "', '" + newProblem.Priority + "', '" + newProblem.Severity + "', '" + newProblem.Status + "', date('now'))";
                SQLiteCommand command = new SQLiteCommand(query, connection);

                command.ExecuteNonQuery();

                string query2 = "SELECT last_insert_rowid() AS LastID;";
                SQLiteCommand command2 = new SQLiteCommand(query2, connection);

                SQLiteDataReader dr = command2.ExecuteReader();
                dr.Read();

                foreach (Device device in DevicesOfCurrentProblem)
                {
                    command.CommandText = "INSERT INTO DeviceStoring (StoringID, DeviceID) VALUES ('" + Convert.ToInt32(dr["LastID"]) + "','" + device.Id + "')";
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateProblem(Problem selectedProblem, Problem newProblem, ObservableCollection<Device> DevicesOfCurrentProblem)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "UPDATE Storing SET Beschrijving = '" + newProblem.Description + "', Prioriteit = '" + newProblem.Priority + "', Ernst = '" + newProblem.Severity + "', Status = '" + newProblem.Status + "', DatumAfhandeling = '" + newProblem.ClosureDate + "', MedewerkerBehandeld = '" + newProblem.HandledBy + "' WHERE StoringID = '" + selectedProblem.Id + "'";
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

                command.CommandText = "DELETE FROM Storing WHERE StoringID = '" + selectedProblem.Id + "'";

                command.ExecuteNonQuery();
            }
        }

        public List<Comment> GetCommentsOfCurrentProblem(Problem selectedProblem)
        {
            List<Comment> comments = new List<Comment>();

            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "SELECT OpmerkingID, Date(Datum) AS Datum, Beschrijving FROM Opmerking WHERE StoringID = '" + selectedProblem.Id + "'";
                SQLiteCommand command = new SQLiteCommand(query, connection);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Comment comment = new Comment()
                        {
                            CommentID = Convert.ToInt32(reader["OpmerkingID"]),
                            Date = Convert.ToDateTime(reader["Datum"]),
                            Text = Convert.ToString(reader["Beschrijving"])
                        };
                        comments.Add(comment);
                    }
                }
            }

            return comments;
        }

        public void AddComment(Problem selectedProblem, Comment newComment)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "INSERT INTO Opmerking (StoringID, Datum, Beschrijving) VALUES ('" + selectedProblem.Id + "', date('now'), '" + newComment.Text + "')";
                SQLiteCommand command = new SQLiteCommand(query, connection);

                command.ExecuteNonQuery();
            }
        }

        public void RemoveComment(Comment selectedComment, Problem selectedProblem)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "DELETE FROM Opmerking WHERE Beschrijving = '" + selectedComment.Text + "' AND StoringID = '" + selectedProblem.Id + "'";
                SQLiteCommand command = new SQLiteCommand(query, connection);

                command.ExecuteNonQuery();
            }
        }
    }
}
