using Microsoft.EntityFrameworkCore;
using Npgsql;
using ProductService.DTOs;
using ProductService.Exceptions;
using ProductService.Data;
using ProductService.Data.Entities;

namespace ProductService.Services;

public class ProductManagementService
{
    private readonly AppDbContext _context;
    private readonly ILogger<ProductManagementService> _logger;

    public ProductManagementService(AppDbContext context, ILogger<ProductManagementService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ProductResponseDto> CreateAsync(CreateProductDto req)
    {
        var product = new Product
        {
            Name = req.Name,
            Description = req.Description ?? "",
            Price = req.Price,
            StockQuantity = req.StockQuantity,
            CreatedAt = DateTime.UtcNow
        };

        _context.Products.Add(product);

        try
        { 
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
            when (
                ex.InnerException is PostgresException pgEx &&
                pgEx.SqlState == PostgresErrorCodes.UniqueViolation
            )
        {
            throw new DuplicateProductNameException(req.Name);
        }

        return MapToProductResponseDto(product);
    }

    public async Task<ProductResponseDto?> GetByIdAsync(int id)
    {
        var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        if (product == null) return null;
        return MapToProductResponseDto(product);
    }

    public async Task<List<ProductResponseDto>> GetAllAsync()
    {
        var products = await _context.Products.AsNoTracking().ToListAsync();
        return [.. products.Select(MapToProductResponseDto)];
    }

    public async Task<ProductResponseDto?> UpdateAsync(int id, UpdateProductDto req)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null) return null;

        product.Name = req.Name;
        product.Description = req.Description ?? product.Description;
        product.Price = req.Price;
        product.StockQuantity = req.StockQuantity;

        await _context.SaveChangesAsync();

        return MapToProductResponseDto(product);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null) return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }

    private static ProductResponseDto MapToProductResponseDto(Product product)
    {
        return new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            CreatedAt = product.CreatedAt
        };
    }
}