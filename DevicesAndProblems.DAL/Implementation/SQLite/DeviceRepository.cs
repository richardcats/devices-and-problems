using DevicesAndProblems.DAL.Interface;
using DevicesAndProblems.Model;
using System.Collections.Generic;
using System.Linq;

namespace DevicesAndProblems.DAL.SQLite
{
    public class DeviceRepository : Implementation.SQLiteDataAccess, IDeviceRepository
    {
        public List<Device> SelectListByID(int id)
        {
            string sql = "SELECT DeviceID AS ID, Naam, Afdeling, Date(DatumToegevoegd) AS Datum " +
                "FROM Device WHERE DeviceTypeID = '" + id + "'";

            return SelectList<Device>(sql, null).ToList();
        }
    }
}
