using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace DevicesEnStoringen
{
    class DatabaseConnectie
    {
        private SQLiteConnection conn;
        private string connString = ConfigurationManager.ConnectionStrings["DevicesEnStoringen"].ToString();

        public void OpenConection()
        {
            conn = new SQLiteConnection(connString);
            conn.Open();
        }
        public void CloseConnection()
        {
            conn.Close();
        }
        public void ExecuteQueries(string Query_)
        {
            SQLiteCommand cmd = new SQLiteCommand(Query_, conn);
            cmd.ExecuteNonQuery();
        }

        public SQLiteDataReader DataReader(string Query_)
        {
            SQLiteCommand cmd = new SQLiteCommand(Query_, conn);
            SQLiteDataReader dr = cmd.ExecuteReader();
            return dr;
        }

        public object ShowDataInGridView(string Query_)
        {
            SQLiteDataAdapter dr = new SQLiteDataAdapter(Query_, connString);
            DataSet ds = new DataSet();
            dr.Fill(ds);
            object dataum = ds.Tables[0];
            return dataum;
        }
    }
}
