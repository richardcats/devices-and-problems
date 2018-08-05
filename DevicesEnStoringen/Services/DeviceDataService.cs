using Model;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesEnStoringen.Services
{
    class DeviceDataService
    {
        DatabaseConnection conn = new DatabaseConnection();

        public List<Device> GetAllDevices()
        {
            conn.OpenConnection();
            return conn.GetDevices();
        }

        public void AddDevice(Device newDevice)
        {
            conn.OpenConnection();
            conn.AddDevice(newDevice);
        }

        public void UpdateDevice(Device selectedDevice, Device newDevice)
        {
            conn.OpenConnection();
            conn.UpdateDevice(selectedDevice, newDevice);
        }

        public void DeleteDeviceType(Device selectedDevice)
        {
            conn.OpenConnection();
            conn.DeleteDevice(selectedDevice);
        }
    }
}
