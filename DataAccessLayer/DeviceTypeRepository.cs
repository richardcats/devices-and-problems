using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Configuration;

namespace DataAccessLayer
{
    public class DeviceTypeRepository : IDeviceTypeRepository
    {
        private string connString = "Data Source = C:\\Projects\\devices-en-storingen\\DevicesEnStoringen\\Data\\DevicesEnStoringen.sqlite;Version=3";
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

        public void UpdateDeviceType(DeviceType newDeviceType, int selectedDeviceTypeId)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                string query = "UPDATE DeviceType SET Naam = '" + newDeviceType.DeviceTypeName + "', Opmerkingen = '" + newDeviceType.Description + "' WHERE DeviceTypeID = '" + selectedDeviceTypeId + "'";
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

    }
}
