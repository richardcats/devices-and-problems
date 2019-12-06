using DevicesAndProblems.DAL.Interface;
using DevicesAndProblems.Model;
using System.Collections.Generic;

namespace DevicesAndProblems.Tests
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
                    Id = 1,
                    Name = "Telefoon",
                    Description = "",
                    DeviceAmount = 2
                },
                new DeviceType()
                {
                    Id = 2,
                    Name = "Workstation",
                    Description = "",
                    DeviceAmount = 3
                },
                new DeviceType()
                {
                    Id = 3,
                    Name = "Server",
                    Description = "",
                    DeviceAmount = 3
                },
                new DeviceType()
                {
                    Id = 4,
                    Name = "Printer",
                    Description = "",
                    DeviceAmount = 2
                },
                new DeviceType()
                {
                    Id = 5,
                    Name = "test",
                    Description = "",
                    DeviceAmount = 0
                },
            };
        }

        public List<DeviceType> GetDeviceTypes()
        {
            return deviceTypes;  
        }


        public void DeleteDeviceType(DeviceType deviceType)
        {
        }


        public List<Device> GetDevicesOfDeviceType(int id)
        {
            return null;
        }

        public List<DeviceType> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public void Update(DeviceType newDeviceType, int selectedDeviceTypeId)
        {
            throw new System.NotImplementedException();
        }

        public int Add(DeviceType newDeviceType)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(DeviceType deviceType)
        {
            throw new System.NotImplementedException();
        }
    }
}
