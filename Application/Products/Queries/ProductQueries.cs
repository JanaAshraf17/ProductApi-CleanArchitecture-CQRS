using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductApi_CleanArchitecture_CQRS.Application.DTOs;
using ProductApi_CleanArchitecture_CQRS.Infrastructure.Persistence;

namespace ProductApi_CleanArchitecture_CQRS.Application.Products.Queries
{
     
    public record GetProductsQuery() : IRequest<List<ProductDto>>;

    public class GetProductsHandler : IRequestHandler<GetProductsQuery, List<ProductDto>>
    {
        private readonly AppDbContext _context;
        public GetProductsHandler(AppDbContext context) => _context = context;

        public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products
                .AsNoTracking()
                .Select(p => new ProductDto(p.Id, p.Name, p.Price))
                .ToListAsync(cancellationToken);
        }
    }

     
    public record GetProductByIdQuery(int Id) : IRequest<ProductDto?>;

    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        private readonly AppDbContext _context;
        public GetProductByIdHandler(AppDbContext context) => _context = context;

        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var p = await _context.Products.FindAsync(new object[] { request.Id }, cancellationToken);
            return p == null ? null : new ProductDto(p.Id, p.Name, p.Price);
        }
    }
}