using DevicesAndProblems.App.Services;
using DevicesAndProblems.App.ViewModel;
using DevicesAndProblems.DAL;

namespace DevicesAndProblems.App
{
    public class ViewModelLocator
    {
        private static IDialogService dialogService = new DialogService();

        private static IDeviceTypeDataService deviceTypeDataService = new DeviceTypeDataService(new DeviceTypeRepository());

        private static OverviewViewModel overviewViewModel = new OverviewViewModel();

        private static DeviceTypeOverviewViewModel deviceTypeOverviewViewModel = new DeviceTypeOverviewViewModel(deviceTypeDataService, dialogService);

        private static DeviceTypeDetailViewModel deviceTypeDetailViewModel = new DeviceTypeDetailViewModel(deviceTypeDataService, dialogService);

        public static OverviewViewModel OverviewViewModel
        {
            get
            {
                return overviewViewModel;
            }
        }

        public static DeviceTypeOverviewViewModel DeviceTypeOverviewViewModel
        {
            get
            {
                return deviceTypeOverviewViewModel;
            }
        }

        public static DeviceTypeDetailViewModel DeviceTypeDetailViewModel
        {
            get
            {
                return deviceTypeDetailViewModel;
            }
        }
    }
}

