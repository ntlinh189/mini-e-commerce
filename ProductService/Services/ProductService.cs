using ProductService.DTOs;
using ProductService.Models;

namespace ProductService.Services;

public class ProductManagementService
{
    private readonly List<Product> _products = new();
    private int _nextId = 1;

    public ProductResponseDto Create(CreateProductDto req)
    {
        var product = new Product
        {
            Id = _nextId++,
            Name = req.Name,
            Description = req.Description,
            Price = req.Price,
            StockQuantity = req.StockQuantity,
            CreatedAt = DateTime.UtcNow
        };

        _products.Add(product);

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

    public ProductResponseDto? GetById(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return null;
        }
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

    public List<ProductResponseDto> GetAll()
    {
        return _products.Select(p => new ProductResponseDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            StockQuantity = p.StockQuantity,
            CreatedAt = p.CreatedAt
        }).ToList();
    }

    public ProductResponseDto? Update(int id, UpdateProductDto req)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return null;
        }

        product.Name = req.Name;
        product.Description = req.Description;
        product.Price = req.Price;
        product.StockQuantity = req.StockQuantity;

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

    public bool Delete(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return false;
        }

        _products.Remove(product);

        return true;
    }
}


public class OrderService
{
    private readonly ILogger<OrderService> _logger;

    public OrderService(ILogger<OrderService> logger)
    {
        _logger = logger;
    }

    public async Task ProcessOrderAsync(
        int orderId,
        int userId)
    {
using (_logger.BeginScope(new Dictionary<string, object>
{
    ["OrderId"] = orderId,
    ["UserId"] = userId
}))
{
    _logger.LogInformation("Starting order processing");
    _logger.LogInformation("Validating order");
    _logger.LogInformation("Order processed successfully");
}
    }
}