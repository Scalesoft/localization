using Microsoft.EntityFrameworkCore;

namespace Localization.Database.EFCore.Dao.Impl
{
    public class GenericDao<T, PK> : IGenericDao<T, PK> where T : class
    {
        protected readonly DbSet<T> m_dbSet;

        protected GenericDao(DbSet<T> dbSet)
        {
            m_dbSet = dbSet;
        }

        public T Create(T t)
        {
            return m_dbSet.Add(t).Entity;
        }

        public T Read(PK id)
        {
            return m_dbSet.Find(id);
        }

        public T Update(T t)
        {
            return m_dbSet.Update(t).Entity;
        }

        public void Delete(T t)
        {
            m_dbSet.Remove(t);
        }
    }
}