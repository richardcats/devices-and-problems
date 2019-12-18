using DevicesAndProblems.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesAndProblems.DAL.Interface
{
    public interface IDeviceRepository
    {
        List<Device> GetAll();
        List<Device> GetByDeviceTypeId(int deviceTypeId);
        List<Device> GetDevicesByProblemId(int problemId);
        void Add(Device device);
        void Update(Device device, int deviceId);
        void Delete(Device device);
    }
}
