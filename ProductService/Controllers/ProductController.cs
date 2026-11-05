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
    public IActionResult CreateProduct(CreateProductDto req)
    {
        var product = _productService.Create(req);
        return CreatedAtAction(
            nameof(GetProduct),
            new { id = product.Id },
            product
        );
    }

    [HttpGet("{id}")]
    public IActionResult GetProduct(int id)
    {
        var product = _productService.GetById(id);
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
    public IActionResult GetAllProducts()
    {
        return Ok(_productService.GetAll());
    }

    [HttpPut("{id}")]
    public IActionResult UpdateProduct(int id, UpdateProductDto req)
    {
        var product = _productService.Update(id, req);
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
    public IActionResult DeleteProduct(int id)
    {
        var isDeleted = _productService.Delete(id);
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