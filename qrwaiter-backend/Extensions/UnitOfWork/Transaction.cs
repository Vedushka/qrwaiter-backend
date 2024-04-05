
namespace qrwaiter_backend.Extensions.UnitOfWork
{
    public class Transaction : ITransaction
    { 
        public Task CommitAsync() => Task.CompletedTask;

        public void Dispose()
        {
        }

        public Task RollBackAsync() => Task.CompletedTask;
    }
}
