using BZDesktopApp.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BZDesktopApp.Api.Services.Interfaces
{
    public interface IProductService
    {
        // Fetch products with optional filters
        Task<ProductSummaryDto> GetProductsAsync(string? search = null, string? category = null, string? brand = null, string? status = null);

        // Create / update products using the new ProductForm (supports file upload via IFormFile)
        Task<ProductDto> CreateProductAsync(ProductForm form, long userId);
        Task<ProductDto?> UpdateProductAsync(long id, ProductForm form, long userId);

        // Brand management
        Task<List<BrandDto>> GetBrandsAsync();
        Task<BrandDto> CreateBrandAsync(string name);
    }
}
