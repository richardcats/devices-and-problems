using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesAndProblems.DAL.Implementation
{
    public interface IDataAccess
    {
        List<T> GetAll<T>(string sql, object parameters = null);
        void Add<T>(string sql, object poco);
        void Delete(string sql, object parameters = null);
        void Update<T>(string sql, object poco);
    }
}
