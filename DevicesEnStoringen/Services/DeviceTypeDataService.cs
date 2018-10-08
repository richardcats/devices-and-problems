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
            return conn.GetDeviceTypes();
        }

        public void UpdateDeviceType(DeviceType newDeviceType, int selectedDeviceTypeId)
        {
            conn.UpdateDeviceType(newDeviceType, selectedDeviceTypeId);
        }

        public void AddDeviceType(DeviceType newDeviceType)
        {
            conn.AddDeviceType(newDeviceType);
        }

        public void DeleteDeviceType(DeviceType deviceType)
        {
            conn.DeleteDeviceType(deviceType);
        }

        public List<Device> GetDevicesOfDeviceType(int id)
        {
            return conn.GetDevicesOfDeviceType(id);
        }
    }
}
