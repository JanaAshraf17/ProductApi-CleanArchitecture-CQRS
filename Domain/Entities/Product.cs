using System.ComponentModel.DataAnnotations.Schema;

namespace ProductApi_CleanArchitecture_CQRS.Domain.Entities
{
    [Table("products")]
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
