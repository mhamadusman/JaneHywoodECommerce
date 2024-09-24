using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IRepository<TEntity>
    {
        Task Add(TEntity entity);
        Task<IEnumerable<TEntity>> GetAll();
        Task Edit(TEntity entity);
        Task delete(int id);
        Task<TEntity> find(int id);
    }
}
