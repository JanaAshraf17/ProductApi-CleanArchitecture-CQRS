using System.ComponentModel.DataAnnotations;

namespace ProductApi_CleanArchitecture_CQRS.Application.DTOs
{
    public record ProductDto(int Id, string Name, decimal Price);

    public record CreateProductDto(
        [Required]
        [StringLength(100, MinimumLength = 1)]
        string Name,

        [Range(0.01, 1000000)]
        decimal Price
    );

    public record UpdateProductDto(
        [Required]
        [StringLength(100, MinimumLength = 1)]
        string Name,

        [Range(0.01, 1000000)]
        decimal Price
    );
}