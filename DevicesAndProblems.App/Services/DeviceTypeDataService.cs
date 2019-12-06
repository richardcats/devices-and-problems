using DevicesAndProblems.DAL.Interface;
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
            return repository.GetAll();
        }

        public void UpdateDeviceType(DeviceType newDeviceType, int selectedDeviceTypeId)
        {
            repository.Update(newDeviceType, selectedDeviceTypeId);
        }

        public void AddDeviceType(DeviceType newDeviceType)
        {
            repository.Add(newDeviceType);
        }

        public void DeleteDeviceType(DeviceType deviceType)
        {
            repository.Delete(deviceType);
        }
    }
}
