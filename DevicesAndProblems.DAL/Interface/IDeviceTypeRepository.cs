using DevicesAndProblems.Model;
using System.Collections.Generic;

namespace DevicesAndProblems.DAL.Interface
{
    public interface IDeviceTypeRepository
    {
        List<DeviceType> GetAll();
        void Update(DeviceType newDeviceType, int selectedDeviceTypeId);
        void Add(DeviceType newDeviceType);
        void Delete(DeviceType deviceType);
    }
}
