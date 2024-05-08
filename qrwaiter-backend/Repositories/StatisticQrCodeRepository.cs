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

    public class StatisticQrCodeRepository : Repository<StatisticQrCode>, IStatisticQrCodeRepository
    {
        private readonly ApplicationDbContext _context;

        public StatisticQrCodeRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<StatisticQrCode?> GetLastByQrCodeId(Guid qrCodeId)
        {
            return await _context.StatisticQrCode.FirstOrDefaultAsync(sqr => sqr.IdQrCode == qrCodeId);
        }
    }
}

