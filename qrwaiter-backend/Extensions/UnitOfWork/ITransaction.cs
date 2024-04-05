namespace qrwaiter_backend.Extensions.UnitOfWork
{
    public interface ITransaction : IDisposable
    {
        Task CommitAsync();
        Task RollBackAsync();
    }
}
