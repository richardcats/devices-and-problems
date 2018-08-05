using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesEnStoringen.Services
{
    class DeviceTypeDataService
    {

        DatabaseConnection conn = new DatabaseConnection();

        public List<DeviceType> GetAllDeviceTypes()
        {
            conn.OpenConnection();
            return conn.GetDeviceTypes();
        }

        public void UpdateDeviceType(DeviceType selectedDeviceType, DeviceType newDeviceType)
        {
            conn.OpenConnection();
            conn.UpdateDeviceType(selectedDeviceType, newDeviceType);
        }

        public void AddDeviceType(DeviceType newDeviceType)
        {
            conn.OpenConnection();
            conn.AddDeviceType(newDeviceType);
        }

        public void DeleteDeviceType(DeviceType deviceType)
        {
            conn.OpenConnection();
            conn.DeleteDeviceType(deviceType);
        }

        public List<Device> GetDevicesOfDeviceType(int id)
        {
            conn.OpenConnection();
            return conn.GetDevicesOfDeviceType(id);
        }
    }
}
