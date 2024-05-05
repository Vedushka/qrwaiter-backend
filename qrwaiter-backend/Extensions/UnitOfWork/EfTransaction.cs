using Microsoft.EntityFrameworkCore.Storage;

namespace qrwaiter_backend.Extensions.UnitOfWork
{
    public class EfTransaction : ITransaction
    {
        private IDbContextTransaction _dbContextTransaction;
        public EfTransaction(IDbContextTransaction dbContextTransaction)
        {
            _dbContextTransaction = dbContextTransaction;
        }
        public Task CommitAsync() => _dbContextTransaction.CommitAsync();
        public Task RollBackAsync() => _dbContextTransaction.RollbackAsync();

        public void Dispose()
        {
            _dbContextTransaction?.Dispose();
            _dbContextTransaction = null!;
        }
    }
}
