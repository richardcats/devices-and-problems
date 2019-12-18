using DevicesAndProblems.Model;
using System.Collections.Generic;

namespace DevicesAndProblems.DAL.Interface
{
    public interface IDeviceTypeRepository
    {
        List<DeviceType> GetAll();
        void Add(DeviceType deviceType);
        void Update(DeviceType deviceType, int deviceTypeId);
        void Delete(DeviceType deviceType);
    }
}
