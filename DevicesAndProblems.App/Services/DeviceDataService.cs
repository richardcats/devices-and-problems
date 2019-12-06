using DevicesAndProblems.DAL.Interface;
using DevicesAndProblems.Model;
using System.Collections.Generic;

namespace DevicesAndProblems.App.Services
{
    class DeviceDataService : IDeviceDataService
    {
        IDeviceRepository repository;

        public DeviceDataService(IDeviceRepository repository)
        {
            this.repository = repository;
        }

        public List<Device> GetAllDevices()
        {
            return repository.GetAll();
        }

        public void AddDevice(Device newDevice)
        {
            repository.Add(newDevice);
        }

        public void UpdateDevice(Device newDevice, int selectedDeviceId)
        {
            repository.Update(newDevice, selectedDeviceId);
        }

        public void DeleteDevice(Device selectedDevice)
        {
            repository.Delete(selectedDevice);
        }

        public List<Device> GetDevicesOfDeviceType(int id)
        {
            return repository.GetById(id);
        }
    }
}
