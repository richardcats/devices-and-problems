using DevicesAndProblems.Model;
using System.Collections.Generic;

namespace DevicesAndProblems.App.Services
{
    class DeviceDataService
    {
        DatabaseConnection conn = new DatabaseConnection();

        public List<Device> GetAllDevices()
        {
            return conn.GetDevices();
        }

        public void AddDevice(Device newDevice)
        {
            conn.AddDevice(newDevice);
        }

        public void UpdateDevice(Device selectedDevice, Device newDevice)
        {
            conn.UpdateDevice(selectedDevice, newDevice);
        }

        public void DeleteDeviceType(Device selectedDevice)
        {
            conn.DeleteDevice(selectedDevice);
        }
    }
}
