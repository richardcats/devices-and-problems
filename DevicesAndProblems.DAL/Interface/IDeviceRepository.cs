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
        List<Device> SelectList();
        List<Device> SelectListByID(int id);
    }
}
