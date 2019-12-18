using DevicesAndProblems.DAL.Interface;
using DevicesAndProblems.Model;
using System.Collections.Generic;
using System.Linq;

namespace DevicesAndProblems.DAL.SQLite
{
    public class DeviceTypeRepository : Implementation.SQLiteDataAccess, IDeviceTypeRepository
    {
        public List<DeviceType> GetAll()
        {
            string sql = "SELECT DeviceType.Id, DeviceType.Name, " +
                "COUNT(Device.DeviceTypeID) AS DeviceAmount, DeviceType.Description " +
                "FROM DeviceType " +
                "LEFT JOIN Device ON Device.DeviceTypeID = DeviceType.Id " +
                "GROUP BY DeviceType.ID " +
                "ORDER BY DeviceType.Id";

            return GetAll<DeviceType>(sql, null).ToList();
        }

        public void Add(DeviceType deviceType)
        {
            string sql = "INSERT INTO DeviceType (Name, Description) " +
                "VALUES (@Name, @Description)";

            Add<DeviceType>(sql, deviceType);
        }

        public void Update(DeviceType deviceType, int deviceTypeId)
        {
            string sql = "UPDATE DeviceType SET Name = @Name, Description = @Description" +
                " WHERE Id = '" + deviceTypeId + "'";

            Update<DeviceType>(sql, deviceType);
        }

        public void Delete(DeviceType deviceType)
        {
            string sql = "DELETE FROM DeviceType " +
                "WHERE Id = @Id";

            Delete(sql, deviceType);
        }
    }
}
