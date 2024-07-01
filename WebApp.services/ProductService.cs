using Microsoft.EntityFrameworkCore;
using WebApp.data;
using WebApp.models;

namespace WebApp.services;

public class ProductService
{
    private readonly AppDb _dbContext;

    public ProductService(AppDb dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        return await _dbContext.Products.ToListAsync();
    }

    public async Task<Product> GetProductByIdAsync(Guid productId)
    {
        return await _dbContext.Products.FindAsync(productId);
    }

    public async Task AddProductAsync(Product product)
    {
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateProductAsync(Product product)
    {
        _dbContext.Products.Update(product);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(Guid productId)
    {
        var product = await _dbContext.Products.FindAsync(productId);
        if (product != null)
        {
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
        }
    }
}
