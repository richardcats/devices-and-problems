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
        void UpdateDevice(Device newDevice, int selectedDeviceId);
        void AddDevice(Device newDevice);
        void DeleteDevice(Device selectedDevice);
        List<Device> GetDevicesOfDeviceType(int deviceTypeId);
    }
}
