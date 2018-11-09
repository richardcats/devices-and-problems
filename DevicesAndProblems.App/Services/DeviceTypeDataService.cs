using DevicesAndProblems.DAL;
using DevicesAndProblems.Model;
using System.Collections.Generic;

namespace DevicesAndProblems.App.Services
{
    public class DeviceTypeDataService : IDeviceTypeDataService
    {
        IDeviceTypeRepository repository;

        public DeviceTypeDataService(IDeviceTypeRepository repository)
        {
            this.repository = repository;
        }

        public List<DeviceType> GetAllDeviceTypes()
        {
            return repository.GetDeviceTypes();
        }

        public void UpdateDeviceType(DeviceType newDeviceType, int selectedDeviceTypeId)
        {
            repository.UpdateDeviceType(newDeviceType, selectedDeviceTypeId);
        }

        public void AddDeviceType(DeviceType newDeviceType)
        {
            repository.AddDeviceType(newDeviceType);
        }

        public void DeleteDeviceType(DeviceType deviceType)
        {
            repository.DeleteDeviceType(deviceType);
        }

        public List<Device> GetDevicesOfDeviceType(int id)
        {
            return repository.GetDevicesOfDeviceType(id);
        }
    }
}
