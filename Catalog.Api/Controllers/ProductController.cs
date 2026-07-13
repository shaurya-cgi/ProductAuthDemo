using System;
using Catalog.Api.Dtos;
using Catalog.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetAll()
    {
        var products = await _productService.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<Product>>> SearchByCategory([FromQuery] string category)
    {
        var products = await _productService.SearchByCategoryAsync(category);
        return Ok(products);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Create([FromBody] CreateProductDto createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var product = new Product
        {
            Name = createDto.Name,
            Category = createDto.Category,
            Price = createDto.Price,
            Stock = createDto.Stock,
            CreatedDate = DateTime.UtcNow
        };

        var createdProduct = await _productService.CreateAsync(product);
        return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto updateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingProduct = await _productService.GetByIdAsync(id);
        if (existingProduct == null)
            return NotFound();

        existingProduct.Name = updateDto.Name;
        existingProduct.Category = updateDto.Category;
        existingProduct.Price = updateDto.Price;
        existingProduct.Stock = updateDto.Stock;
        existingProduct.UpdatedDate = DateTime.UtcNow;

        var result = await _productService.UpdateAsync(existingProduct);
        if (!result)
            return BadRequest("Failed to update product");

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _productService.DeleteAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
}