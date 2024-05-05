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

    public class QrCodeRepository : Repository<QrCode>, IQrCodeRepository
    {
        private readonly ApplicationDbContext _context;

        public QrCodeRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<QrCode> GetByLink(string link, LinkType type)
        {
            return type switch
            {
                LinkType.ClientLink => await _context.QrCode.Where(qr => qr.ClientLink == link).Include(qr => qr.Table).Include(qr => qr.NotifyDevices).Include(qr => qr.Table).FirstAsync(),
                LinkType.WaiterLink => await _context.QrCode.Where(qr => qr.WaiterLink == link).Include(qr => qr.Table).Include(qr => qr.NotifyDevices).Include(qr => qr.Table).FirstAsync(),
                _ => throw new NoNullAllowedException()
            };
        }

        public async Task<QrCode> GenerateNewLink(Guid id, LinkType type)
        {
            QrCode? qrCode = await _context.QrCode.FindAsync(id);
            if (qrCode == null)
            {
                throw new InvalidOperationException();
            }
            if (type == LinkType.WaiterLink)
            {
                qrCode.WaiterLink = ShortId.Generate();
                _context.QrCode.Update(qrCode);
            }
            else if (type == LinkType.ClientLink)
            {
                qrCode.ClientLink = ShortId.Generate();
                _context.QrCode.Update(qrCode);
            }
            return qrCode;
        }

        public void SoftDelete(Guid id)
        {
            QrCode? qrCode = _context.QrCode.Find(id);
            if (qrCode == null)
            {
                throw new InvalidOperationException();
            }
            qrCode.IsDeleted = true;
            _context.Entry(qrCode).State = EntityState.Modified;
        }



    }
}

