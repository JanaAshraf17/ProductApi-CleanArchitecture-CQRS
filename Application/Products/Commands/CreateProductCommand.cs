using MediatR;
using ProductApi_CleanArchitecture_CQRS.Domain.Entities;
using ProductApi_CleanArchitecture_CQRS.Infrastructure.Persistence;

namespace ProductApi_CleanArchitecture_CQRS.Application.Products.Commands
{
    public record CreateProductCommand(string Name, decimal Price) : IRequest<int>;

    public class CreateProductHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly AppDbContext _context;
        public CreateProductHandler(AppDbContext context) => _context = context;

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product { Name = request.Name, Price = request.Price };
            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);
            return product.Id;
        }
    }
}