using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Configuration;

namespace DataAccessLayer
{
    public class MockRepository : IDeviceTypeRepository
    {
        private List<DeviceType> deviceTypes;

        public MockRepository()
        {
            deviceTypes = LoadMockDeviceTypes();
        }

        private List<DeviceType> LoadMockDeviceTypes()
        {
            return new List<DeviceType>()
            {
                new DeviceType()
                {
                    DeviceTypeId = 1,
                    DeviceTypeName = "Telefoon",
                    Description = "",
                    DeviceAmount = 2
                },
                new DeviceType()
                {
                    DeviceTypeId = 2,
                    DeviceTypeName = "Workstation",
                    Description = "",
                    DeviceAmount = 3
                },
                new DeviceType()
                {
                    DeviceTypeId = 3,
                    DeviceTypeName = "Server",
                    Description = "",
                    DeviceAmount = 3
                },
                new DeviceType()
                {
                    DeviceTypeId = 4,
                    DeviceTypeName = "Printer",
                    Description = "",
                    DeviceAmount = 2
                },
                new DeviceType()
                {
                    DeviceTypeId = 5,
                    DeviceTypeName = "test",
                    Description = "",
                    DeviceAmount = 0
                },
            };
        }

        public List<DeviceType> GetDeviceTypes()
        {
            return deviceTypes;  
        }

        public void UpdateDeviceType(DeviceType newDeviceType, int selectedDeviceTypeId)
        {

        }

        public void AddDeviceType(DeviceType newDeviceType)
        {

        }

        public void DeleteDeviceType(DeviceType deviceType)
        {
        }


        public List<Device> GetDevicesOfDeviceType(int id)
        {
            return null;
        }

    }
}
