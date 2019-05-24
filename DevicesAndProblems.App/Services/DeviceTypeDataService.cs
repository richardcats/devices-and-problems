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
            return repository.SelectList();
        }

        public void UpdateDeviceType(DeviceType newDeviceType, int selectedDeviceTypeId)
        {
            repository.Update(newDeviceType, selectedDeviceTypeId);
        }

        public void AddDeviceType(DeviceType newDeviceType)
        {
            repository.Insert(newDeviceType);
        }

        public void DeleteDeviceType(DeviceType deviceType)
        {
            repository.Delete(deviceType);
        }
    }
}
