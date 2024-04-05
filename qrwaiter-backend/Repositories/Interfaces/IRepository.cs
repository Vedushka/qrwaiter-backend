using Microsoft.EntityFrameworkCore;

namespace qrwaiter_backend.Repositories.Interfaces
{
        public interface IRepository<T> where T : class
        {
            Task<IQueryable<T>> GetAll();
            Task<T> GetById(Guid id);
            Task<T> Insert(T entity);
            Task<T> Update(T entity);
            void DeleteById(Guid id);
            void Delete(T entity);
        
    }
}
