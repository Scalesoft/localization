using Microsoft.EntityFrameworkCore;

namespace Localization.Database.EFCore.Dao.Impl
{
    public class GenericDao<T, PK> : IGenericDao<T, PK> where T : class
    {
        protected readonly DbSet<T> DbSet;

        protected GenericDao(DbSet<T> dbSet)
        {
            DbSet = dbSet;
        }

        public T Create(T t)
        {
            return DbSet.Add(t).Entity;
        }

        public T Read(PK id)
        {
            return DbSet.Find(id);
        }

        public T Update(T t)
        {
            return DbSet.Update(t).Entity;
        }

        public void Delete(T t)
        {
            DbSet.Remove(t);
        }
    }
}