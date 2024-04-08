using qrwaiter_backend.Repositories.Interfaces;

namespace qrwaiter_backend.Extensions.UnitOfWork
{
    public interface IUnitOfWork
    {
        void SaveChanges();
        Task BeginTransactionAsync();
        Task CommitAsync();
        IRestaurantRepository RestaurantRepository { get; }
        ITableRepository TableRepository { get; }
    }
}
