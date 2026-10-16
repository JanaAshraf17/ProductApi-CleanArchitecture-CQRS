using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductApi_CleanArchitecture_CQRS.Application.DTOs;
using ProductApi_CleanArchitecture_CQRS.Application.Products.Commands;
using ProductApi_CleanArchitecture_CQRS.Application.Products.Queries;

namespace ProductApi_CleanArchitecture_CQRS.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ISender sender, ILogger<ProductsController> logger)
        {
            _sender = sender;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            _logger.LogInformation("Fetching all products.");
            var result = await _sender.Send(new GetProductsQuery(), ct);
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            _logger.LogInformation("Fetching product with ID: {ProductId}", id);
            var result = await _sender.Send(new GetProductByIdQuery(id), ct);

            if (result == null)
            {
                _logger.LogWarning("Product with ID: {ProductId} was not found.", id);
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto, CancellationToken ct)
        {
            _logger.LogInformation("Creating a new product with Name: {ProductName} and Price: {ProductPrice}", dto.Name, dto.Price);

            var id = await _sender.Send(new CreateProductCommand(dto.Name, dto.Price), ct);

            _logger.LogInformation("Product successfully created with ID: {ProductId}", id);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto, CancellationToken ct)
        {
            _logger.LogInformation("Updating product with ID: {ProductId}", id);

            var success = await _sender.Send(new UpdateProductCommand(id, dto.Name, dto.Price), ct);

            if (!success)
            {
                _logger.LogWarning("Failed to update. Product with ID: {ProductId} was not found.", id);
                return NotFound();
            }

            _logger.LogInformation("Product with ID: {ProductId} updated successfully.", id);
            return NoContent();
        }
        

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            _logger.LogInformation("Deleting product with ID: {ProductId}", id);

            var success = await _sender.Send(new DeleteProductCommand(id), ct);

            if (!success)
            {
                _logger.LogWarning("Failed to delete. Product with ID: {ProductId} was not found.", id);
                return NotFound();
            }

            _logger.LogInformation("Product with ID: {ProductId} deleted successfully.", id);
            return NoContent();
        }
    }
}