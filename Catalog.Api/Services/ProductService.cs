using Catalog.Api.Data;
using Catalog.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ProductService> _logger;

    public ProductService(ApplicationDbContext context, ILogger<ProductService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<List<Product>> SearchByCategoryAsync(string category)
    {
        return await _context.Products
            .Where(p => p.Category == category)
            .ToListAsync();
    }

    public async Task<Product> CreateAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Product Created: Id={ProductId}, Name={ProductName}, Category={Category}, Price={Price}", 
            product.Id, product.Name, product.Category, product.Price);

        return product;
    }

    public async Task<bool> UpdateAsync(Product product)
    {
        var existing = await _context.Products.FindAsync(product.Id);

        if (existing == null)
        {
            return false;
        }

        existing.Name = product.Name;
        existing.Price = product.Price;
        existing.Category = product.Category;
        existing.Stock = product.Stock;
        existing.UpdatedDate = product.UpdatedDate;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Product Updated: Id={ProductId}, Name={ProductName}, Category={Category}, Price={Price}", 
            product.Id, product.Name, product.Category, product.Price);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return false;
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Product Deleted: Id={ProductId}, Name={ProductName}", 
            id, product.Name);

        return true;
    }
}