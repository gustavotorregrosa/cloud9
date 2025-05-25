namespace Domains.Products
{
    public class ProductDtoOut : ProductDtoIn
    {
        public Guid Id { get; set; }
        public string? CategoryName { get; set; } = string.Empty;
        
    }
}