using BZDesktopApp.Api.Services.Interfaces;
using BZDesktopApp.Shared;
using BZDesktopApp.Shared.Dtos;

namespace BZDesktopApp.Facades
{
    public class ProductsFacade
    {
        private readonly IProductService _productService;

        public ProductsFacade(IProductService productService)
        {
            _productService = productService;
        }

        // Equivalent to GET /api/products
        public async Task<ProductSummaryDto> GetProductsAsync(string? search = null, string? category = null, string? brand = null, string? status = null)
        {
            return await _productService.GetProductsAsync(search, category, brand, status);
        }

        // Equivalent to POST /api/products
        public async Task<ProductDto> CreateProductAsync(ProductForm form, int userId)
        {
            // Handle image saving inside the facade or a helper
            if (form.ImageFile != null && form.ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(FileSystem.AppDataDirectory, "products");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(form.ImageFile.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                    await form.ImageFile.CopyToAsync(stream);

                form.ImageUrl = filePath; // Or relative URL for UI
            }

            return await _productService.CreateProductAsync(form, userId);
        }

        

        // Brands
        public async Task<List<BrandDto>> GetBrandsAsync()
        {
            return await _productService.GetBrandsAsync();
        }

        public async Task<BrandDto> CreateBrandAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Brand name is required");

            return await _productService.CreateBrandAsync(name);
        }

        // Equivalent to PUT /api/products/{id}
        //public async Task<ProductDto?> UpdateProductAsync(long id, ProductForm form, int userId)
        //{
        //    if (form.ImageFile != null && form.ImageFile.Length > 0)
        //    {
        //        var uploadsFolder = Path.Combine(FileSystem.AppDataDirectory, "products");
        //        if (!Directory.Exists(uploadsFolder))
        //            Directory.CreateDirectory(uploadsFolder);

        //        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(form.ImageFile.FileName)}";
        //        var filePath = Path.Combine(uploadsFolder, fileName);

        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //            await form.ImageFile.CopyToAsync(stream);

        //        form.ImageUrl = filePath;
        //    }

        //    return await _productService.UpdateProductAsync(id, form, userId);
        //}

        // Equivalent to GET /api/products/{id}
        //public async Task<ProductDto?> GetProductByIdAsync(long id)
        //{
        //    return await _productService.GetProductByIdAsync(id);
        //}
    }
}
