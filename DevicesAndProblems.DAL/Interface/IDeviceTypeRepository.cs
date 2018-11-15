using DevicesAndProblems.Model;
using System.Collections.Generic;

namespace DevicesAndProblems.DAL.Interface
{
    public interface IDeviceTypeRepository
    {
        List<DeviceType> SelectList();
        void Update(DeviceType newDeviceType, int selectedDeviceTypeId);
        int Insert(DeviceType newDeviceType);
        void Delete(DeviceType deviceType);
    }
}
