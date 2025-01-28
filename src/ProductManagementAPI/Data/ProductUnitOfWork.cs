using ProductManagementAPI.Data.Repositories;

namespace ProductManagementAPI.Data
{
    internal class ProductUnitOfWork : IProductUnitOfWork
    {
        private readonly ProductDbContext _dbContext;
        private readonly Lazy<IProductRepository> _productRepository;

        public ProductUnitOfWork(ProductDbContext dbContext)
        {
            _dbContext = dbContext;

            _productRepository = new Lazy<IProductRepository>(() => new ProductRepository(_dbContext));
        }

        public IProductRepository productRepository => _productRepository.Value;

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
