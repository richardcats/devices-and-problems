using DevicesEnStoringen.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesEnStoringen
{
    public class ViewModelLocator
    { 


        private static DeviceTypeOverviewViewModel deviceTypeOverviewViewModel = new DeviceTypeOverviewViewModel();

        private static DeviceTypeDetailViewModel deviceTypeDetailViewModel = new DeviceTypeDetailViewModel();

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

