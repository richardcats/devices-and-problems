using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SQLite;

namespace DevicesEnStoringen.DataAccess
{

    class DataCommander
    {
        public event Action ContainersReady; //Delegate maar dan zonder parameters


        private readonly SQLiteConnection conn;
        private string connString = ConfigurationManager.ConnectionStrings["DevicesEnStoringen"].ToString();

        public DataCommander()
        {
            conn = new SQLiteConnection(connString);
        }

        public List<string> GetAllContainers()
        {
            List<string> list = new List<string>();

            string sql = "SELECT * FROM devices";

            SQLiteCommand command = new SQLiteCommand(conn);
            command.CommandText = sql;

            conn.Open();

            SQLiteDataReader datareader;

            using (datareader = command.ExecuteReader())
            {
                while (datareader.Read())
                {
                    string result = String.Format("Device: {0}, {1}, {2}, {3}, {4}",
                        datareader.GetInt32(0),
                        datareader.GetString(1),
                        datareader.GetString(2),
                        datareader.GetString(3),
                        datareader.GetString(4));

                    list.Add(result);
                }
            }
            conn.Close();


            if (ContainersReady != null)
            {
                ContainersReady.Invoke();
            }
            return list;
        }
    }
}
