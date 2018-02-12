﻿using System.Configuration;
using System.Data;
using System.Data.SQLite;

namespace DevicesEnStoringen
{
    class DatabaseConnection
    {
        private SQLiteConnection conn;
        private readonly string connString = ConfigurationManager.ConnectionStrings["DevicesEnStoringen"].ToString();
  
        public void OpenConnection()
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

        public DataTable ShowDataInGridView(string Query_)
        {
            SQLiteDataAdapter dr = new SQLiteDataAdapter(Query_, connString);
            DataSet ds = new DataSet();
            dr.Fill(ds);
            DataTable dataum = ds.Tables[0];
            return dataum;
        }

        public SQLiteCommand ReturnSQLiteCommand(string Query_)
        {
            SQLiteCommand sqlCmd = new SQLiteCommand(Query_, conn);
            return sqlCmd;
        }
    }
}
