﻿
using Microsoft.EntityFrameworkCore;
using qrwaiter_backend.Data;
using qrwaiter_backend.Repositories;
using qrwaiter_backend.Repositories.Interfaces;

namespace qrwaiter_backend.Extensions.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IRestaurantRepository _restaurantRepository;
        private ITransaction? _currentTransaction;
            public UnitOfWork(ApplicationDbContext context)
            {
                _context = context;
            }
        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction is not null)
                throw new InvalidOperationException("A transaction has already been started.");
            _currentTransaction = await _context.BeginTransaction();
        }
        public async Task CommitAsync()
        {
            if (_currentTransaction is null)
                throw new InvalidOperationException("A transaction has not been started.");

            try
            {
                await _currentTransaction.CommitAsync();
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
            catch (Exception)
            {
                if (_currentTransaction is not null)
                    await _currentTransaction.RollBackAsync();
                throw;
            }
        }


        public IRestaurantRepository RestaurantRepository => _restaurantRepository ??= new RestaurantRepository(_context);
        public void SaveChanges()
            {
                _context.SaveChanges();
            }
        }
    }
