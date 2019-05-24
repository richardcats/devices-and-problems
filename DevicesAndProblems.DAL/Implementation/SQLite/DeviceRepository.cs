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
            string sql = "SELECT Device.DeviceID AS ID, Device.Name AS DeviceName, " +
                                "DeviceType.ID AS DeviceTypeValue, DeviceType.Name AS DeviceTypeName, " +
                                "SerialNumber, Department, Device.Comments AS DeviceComments, " +
                                "Date(Device.FirstAddedDate) AS FirstAddedDate, COUNT(Storing.StoringID) AS Storingen " +
                                "FROM Device " +
                                "LEFT JOIN DeviceStoring ON DeviceStoring.DeviceID = Device.DeviceID " +
                                "LEFT JOIN Storing ON DeviceStoring.StoringID = Storing.StoringID AND Status='Open' " +
                                "LEFT JOIN DeviceType ON DeviceType.ID = Device.DeviceTypeID " +
                                "GROUP BY Device.DeviceID";

            return SelectList<Device>(sql, null).ToList();
        }

        public List<Device> SelectListByID(int id)
        {
            string sql = "SELECT DeviceID AS ID, Name, Department, Date(FirstAddedDate) AS FirstAddedDate " +
                "FROM Device WHERE DeviceTypeID = '" + id + "'";

            return SelectList<Device>(sql, null).ToList();
        }
    }
}
