using DevicesAndProblems.DAL.Interface;
using DevicesAndProblems.Model;
using System.Collections.Generic;
using System.Linq;

namespace DevicesAndProblems.DAL.SQLite
{
    public class DeviceRepository : Implementation.SQLiteDataAccess, IDeviceRepository
    {
        public List<Device> SelectList()
        {
            string sql = "SELECT Device.Id AS Id, Device.Name AS Name, " +
                                "DeviceType.Id AS DeviceTypeValue, DeviceType.Name AS DeviceTypeName, " +
                                "SerialNumber, Department, Device.Comments AS Comments, " +
                                "Date(Device.FirstAddedDate) AS FirstAddedDate, COUNT(Storing.StoringID) AS NumberOfFaults " +
                                "FROM Device " +
                                "LEFT JOIN DeviceStoring ON DeviceStoring.DeviceID = Device.Id " +
                                "LEFT JOIN Storing ON DeviceStoring.StoringID = Storing.StoringID AND Status='Open' " +
                                "LEFT JOIN DeviceType ON DeviceType.Id = Device.DeviceTypeId  " +
                                "GROUP BY Device.Id";

            return SelectList<Device>(sql, null).ToList();
        }

        public List<Device> SelectListById(int id)
        {
            string sql = "SELECT Id, Name, Department, Date(FirstAddedDate) AS FirstAddedDate " +
                "FROM Device WHERE Id = '" + id + "'";

            return SelectList<Device>(sql, null).ToList();
        }

        public void Update(Device newDevice, int selectedDeviceId)
        {
            string sql = "UPDATE Device SET DeviceTypeId = @DeviceTypeValue, Name = @Name, " +
                "SerialNumber = @SerialNumber, Department = @Department, Comments = @Comments " +
                "WHERE Id = '" + selectedDeviceId + "'";

            Update<Device>(sql, newDevice);
        }
    }
}
