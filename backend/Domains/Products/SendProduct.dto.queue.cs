using Domains.Products;

public class SendProductDtoQueue : SendProductDtoIn
{
    public List<ProductDtoOut> Products { get; set; } = new List<ProductDtoOut>();
}

