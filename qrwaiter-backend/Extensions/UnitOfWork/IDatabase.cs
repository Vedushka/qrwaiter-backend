namespace qrwaiter_backend.Extensions.UnitOfWork
{
    public interface IDatabase
    {
        Task<ITransaction> BeginTransactionAsync();
    }
}
