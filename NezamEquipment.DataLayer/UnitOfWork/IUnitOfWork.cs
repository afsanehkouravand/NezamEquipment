using System.Data.Entity;
using System.Threading.Tasks;

namespace NezamEquipment.DataLayer.UnitOfWork
{
    public interface IUnitOfWork<T>
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;

        Task<int> SaveChangesAsync();

        int SaveChanges();

        void MarkAsChanged<TEntity>(TEntity entity) where TEntity : class;

        void MarkAsSafeDelete<TEntity>(TEntity entity) where TEntity : class;

        Database Database { get; }
    }
}
