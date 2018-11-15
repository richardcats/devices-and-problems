
using DevicesAndProblems.Model;
using System;

namespace DevicesAndProblems.DAL
{
    public class Repository<T> //: IRepository<T> where T : class, IEntity
    {
        public void Add(T newEntity)
        {
            throw new NotImplementedException();
        }

        public void Remove(T entity)
        {
            throw new NotImplementedException();
        }

        public void Update(T newEntity, int selectedEntityId)
        {
            throw new NotImplementedException();
        }
    }
}
