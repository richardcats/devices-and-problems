using DevicesAndProblems.DAL.Interface;
using DevicesAndProblems.Model;
using System.Collections.Generic;
using System.Linq;

namespace DevicesAndProblems.DAL.SQLite
{
    public class DeviceRepository : Implementation.SQLiteDataAccess, IDeviceRepository
    {
        public List<Device> GetAll()
        {
            string sql = "SELECT Device.Id AS Id, Device.Name AS Name, " +
                                "DeviceType.Name AS DeviceTypeName, " +
                                "SerialNumber, Department, Device.Comments AS Comments, " +
                                "Date(Device.FirstAddedDate) AS FirstAddedDate, COUNT(Storing.Id) AS NumberOfFaults " +
                                "FROM Device " +
                                "LEFT JOIN DeviceStoring ON DeviceStoring.DeviceID = Device.Id " +
                                "LEFT JOIN Storing ON DeviceStoring.StoringID = Storing.Id AND Status='Open' " +
                                "LEFT JOIN DeviceType ON DeviceType.Id = Device.DeviceTypeId  " +
                                "GROUP BY Device.Id";

            return GetAll<Device>(sql, null).ToList();
        }

        public List<Device> GetByDeviceTypeId(int deviceTypeId)
        {
            string sql = "SELECT Id, Name, Department, Date(FirstAddedDate) AS FirstAddedDate " +
                "FROM Device WHERE Id = '" + deviceId + "'";

            return GetAll<Device>(sql, null).ToList();
        }

        public List<Device> GetDevicesByProblemId(int problemId)
        {

        }

        public void Add(Device device)
        {
            string sql = "INSERT INTO Device (DeviceTypeId, Name, SerialNumber, Department, Comments, FirstAddedDate) " +
                "VALUES ((SELECT Id FROM DeviceType WHERE Name = @DeviceTypeName), @Name, @SerialNumber, @Department, @Comments, date('now'))";

            Add<Device>(sql, device);
        }

        public void Update(Device device, int deviceId)
        {
            string sql = "UPDATE Device SET DeviceTypeId = (SELECT Id FROM DeviceType WHERE Name = @DeviceTypeName), Name = @Name, " +
                "SerialNumber = @SerialNumber, Department = @Department, Comments = @Comments " +
                "WHERE Id = '" + deviceId + "'";

            Update<Device>(sql, device);
        }

        public void Delete(Device device)
        {
            string sql = "DELETE FROM Device " +
                "WHERE Id = @Id";

            Delete(sql, device);
        }
    }
}
