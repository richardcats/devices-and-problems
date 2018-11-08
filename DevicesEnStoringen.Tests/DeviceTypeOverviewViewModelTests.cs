using System;
using System.Collections.ObjectModel;
using System.Linq;
using DataAccessLayer;
using DevicesEnStoringen.Services;
using DevicesEnStoringen.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace DevicesEnStoringen.Tests
{
    [TestClass]
    public class DeviceTypeOverviewViewModelTests
    {
        private IDeviceTypeDataService deviceTypeDataService;
        private IDialogService dialogService;

        private DeviceTypeOverviewViewModel GetViewModel()
        {
            return new DeviceTypeOverviewViewModel(new DeviceTypeDataService(new DeviceTypeRepository()), dialogService);
        }

        [TestInitialize]
        public void Init()
        {
            deviceTypeDataService = new MockDeviceTypeDataService();
            dialogService = new MockDialogService();
        }

        [TestMethod]
        public void LoadAllDeviceTypes()
        {
            //Arrange
            ObservableCollection<DeviceType> deviceTypes = null;
            var expectedDeviceTypes = deviceTypeDataService.GetAllDeviceTypes();

            //act
            var viewModel = GetViewModel();
            deviceTypes = viewModel.DeviceTypes;

            //assert
            CollectionAssert.AreEqual(expectedDeviceTypes, deviceTypes);
        }
    }
}
