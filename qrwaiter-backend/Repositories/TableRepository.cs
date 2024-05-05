using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Microsoft.EntityFrameworkCore;
using qrwaiter_backend.Repositories.Interfaces;
using qrwaiter_backend.Data.Models;
using qrwaiter_backend.Data;
using Google.Apis.Util;

namespace qrwaiter_backend.Repositories
{

    public class TableRepository : Repository<Table>, ITableRepository
    {
        private readonly ApplicationDbContext _context;

        public TableRepository(ApplicationDbContext context) :base(context)
        {
            this._context = context;
        }


        public async Task<List<Table>> GetTablesWithQrCodesWithDevicesByRestaurantLink(string restaurantLink)
        {
            Restaurant rest = await _context.Restaurant.Where(r => r.WaiterLink == restaurantLink)
                                                .Include(r => r.Tables.Where(t => t.IsDeleted == false))
                                                .ThenInclude(t => t.QrCode)
                                                .ThenInclude(qr => qr.NotifyDevices)
                                                .FirstAsync();
            return rest.Tables.OrderBy(t => t.Number).ToList();

        }
    }
}

