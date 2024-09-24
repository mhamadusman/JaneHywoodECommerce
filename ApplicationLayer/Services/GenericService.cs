using Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationLayer.Services
{
    public class GenericService<TEntity>
    {
        private readonly IRepository<TEntity> _irepo;

        public GenericService(IRepository<TEntity> repo)
        {
            _irepo = repo;
        }

        public async Task Add(TEntity entity)
        {
            await _irepo.Add(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _irepo.GetAll();
        }

        public async Task Edit(TEntity entity)
        {
            await _irepo.Edit(entity);
        }

        public async Task delete(int id)
        {
            await _irepo.delete(id);
        }

        public async Task<TEntity> find(int id)
        {
            return await _irepo.find(id);
        }
    }
}
