using ProductManagementAPI.Data.Repositories;

namespace ProductManagementAPI.Data
{
    internal interface IProductUnitOfWork : IDisposable
    {
        IProductRepository productRepository { get; }

        Task<int> SaveChangesAsync();

    }
}
