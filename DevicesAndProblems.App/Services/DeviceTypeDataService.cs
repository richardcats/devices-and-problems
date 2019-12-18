using DevicesAndProblems.DAL.Interface;
using DevicesAndProblems.Model;
using System.Collections.Generic;

namespace DevicesAndProblems.App.Services
{
    public class DeviceTypeDataService : IDeviceTypeDataService
    {
        private IDeviceTypeRepository _repository;

        public DeviceTypeDataService(IDeviceTypeRepository repository)
        {
            _repository = repository;
        }

        public List<DeviceType> GetAllDeviceTypes()
        {
            return _repository.GetAll();
        }

        public void UpdateDeviceType(DeviceType newDeviceType, int selectedDeviceTypeId)
        {
            _repository.Update(newDeviceType, selectedDeviceTypeId);
        }

        public void AddDeviceType(DeviceType newDeviceType)
        {
            _repository.Add(newDeviceType);
        }

        public void DeleteDeviceType(DeviceType deviceType)
        {
            _repository.Delete(deviceType);
        }
    }
}
