using DevicesAndProblems.App.Services;
using DevicesAndProblems.App.ViewModel;
using DevicesAndProblems.DAL.SQLite;

namespace DevicesAndProblems.App
{
    public class ViewModelLocator
    {
        private static IDialogService dialogService = new DialogService();

        private static IProblemDataService problemDataService = new ProblemDataService(new ProblemRepository());

        private static IDeviceTypeDataService deviceTypeDataService = new DeviceTypeDataService(new DeviceTypeRepository());

        private static IDeviceDataService deviceDataService = new DeviceDataService(new DeviceRepository());

        private static OverviewViewModel overviewViewModel = new OverviewViewModel();

        private static ProblemOverviewViewModel problemOverviewViewModel = new ProblemOverviewViewModel(problemDataService, dialogService);

        private static DeviceTypeOverviewViewModel deviceTypeOverviewViewModel = new DeviceTypeOverviewViewModel(deviceTypeDataService, dialogService);

        private static DeviceTypeDetailViewModel deviceTypeDetailViewModel = new DeviceTypeDetailViewModel(deviceDataService, deviceTypeDataService, dialogService);

        private static DeviceDetailViewModel deviceDetailViewModel = new DeviceDetailViewModel(deviceDataService, problemDataService, dialogService);

        private static DeviceOverviewViewModel deviceOverviewViewModel = new DeviceOverviewViewModel(deviceDataService, dialogService);

        public static OverviewViewModel OverviewViewModel
        {
            get
            {
                return overviewViewModel;
            }
        }

        public static ProblemOverviewViewModel ProblemOverviewViewModel
        {
            get
            {
                return problemOverviewViewModel;
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

        public static DeviceOverviewViewModel DeviceOverviewViewModel
        {
            get
            {
                return deviceOverviewViewModel;
            }
        }

        public static DeviceDetailViewModel DeviceDetailViewModel
        {
            get
            {
                return deviceDetailViewModel;
            }
        }
    }
}

