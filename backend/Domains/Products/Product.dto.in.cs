namespace Domains.Products
{
    public class ProductDtoIn
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        
    }
}