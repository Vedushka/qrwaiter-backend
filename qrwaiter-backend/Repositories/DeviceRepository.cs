using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Microsoft.EntityFrameworkCore;
using qrwaiter_backend.Repositories.Interfaces;
using qrwaiter_backend.Data.Models;
using qrwaiter_backend.Data;
using qrwaiter_backend.Extensions;
using shortid;

namespace qrwaiter_backend.Repositories
{

    public class DeviceRepository : Repository<Device>, IDeviceRepository
    {
        private readonly ApplicationDbContext _context;

        public DeviceRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<Device?> GetByDeviceToken(string token)
        {
            return await _context.Device.Include(d => d.QrCodes).Where(d => d.DeviceToken == token).FirstOrDefaultAsync();
        }
    }
}

