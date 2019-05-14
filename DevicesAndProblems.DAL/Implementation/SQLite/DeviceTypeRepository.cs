using DevicesAndProblems.DAL.Interface;
using DevicesAndProblems.Model;
using System.Collections.Generic;
using System.Linq;

namespace DevicesAndProblems.DAL.SQLite
{
    public class DeviceTypeRepository : Implementation.SQLiteDataAccess, IDeviceTypeRepository
    {
        public List<DeviceType> SelectList()
        {
            string sql = "SELECT DeviceType.Id, DeviceType.Name, " +
                "COUNT(Device.DeviceTypeID) AS DeviceAmount, DeviceType.Description " +
                "FROM DeviceType " +
                "LEFT JOIN Device ON Device.DeviceTypeID = DeviceType.Id " +
                "GROUP BY DeviceType.ID " +
                "ORDER BY ID";

            return SelectList<DeviceType>(sql, null).ToList();
        }

        public void Update(DeviceType newDeviceType, int selectedDeviceTypeId)
        {
            string sql = "UPDATE DeviceType SET Name = @Name, Description = @Description" +
                " WHERE Id = '" + selectedDeviceTypeId + "'";

            Update<DeviceType>(sql, newDeviceType);
        }

        public void Insert(DeviceType newDeviceType)
        {
            string sql = "INSERT INTO DeviceType (Name, Description) " +
                "VALUES (@Name, @Description)";

            Insert<DeviceType>(sql, newDeviceType);
        }

        public void Delete(DeviceType deviceType)
        {
            string sql = "DELETE FROM DeviceType " +
                "WHERE Id = @Id";

            Delete(sql, deviceType);
        }
    }
}
