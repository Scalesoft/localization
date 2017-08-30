namespace Localization.Database.EFCore.Dao
{
    public interface IGenericDao<T, PK>
    {
        T Create(T t);
        T Read(PK id);
        T Update(T t);
        void Delete(T t);
    }
}