using DevicesAndProblems.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicesAndProblems.DAL.Interface
{
    public interface ICommentRepository
    {
        List<Comment> GetCommentsByProblemId(int problemId);
        void Add(Comment comment, int problemId);
        void Delete(Comment comment, int problemId);
    }
}
