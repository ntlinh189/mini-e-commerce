using Microsoft.AspNetCore.Mvc;
using ProductService.DTOs;
using ProductService.Services;

namespace ProductService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly ProductManagementService _productService;

    public ProductController(ProductManagementService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductDto req)
    {
        var product = await _productService.CreateAsync(req);
        return CreatedAtAction(
            nameof(GetProduct),
            new { id = product.Id },
            product
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound(new
            {
                message = $"Product with ID {id} not found."
            });
        }
        return Ok(product);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllProducts()
    {
        return Ok(await _productService.GetAllAsync());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, UpdateProductDto req)
    {
        var product = await _productService.UpdateAsync(id, req);
        if (product == null)
        {
            return NotFound(new
            {
                message = $"Product with ID {id} not found."
            });
        }

        return Ok(product);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var isDeleted = await _productService.DeleteAsync(id);
        if (!isDeleted)
        {
            return NotFound(new
            {
                message = $"Product with ID {id} not found."
            });
        }

        return NoContent();
    }
}