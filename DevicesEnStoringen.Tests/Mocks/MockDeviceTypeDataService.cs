using DataAccessLayer;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesEnStoringen.Services
{
    public class MockDeviceTypeDataService : IDeviceTypeDataService
    {
        private MockRepository repository = new MockRepository();

        public List<DeviceType> GetAllDeviceTypes()
        {
            return repository.GetDeviceTypes();
        }

        public void UpdateDeviceType(DeviceType newDeviceType, int selectedDeviceTypeId)
        {
            repository.UpdateDeviceType(newDeviceType, selectedDeviceTypeId);
        }

        public void AddDeviceType(DeviceType newDeviceType)
        {
            repository.AddDeviceType(newDeviceType);
        }

        public void DeleteDeviceType(DeviceType deviceType)
        {
        }

        public List<Device> GetDevicesOfDeviceType(int id)
        {
            return repository.GetDevicesOfDeviceType(id);
        }
    }
}
