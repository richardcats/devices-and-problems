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
        List<Device> GetById(int id);
        void Update(Device newDevice, int selectedDeviceId);
        void Add(Device newDevice);
        void Delete(Device device);
    }
}
