using MediatR;
using ProductApi_CleanArchitecture_CQRS.Infrastructure.Persistence;

namespace ProductApi_CleanArchitecture_CQRS.Application.Products.Commands
{
    // update
    public record UpdateProductCommand(int Id, string Name, decimal Price) : IRequest<bool>;

    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly AppDbContext _context;
        public UpdateProductHandler(AppDbContext context) => _context = context;

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(new object[] { request.Id }, cancellationToken);
            if (product == null) return false;

            product.Name = request.Name;
            product.Price = request.Price;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

    // delete
    public record DeleteProductCommand(int Id) : IRequest<bool>;

    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly AppDbContext _context;
        public DeleteProductHandler(AppDbContext context) => _context = context;

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(new object[] { request.Id }, cancellationToken);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}