using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Microsoft.EntityFrameworkCore;
using qrwaiter_backend.Repositories.Interfaces;
using qrwaiter_backend.Data.Models;
using qrwaiter_backend.Data;

namespace qrwaiter_backend.Repositories
{

    public class TableRepository : Repository<Table>, ITableRepository
    {
        private readonly ApplicationDbContext _context;

        public TableRepository(ApplicationDbContext context) :base(context)
        {
            this._context = context;
        }

        public Task<List<Table>> GetTablesByRestaurantId(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}

