using DevicesAndProblems.DAL.Interface;
using DevicesAndProblems.Model;
using System.Collections.Generic;

namespace DevicesAndProblems.App.Services
{
    class DeviceDataService : IDeviceDataService
    {
        DatabaseConnection conn = new DatabaseConnection();
        IDeviceRepository repository;

        public DeviceDataService(IDeviceRepository repository)
        {
            this.repository = repository;
        }

        public List<Device> GetAllDevices()
        {
            return repository.SelectList();
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
