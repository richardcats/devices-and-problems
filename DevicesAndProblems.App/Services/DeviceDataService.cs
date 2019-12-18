using DevicesAndProblems.DAL.Interface;
using DevicesAndProblems.Model;
using System.Collections.Generic;

namespace DevicesAndProblems.App.Services
{
    class DeviceDataService : IDeviceDataService
    {
        private IDeviceRepository _repository;

        public DeviceDataService(IDeviceRepository repository)
        {
            _repository = repository;
        }

        public List<Device> GetAllDevices()
        {
            return _repository.GetAll();
        }

        public void AddDevice(Device newDevice)
        {
            _repository.Add(newDevice);
        }

        public void UpdateDevice(Device newDevice, int selectedDeviceId)
        {
            _repository.Update(newDevice, selectedDeviceId);
        }

        public void DeleteDevice(Device selectedDevice)
        {
            _repository.Delete(selectedDevice);
        }

        public List<Device> GetDevicesOfDeviceType(int deviceTypeId)
        {
            return _repository.GetByDeviceTypeId(deviceTypeId);
        }
    }
}
