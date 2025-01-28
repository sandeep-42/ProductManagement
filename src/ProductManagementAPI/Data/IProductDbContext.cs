using Microsoft.EntityFrameworkCore;
using ProductManagementAPI.Data.Entities;

namespace ProductManagementAPI.Data
{
    internal interface IProductDbContext
    {
        DbSet<Product> Products { get; set; }
    }
}
