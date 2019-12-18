using DevicesAndProblems.DAL.Interface;
using DevicesAndProblems.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesAndProblems.DAL.SQLite
{
    public class CommentRepository : Implementation.SQLiteDataAccess, ICommentRepository
    {
        public List<Comment> GetCommentsByProblemId(int problemId)
        {
            string sql = "SELECT OpmerkingID, Date(Datum) AS Datum, Description " +
                "FROM Opmerking WHERE StoringID = '" + problemId + "'"; // TODO

            return GetAll<Comment>(sql, null).ToList();
        }

        public void Add(Comment comment, int problemId)
        {
            string sql = "INSERT INTO Opmerking " +
                "(StoringID, Datum, Description) " +
                "VALUES (@problemId, date('now'), @Text)"; // TODO

            Add<Comment>(sql, comment);
        }

        public void Delete(Comment comment, int problemId)
        {
            string sql = "DELETE FROM Opmerking " +
                "WHERE Description = @Text," +
                "AND StoringID = '" + problemId + "'"; // TODO

            Delete(sql, comment);
        }
    }
}
