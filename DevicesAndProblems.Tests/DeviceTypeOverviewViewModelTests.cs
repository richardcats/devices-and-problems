using DevicesAndProblems.App.Services;
using DevicesAndProblems.App.ViewModel;
using DevicesAndProblems.DAL;
using DevicesAndProblems.DAL.SQLite;
using DevicesAndProblems.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;

namespace DevicesAndProblems.Tests
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
