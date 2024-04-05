using Microsoft.EntityFrameworkCore;
using qrwaiter_backend.Data;
using qrwaiter_backend.Data.Models;
using qrwaiter_backend.Repositories.Interfaces;
using System;

namespace qrwaiter_backend.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public async Task<T> GetById(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> Insert(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async void DeleteById(Guid id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
            {
                throw new InvalidOperationException();
            }
            _context.Set<T>().Remove(entity);
        }
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        public async Task<T> Update(T entity)
        {
            _context.Set<T>().Update(entity);
            return entity;
        }

        Task<IQueryable<T>> IRepository<T>.GetAll()
        {
            throw new NotImplementedException();
        }

    }
}
