using System.Text.Json;
using backend.Shared;
using Domains.Products;
using Microsoft.Extensions.Caching.Distributed;

namespace backend.Domains.Products;

class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly KafkaService? _kafkaService;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public ProductService(IProductRepository productRepository, KafkaService kafkaService)
    {
        _productRepository = productRepository;
        _kafkaService = kafkaService;
    }

    public async Task<IEnumerable<ProductDtoOut>> GetAllAsync()
    {
        return (await _productRepository.GetAllAsync()).ToList().Select(product => MapToDto(product));
    }

    public async Task<ProductDtoOut> GetByIdAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product == null)
        {
            throw new Exception("Product not found.");
        }

        return MapToDto(product);
    }

    public async Task SendEmailNotificationAsync(string email)
    {
        List<ProductDtoOut> products = (await _productRepository.GetAllAsync()).Select(product => MapToDto(product)).ToList();

        SendProductDtoKafka sendProductDtoKafka = new SendProductDtoKafka
        {
            Email = email,
            Products = products
        };

        _kafkaService?.SendProductMessageAsync(JsonSerializer.Serialize(sendProductDtoKafka));

        Console.WriteLine($"Email notification sent to {email} with {products.Count} products.");
        Console.WriteLine($"_kafkaService: {_kafkaService != null}");
    }

    public async Task<ProductDtoOut> AddAsync(ProductDtoIn productDto)
    {
        try
        {
            var product = MapToEntity(productDto);
            var productEntity = await _productRepository.AddAsync(product);

            if (productEntity == null)
            {
                throw new Exception("Failed to add the product.");
            }

            return MapToDto(productEntity);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while adding the product: {ex.Message}");
        }
    }

    public async Task<ProductDtoOut> UpdateAsync(ProductDtoIn productDto, Guid id)
    {
        try
        {
            var product = MapToEntity(productDto);
            product.Id = id;
            var productEntity = await _productRepository.UpdateAsync(product);

            if (productEntity == null)
            {
                throw new Exception("Product not found.");
            }

            return MapToDto(productEntity);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while updating the product: {ex.Message}");
        }
    }

    public async Task<ProductDtoOut> DeleteAsync(Guid id)
    {
        try
        {
            var productEntity = await _productRepository.DeleteAsync(id);
            if (productEntity == null)
            {
                throw new Exception("Product not found.");
            }

            return MapToDto(productEntity);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while deleting the product: {ex.Message}");
        }
    }

    private ProductDtoOut MapToDto(Product product)
    {
        return new ProductDtoOut
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            CategoryId = product.CategoryId,
            CategoryName = !string.IsNullOrEmpty(product.Category?.Name) ? product.Category.Name : string.Empty
        };
    }

    private Product MapToEntity(ProductDtoIn productDto)
    {
        return new Product
        {
            Id = productDto.Id ?? Guid.Empty,
            Name = productDto.Name,
            Description = productDto.Description,
            CategoryId = productDto.CategoryId
        };
    }
}
