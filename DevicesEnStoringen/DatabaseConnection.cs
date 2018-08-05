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
        public void CloseConnection()
        {
            conn.Close();
        }
        public void ExecuteQueries(string Query_)
        {
            SQLiteCommand cmd = new SQLiteCommand(Query_, conn);
            cmd.ExecuteNonQuery();
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

        public List<DeviceType> GetDeviceTypes()
        {
            List<DeviceType> columnData = new List<DeviceType>();

            using (conn)
            {
                //conn.Open();
                string query = "SELECT DeviceType.DeviceTypeID AS ID, DeviceType.Naam AS Naam, COUNT(Device.DeviceTypeID) AS 'Aantal devices', DeviceType.Opmerkingen FROM DeviceType LEFT JOIN Device ON Device.DeviceTypeID = DeviceType.DeviceTypeID GROUP BY DeviceType.DeviceTypeID ORDER BY ID";
                using (SQLiteCommand command = new SQLiteCommand(query, conn))
                {
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
                            columnData.Add(deviceType);
                        }
                    }
                }
            }

            return columnData;
        }

        public void UpdateDeviceType(DeviceType selectedDeviceType, DeviceType newDeviceType)
        {
            using (conn)
            {
                ExecuteQueries("UPDATE DeviceType SET Naam = '" + newDeviceType.DeviceTypeName + "', Opmerkingen = '" + newDeviceType.Description + "' WHERE DeviceTypeID = '" + selectedDeviceType.DeviceTypeId + "'");
            }
        }

        public void AddDeviceType(DeviceType newDeviceType)
        {
            using (conn)
            {
                ExecuteQueries("INSERT INTO DeviceType (Naam, Opmerkingen) VALUES ( '" + newDeviceType.DeviceTypeName + "','" + newDeviceType.Description + "')");
            }
        }

        public void DeleteDeviceType(DeviceType deviceType)
        {
            using (conn)
            {
                ExecuteQueries("DELETE FROM DeviceType WHERE DeviceTypeID = '" + deviceType.DeviceTypeId + "'");
            }
        }


        public List<Device> GetDevicesOfDeviceType(int id)
        {
            List<Device> columnData = new List<Device>();

            using (conn)
            {
                //conn.Open();
                string query = "SELECT DeviceID AS ID, Naam, Afdeling, Date(DatumToegevoegd) AS Datum FROM Device WHERE DeviceTypeID = '" + id + "'";
                using (SQLiteCommand command = new SQLiteCommand(query, conn))
                {
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
                            columnData.Add(device);
                        }
                    }
                }
            }

            return columnData;
        }

        public List<Problem> GetCurrentProblemsOfDevice(int id)
        {
            List<Problem> columnData = new List<Problem>();

            using (conn)
            {
                //conn.Open();
                string query = "SELECT Storing.StoringID AS ID, Beschrijving, Date(DatumToegevoegd) AS Datum FROM DeviceStoring LEFT JOIN Storing ON Storing.StoringID = DeviceStoring.StoringID WHERE DeviceID = '" + id + "' AND Status = 'Open'";
                using (SQLiteCommand command = new SQLiteCommand(query, conn))
                {
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
                            columnData.Add(problem);
                        }
                    }
                }
            }

            return columnData;
        }

        public List<Device> GetDevices()
        {
            List<Device> columnData = new List<Device>();

            using (conn)
            {
                //conn.Open();
                string query = "SELECT Device.DeviceID AS ID, Device.Naam AS DeviceNaam, DeviceType.DeviceTypeID AS DeviceTypeValue, DeviceType.Naam AS DeviceTypeName, Serienummer, Afdeling, Device.Opmerkingen AS DeviceOpmerkingen, Date(Device.DatumToegevoegd) AS Toegevoegd, COUNT(Storing.StoringID) AS Storingen FROM Device LEFT JOIN DeviceStoring ON DeviceStoring.DeviceID = Device.DeviceID LEFT JOIN Storing ON DeviceStoring.StoringID = Storing.StoringID AND Status='Open' LEFT JOIN DeviceType ON DeviceType.DeviceTypeID = Device.DeviceTypeID GROUP BY Device.DeviceID";
                using (SQLiteCommand command = new SQLiteCommand(query, conn))
                {
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
                            columnData.Add(device);
                        }
                    }
                }
                
            }

            return columnData;
        }

        // Fill the combobox based on the combobox type 
        public static ObservableCollection<string> FillCombobox(ComboboxType type)
        {
            ObservableCollection<string> list = new ObservableCollection<string>();
            DatabaseConnection conn = new DatabaseConnection();
            conn.OpenConnection();

            if (type == ComboboxType.Afdeling)
            {
                SQLiteDataReader dr = conn.DataReader("SELECT Afdeling FROM Device GROUP BY Afdeling");

                while (dr.Read())
                    list.Add(dr["Afdeling"].ToString());
            }
            else if (type == ComboboxType.DeviceType)
            {
                SQLiteDataReader dr = conn.DataReader("SELECT Naam FROM DeviceType");

                while (dr.Read())
                    list.Add(dr["Naam"].ToString());
            }
            else if (type == ComboboxType.DeviceTypeAll)
            {
                SQLiteDataReader dr = conn.DataReader("SELECT Naam FROM DeviceType");
                list.Add("Alle device-types");
                while (dr.Read())
                    list.Add(dr["Naam"].ToString());
            }
            conn.CloseConnection();
            return list;
        }

        public void AddDevice(Device newDevice)
        {
            using (conn)
            {
                ExecuteQueries("INSERT INTO Device (DeviceTypeID, Naam, Serienummer, Afdeling, Opmerkingen, DatumToegevoegd) VALUES ( '" + newDevice.DeviceTypeValue + "','" + newDevice.DeviceName + "','" + newDevice.SerialNumber + "','" + newDevice.Department + "','" + newDevice.Comments + "', date('now'))");
            }
        }

        public void UpdateDevice(Device selectedDevice, Device newDevice)
        {
            using (conn)
            {
                ExecuteQueries("UPDATE Device SET DeviceTypeID = '" + newDevice.DeviceTypeValue + "', Naam = '" + newDevice.DeviceName + "', Serienummer = '" + newDevice.SerialNumber + "', Afdeling = '" + newDevice.Department + "', Opmerkingen = '" + newDevice.Comments + "' WHERE DeviceID = '" + selectedDevice.DeviceId + "'");
            }
        }

        public void DeleteDevice(Device selectedDevice)
        {
            using (conn)
            {
                ExecuteQueries("DELETE FROM Device WHERE DeviceID = '" + selectedDevice.DeviceId + "'");
            }
        }

        public List<Problem> GetProblems()
        {
            List<Problem> columnData = new List<Problem>();

            using (conn)
            {
                //conn.Open();
                string query = "SELECT StoringID AS ID, Beschrijving, Date(DatumToegevoegd) AS Datum, Prioriteit, Ernst, Status FROM Storing";
                using (SQLiteCommand command = new SQLiteCommand(query, conn))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Problem problem = new Problem()
                            {
                                ProblemId = Convert.ToInt32(reader["ID"]),
                                Description = Convert.ToString(reader["Beschrijving"]),
                                DateRaised = Convert.ToDateTime(reader["Datum"]),
                                Priority = Convert.ToInt32(reader["Prioriteit"]),
                                Severity = Convert.ToInt32(reader["Ernst"]),
                                Status = Convert.ToString(reader["Status"])
                            };
                            columnData.Add(problem);
                        }
                    }
                }
            }

            return columnData;
        }

        public SQLiteCommand ReturnSQLiteCommand(string Query_)
        {
            SQLiteCommand sqlCmd = new SQLiteCommand(Query_, conn);
            return sqlCmd;
        }
    }
}
