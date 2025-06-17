using Contracts.Common;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Product.API.Entities;
using Product.API.Persistence;
using Product.API.Repositories.Intefaces;

namespace Product.API.Repositories;

public class ProductRepository : RepositoryBaseAsync<CatalogProduct, long, ProductContext>, IProductRepository
{
    public ProductRepository(ProductContext context, IUnitOfWork<ProductContext> unitOfWork) : base(context, unitOfWork)
    {
    }
    public async Task<IEnumerable<CatalogProduct>> GetProducts() => await FindAll().ToListAsync();

    public async Task<CatalogProduct?> GetProduct(long id) => await GetByIdAsync(id);

    public async Task<CatalogProduct?> GetProductByNo(string productNo) =>
        await FindByCondition(p => p.No.Equals(productNo)).SingleOrDefaultAsync();

    public Task CreateProduct(CatalogProduct product) => CreateAsync(product);
    public Task UpdateProduct(CatalogProduct product) => UpdateAsync(product);

    public async Task DeleteProduct(long id)
    {
        var product = await GetProduct(id);
        if (product != null) DeleteAsync(product);
    }
}
