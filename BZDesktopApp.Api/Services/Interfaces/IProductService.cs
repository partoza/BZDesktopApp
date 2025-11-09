using BZDesktopApp.Shared.Dtos;

namespace BZDesktopApp.Api.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductSummaryDto> GetProductsAsync(string? search = null, string? category = null, string? brand = null, string? status = null);
        Task<ProductDto> CreateProductAsync(ProductForm form, long userId);
        Task<ProductDto?> UpdateProductAsync(long id, ProductForm form, long userId);

        // Brand management
        Task<List<BrandDto>> GetBrandsAsync();
        Task<BrandDto> CreateBrandAsync(string name);
    }
}
