using DataAccessLayer;
using DevicesEnStoringen.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DevicesEnStoringen.Tests
{
    [TestClass]
    public class DeviceTypeDataServiceTest
    {
        private IDeviceTypeRepository repository;

        [TestInitialize]
        public void Init()
        {
            repository = new DeviceTypeRepository();
        }

        [TestMethod]
        public void GetDevicesOfDeviceTypeTest()
        {
            //arrange
            var service = new DeviceTypeDataService(repository);

            //act
            var devices = service.GetDevicesOfDeviceType(0);

            //assert
            Assert.IsNotNull(devices);
        }
    }
}
