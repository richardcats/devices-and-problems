using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesAndProblems.DAL.Implementation
{
    public class SQLiteDataAccess : IDataAccess
    {
        private string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        public List<T> GetAll<T>(string sql, object parameters = null)
        {
            using (IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
            {
                conn.Open();
                return conn.Query<T>(sql, parameters).ToList();
            }
        }

        public void Add<T>(string sql, object poco)
        {
            using (IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
            {
                conn.Open();
                conn.ExecuteScalar<int>(sql, (T)poco);
            }
        }

        public void Delete(string sql, object parameters = null)
        {
            using (IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
            {
                conn.Open();
                conn.Execute(sql, parameters);
            }
        }

        public void Update<T>(string sql, object poco)
        {
            using (IDbConnection conn = new SQLiteConnection(LoadConnectionString()))
            {
                conn.Open();
                conn.Execute(sql, (T)poco);
            }
        }
    }
}
