using Domains.Products;

public class SendProductDtoKafka : SendProductDtoIn
{
    public List<ProductDtoOut> Products { get; set; } = new List<ProductDtoOut>();
}

