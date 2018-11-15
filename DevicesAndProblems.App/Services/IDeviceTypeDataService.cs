using DevicesAndProblems.Model;
using System.Collections.Generic;

namespace DevicesAndProblems.App.Services
{
    public interface IDeviceTypeDataService
    {
        List<DeviceType> GetAllDeviceTypes();
        void UpdateDeviceType(DeviceType newDeviceType, int selectedDeviceTypeId);
        int AddDeviceType(DeviceType newDeviceType);
        void DeleteDeviceType(DeviceType deviceType);
        List<Device> GetDevicesOfDeviceType(int id);
    }
}
