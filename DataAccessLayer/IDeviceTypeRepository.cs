using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
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
