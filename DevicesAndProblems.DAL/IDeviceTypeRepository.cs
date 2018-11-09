using DevicesAndProblems.Model;
using System.Collections.Generic;

namespace DevicesAndProblems.DAL
{
    public interface IDeviceTypeRepository
    {
        List<DeviceType> GetDeviceTypes();
        void UpdateDeviceType(DeviceType newDeviceType, int selectedDeviceTypeId);
        void AddDeviceType(DeviceType newDeviceType);
        void DeleteDeviceType(DeviceType deviceType);
        List<Device> GetDevicesOfDeviceType(int id);
    }
}
