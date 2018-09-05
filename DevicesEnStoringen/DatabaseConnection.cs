using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using Model;

namespace DevicesEnStoringen
{
    class DatabaseConnection
    {
        private SQLiteConnection conn;
        private readonly string connString = ConfigurationManager.ConnectionStrings["DevicesEnStoringen"].ToString();
  
        public void OpenConnection()
        {
            conn = new SQLiteConnection(connString);
            conn.Open();
        }

        public SQLiteDataReader DataReader(string Query_)
        {
            SQLiteCommand cmd = new SQLiteCommand(Query_, conn);
            SQLiteDataReader dr = cmd.ExecuteReader();
            return dr;
        }

        public DataTable ShowDataInGridView(string Query_)
        {
            SQLiteDataAdapter dr = new SQLiteDataAdapter(Query_, connString);
            DataSet ds = new DataSet();
            dr.Fill(ds);
            DataTable dataum = ds.Tables[0];
            return dataum;
        }

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


        public List<DeviceType> GetDeviceTypes()
        {
            List<DeviceType> deviceTypes = new List<DeviceType>();

            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "SELECT DeviceType.DeviceTypeID AS ID, DeviceType.Naam AS Naam, COUNT(Device.DeviceTypeID) AS 'Aantal devices', DeviceType.Opmerkingen FROM DeviceType LEFT JOIN Device ON Device.DeviceTypeID = DeviceType.DeviceTypeID GROUP BY DeviceType.DeviceTypeID ORDER BY ID";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DeviceType deviceType = new DeviceType()
                        {
                            DeviceTypeId = Convert.ToInt32(reader["ID"]),
                            DeviceTypeName = Convert.ToString(reader["Naam"]),
                            DeviceAmount = Convert.ToInt32(reader["Aantal devices"]),
                            Description = Convert.ToString(reader["Opmerkingen"])
                        };
                        deviceTypes.Add(deviceType);
                    }
                }
            }
            return deviceTypes;
        }

        public void UpdateDeviceType(DeviceType selectedDeviceType, DeviceType newDeviceType)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "UPDATE DeviceType SET Naam = '" + newDeviceType.DeviceTypeName + "', Opmerkingen = '" + newDeviceType.Description + "' WHERE DeviceTypeID = '" + selectedDeviceType.DeviceTypeId + "'";
                SQLiteCommand command = new SQLiteCommand(query, connection);

                command.ExecuteNonQuery();
            }
        }

        public void AddDeviceType(DeviceType newDeviceType)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "INSERT INTO DeviceType (Naam, Opmerkingen) VALUES ( '" + newDeviceType.DeviceTypeName + "','" + newDeviceType.Description + "')";
                SQLiteCommand command = new SQLiteCommand(query, connection);

                command.ExecuteNonQuery();
            }
        }

        public void DeleteDeviceType(DeviceType deviceType)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "DELETE FROM DeviceType WHERE DeviceTypeID = '" + deviceType.DeviceTypeId + "'";
                SQLiteCommand command = new SQLiteCommand(query, connection);

                command.ExecuteNonQuery();
            }
        }


        public List<Device> GetDevicesOfDeviceType(int id)
        {
            List<Device> devices = new List<Device>();

            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "SELECT DeviceID AS ID, Naam, Afdeling, Date(DatumToegevoegd) AS Datum FROM Device WHERE DeviceTypeID = '" + id + "'";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Device device = new Device()
                        {
                            DeviceId = Convert.ToInt32(reader["ID"]),
                            DeviceName = Convert.ToString(reader["Naam"]),
                            Department = Convert.ToString(reader["Afdeling"]),
                            FirstAddedDate = Convert.ToDateTime(reader["Datum"])
                        };
                        devices.Add(device);
                    }
                }
            }
            return devices;
        }

        public List<Problem> GetCurrentProblemsOfDevice(int id)
        {
            List<Problem> problems = new List<Problem>();

            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "SELECT Storing.StoringID AS ID, Beschrijving, Date(DatumToegevoegd) AS Datum FROM DeviceStoring LEFT JOIN Storing ON Storing.StoringID = DeviceStoring.StoringID WHERE DeviceID = '" + id + "' AND Status = 'Open'";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Problem problem = new Problem()
                        {
                            ProblemId = Convert.ToInt32(reader["ID"]),
                            Description = Convert.ToString(reader["Beschrijving"]),
                            DateRaised = Convert.ToDateTime(reader["Datum"]),
                        };
                        problems.Add(problem);
                    }
                }
                
            }
            return problems;
        }

        public List<Device> GetDevices()
        {
            List<Device> devices = new List<Device>();

            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "SELECT Device.DeviceID AS ID, Device.Naam AS DeviceNaam, DeviceType.DeviceTypeID AS DeviceTypeValue, DeviceType.Naam AS DeviceTypeName, Serienummer, Afdeling, Device.Opmerkingen AS DeviceOpmerkingen, Date(Device.DatumToegevoegd) AS Toegevoegd, COUNT(Storing.StoringID) AS Storingen FROM Device LEFT JOIN DeviceStoring ON DeviceStoring.DeviceID = Device.DeviceID LEFT JOIN Storing ON DeviceStoring.StoringID = Storing.StoringID AND Status='Open' LEFT JOIN DeviceType ON DeviceType.DeviceTypeID = Device.DeviceTypeID GROUP BY Device.DeviceID";
                SQLiteCommand command = new SQLiteCommand(query, connection);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Device device = new Device()
                        {
                            DeviceId = Convert.ToInt32(reader["ID"]),
                            DeviceName = Convert.ToString(reader["DeviceNaam"]),
                            DeviceTypeValue = Convert.ToInt32(reader["DeviceTypeValue"]),
                            DeviceTypeName = Convert.ToString(reader["DeviceTypeName"]),
                            SerialNumber = Convert.ToString(reader["Serienummer"]),
                            Department = Convert.ToString(reader["Afdeling"]),
                            Comments = Convert.ToString(reader["DeviceOpmerkingen"]),
                            FirstAddedDate = Convert.ToDateTime(reader["Toegevoegd"]),
                            NumberOfFaults = Convert.ToInt32(reader["Storingen"])
                        };
                        devices.Add(device);
                    }
                }
                
            }
            return devices;
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
                    string query = "SELECT Afdeling FROM Device GROUP BY Afdeling";
                    SQLiteCommand command = new SQLiteCommand(query, connection);
                    SQLiteDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                        comboboxItems.Add(dr["Afdeling"].ToString());
                }
                else if (type == ComboboxType.DeviceType)
                {
                    string query = "SELECT Naam FROM DeviceType";
                    SQLiteCommand command = new SQLiteCommand(query, connection);
                    SQLiteDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                        comboboxItems.Add(dr["Naam"].ToString());
                }
                else if (type == ComboboxType.DeviceTypeAll)
                {
                    string query = "SELECT Naam FROM DeviceType";
                    SQLiteCommand command = new SQLiteCommand(query, connection);
                    SQLiteDataReader dr = command.ExecuteReader();

                    comboboxItems.Add("Alle device-types");

                    while (dr.Read())
                        comboboxItems.Add(dr["Naam"].ToString());
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

        public void AddDevice(Device newDevice)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "INSERT INTO Device (DeviceTypeID, Naam, Serienummer, Afdeling, Opmerkingen, DatumToegevoegd) VALUES ( '" + newDevice.DeviceTypeValue + "','" + newDevice.DeviceName + "','" + newDevice.SerialNumber + "','" + newDevice.Department + "','" + newDevice.Comments + "', date('now'))";
                SQLiteCommand command = new SQLiteCommand(query, connection);

                command.ExecuteNonQuery();
            }
        }

        public void UpdateDevice(Device selectedDevice, Device newDevice)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "UPDATE Device SET DeviceTypeID = '" + newDevice.DeviceTypeValue + "', Naam = '" + newDevice.DeviceName + "', Serienummer = '" + newDevice.SerialNumber + "', Afdeling = '" + newDevice.Department + "', Opmerkingen = '" + newDevice.Comments + "' WHERE DeviceID = '" + selectedDevice.DeviceId + "'";
                SQLiteCommand command = new SQLiteCommand(query, connection);

                command.ExecuteNonQuery();
            }
        }

        public void DeleteDevice(Device selectedDevice)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "DELETE FROM Device WHERE DeviceID = '" + selectedDevice.DeviceId + "'";
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
                string query = "SELECT Storing.StoringID AS ID, MedewerkerGeregistreerd, Beschrijving, Date(DatumToegevoegd) AS DatumToegevoegd, Date(DatumAfhandeling) AS DatumAfhandeling, Prioriteit, Ernst, Status, MedewerkerBehandeld, Medewerker.* FROM Storing LEFT JOIN Medewerker ON Storing.MedewerkerGeregistreerd = Medewerker.MedewerkerID";

                SQLiteCommand command = new SQLiteCommand(query, connection);
                
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Problem problem = new Problem()
                        {
                            ProblemId = Convert.ToInt32(reader["ID"]),
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
                string query = "SELECT Device.DeviceID AS ID, Naam, Serienummer FROM Device INNER JOIN DeviceStoring ON DeviceStoring.DeviceID = Device.DeviceID WHERE DeviceStoring.StoringID ='" + id + "'";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Device device = new Device()
                        {
                            DeviceId = Convert.ToInt32(reader["ID"]),
                            DeviceName = Convert.ToString(reader["Naam"]),
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
                    command.CommandText = "INSERT INTO DeviceStoring (StoringID, DeviceID) VALUES ('" + Convert.ToInt32(dr["LastID"]) + "','" + device.DeviceId + "')";
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateProblem(Problem selectedProblem, Problem newProblem, ObservableCollection<Device> DevicesOfCurrentProblem)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "UPDATE Storing SET Beschrijving = '" + newProblem.Description + "', Prioriteit = '" + newProblem.Priority + "', Ernst = '" + newProblem.Severity + "', Status = '" + newProblem.Status + "', DatumAfhandeling = '" + newProblem.ClosureDate + "', MedewerkerBehandeld = '" + newProblem.HandledBy + "' WHERE StoringID = '" + selectedProblem.ProblemId + "'";
                SQLiteCommand command = new SQLiteCommand(query, connection);

                command.ExecuteNonQuery();

                command.CommandText = "DELETE FROM DeviceStoring WHERE StoringID = '" + selectedProblem.ProblemId + "'";

                command.ExecuteNonQuery();

                foreach (Device device in DevicesOfCurrentProblem)
                {
                    command.CommandText = "INSERT INTO DeviceStoring (StoringID, DeviceID) VALUES ('" + selectedProblem.ProblemId + "','" + device.DeviceId + "')";
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteProblem(Problem selectedProblem)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "DELETE FROM DeviceStoring WHERE StoringID = '" + selectedProblem.ProblemId + "'";
                SQLiteCommand command = new SQLiteCommand(query, connection);

                command.ExecuteNonQuery();

                command.CommandText = "DELETE FROM Storing WHERE StoringID = '" + selectedProblem.ProblemId + "'";

                command.ExecuteNonQuery();
            }
        }

        public List<Comment> GetCommentsOfCurrentProblem(Problem selectedProblem)
        {
            List<Comment> comments = new List<Comment>();

            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "SELECT OpmerkingID, Date(Datum) AS Datum, Beschrijving FROM Opmerking WHERE StoringID = '" + selectedProblem.ProblemId + "'";
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
                string query = "INSERT INTO Opmerking (StoringID, Datum, Beschrijving) VALUES ('" + selectedProblem.ProblemId + "', date('now'), '" + newComment.Text + "')";
                SQLiteCommand command = new SQLiteCommand(query, connection);

                command.ExecuteNonQuery();
            }
        }

        public void RemoveComment(Comment selectedComment, Problem selectedProblem)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "DELETE FROM Opmerking WHERE Beschrijving = '" + selectedComment.Text + "' AND StoringID = '" + selectedProblem.ProblemId + "'";
                SQLiteCommand command = new SQLiteCommand(query, connection);

                command.ExecuteNonQuery();
            }
        }
    }
}
