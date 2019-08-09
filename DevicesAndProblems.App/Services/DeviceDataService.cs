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

        public void UpdateDevice(Device newDevice, int selectedDeviceId)
        {
            conn.UpdateDevice(newDevice, selectedDeviceId);
        }

        public void DeleteDevice(Device selectedDevice)
        {
            conn.DeleteDevice(selectedDevice);
        }

        public List<Device> GetDevicesOfDeviceType(int id)
        {
            return repository.SelectListById(id);
        }
    }
}
