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
        public ProductsController(ISender sender) => _sender = sender;

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct) =>
            Ok(await _sender.Send(new GetProductsQuery(), ct));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await _sender.Send(new GetProductByIdQuery(id), ct);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto, CancellationToken ct)
        {
            var id = await _sender.Send(new CreateProductCommand(dto.Name, dto.Price), ct);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto, CancellationToken ct)
        {
            var success = await _sender.Send(new UpdateProductCommand(id, dto.Name, dto.Price), ct);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var success = await _sender.Send(new DeleteProductCommand(id), ct);
            return success ? NoContent() : NotFound();
        }
    }
}