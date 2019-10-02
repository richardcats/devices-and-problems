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
            return repository.SelectList();
        }

        public void AddDevice(Device newDevice)
        {
           // newDevice.DeviceTypeId++; //TODO: this binds the ComboBox index to DeviceType ID. Find a better solution.
            repository.Insert(newDevice);
        }

        public void UpdateDevice(Device newDevice, int selectedDeviceId)
        {
          //  newDevice.DeviceTypeId++; //TODO: this binds the ComboBox index to DeviceType ID. Find a better solution.
            repository.Update(newDevice, selectedDeviceId);
        }

        public void DeleteDevice(Device selectedDevice)
        {
            repository.Delete(selectedDevice);
        }

        public List<Device> GetDevicesOfDeviceType(int id)
        {
            return repository.SelectListById(id);
        }
    }
}
