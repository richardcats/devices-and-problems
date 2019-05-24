using DevicesAndProblems.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesAndProblems.App.Services
{
    public interface IDeviceDataService
    {
        List<Device> GetAllDevices();
        void UpdateDevice(Device selectedDevice, Device newDevice);
        void AddDevice(Device newDevice);
        void DeleteDeviceType(Device selectedDevice);
        List<Device> GetDevicesOfDeviceType(int id);
    }
}
