﻿using DevicesAndProblems.Model;
using System.Collections.Generic;

namespace DevicesAndProblems.App.Services
{
    public interface IDeviceTypeDataService
    {
        List<DeviceType> GetAllDeviceTypes();
        void UpdateDeviceType(DeviceType newDeviceType, int selectedDeviceTypeId);
        void AddDeviceType(DeviceType newDeviceType);
        void DeleteDeviceType(DeviceType deviceType);
    }
}
