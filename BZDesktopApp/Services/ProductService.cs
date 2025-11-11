using BZDesktopApp.Api.Models;
using BZDesktopApp.Api.Services.Interfaces;
using BZDesktopApp.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BZDesktopApp.Api.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    // -----------------------------
    // PRODUCT SUMMARY + FILTERS
    // -----------------------------
    public async Task<ProductSummaryDto> GetProductsAsync(
    string? search = null,
    string? category = null,
    string? brand = null,
    string? status = null)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        // Fetch all products once to calculate summary
        var allProductsQuery = _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .AsNoTracking();

        var allProducts = await allProductsQuery.ToListAsync();

        stopwatch.Stop();
        Console.WriteLine($"[GetProductsAsync] EF Core query took {stopwatch.ElapsedMilliseconds} ms");

        // Build summary from all products (unfiltered)
        var summary = new ProductSummaryDto
        {
            TotalCount = allProducts.Count,
            ActiveCount = allProducts.Count(p => p.ActiveStatus),
            InactiveCount = allProducts.Count(p => !p.ActiveStatus)
        };

        // Apply filters for table only
        var filteredQuery = allProducts.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            filteredQuery = filteredQuery.Where(p => p.ProductName.Contains(search));

        if (!string.IsNullOrWhiteSpace(category) && category != "all")
            filteredQuery = filteredQuery.Where(p => p.Category != null && p.Category.Name == category);

        if (!string.IsNullOrWhiteSpace(brand) && brand != "all")
            filteredQuery = filteredQuery.Where(p => p.Brand != null && p.Brand.Name == brand);

        if (!string.IsNullOrWhiteSpace(status) && status != "all")
            filteredQuery = status == "Active"
                ? filteredQuery.Where(p => p.ActiveStatus)
                : filteredQuery.Where(p => !p.ActiveStatus);

        var filteredProducts = filteredQuery.ToList();

        summary.Products = filteredProducts.Select(p => new ProductDto
        {
            ProductId = p.ProductId,
            ProductName = p.ProductName,
            CategoryId = p.CategoryId,
            CategoryName = p.Category?.Name,
            SubCategoryId = p.SubCategoryId,
            SubCategoryName = p.SubCategory?.Name,
            BrandId = p.BrandId,
            BrandName = p.Brand?.Name,
            ActiveStatus = p.ActiveStatus,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt,
            ImageUrl = p.Image  // <-- assign the URL from DB
        }).ToList();

        return summary;
    }


    // -----------------------------
    // CREATE PRODUCT
    // -----------------------------
    public async Task<ProductDto> CreateProductAsync(ProductForm form, long userId)
    {
        string? imageUrl = null;

        // Determine upload folder safely in MAUI Blazor wwwroot
        var webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var uploadsFolder = Path.Combine(webRoot, "uploads", "products");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        // Handle uploaded image
        if (form.ImageFile != null)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(form.ImageFile.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
                await form.ImageFile.CopyToAsync(stream);

            // Store relative path only
            imageUrl = $"/uploads/products/{fileName}";
        }

        var product = new Product
        {
            ProductName = form.ProductName,
            CategoryId = form.CategoryId,
            SubCategoryId = form.SubCategoryId,
            BrandId = form.BrandId,
            Image = imageUrl,
            ActiveStatus = form.ActiveStatus,
            CreatedAt = DateTime.UtcNow,
            CreatedById = userId
        };

        _context.Products.Add(product);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Error saving product: " + ex.Message);
            if (ex.InnerException != null)
                Console.WriteLine("➡ Inner Exception: " + ex.InnerException.Message);
            if (ex.InnerException?.InnerException != null)
                Console.WriteLine("➡➡ Deep Inner: " + ex.InnerException.InnerException.Message);

            // Optional: rethrow with detailed info for debugging (comment this out in prod)
            throw new Exception($"Database save failed: {ex.InnerException?.Message ?? ex.Message}");
        }

        return new ProductDto
        {
            ProductId = product.ProductId,
            ProductName = product.ProductName,
            CategoryId = product.CategoryId,
            SubCategoryId = product.SubCategoryId,
            BrandId = product.BrandId,
            ImageUrl = product.Image,
            ActiveStatus = product.ActiveStatus,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }

    // -----------------------------
    // UPDATE PRODUCT
    // -----------------------------
    public async Task<ProductDto?> UpdateProductAsync(long id, ProductForm form, long userId)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return null;

        product.ProductName = form.ProductName;
        product.CategoryId = form.CategoryId;
        product.SubCategoryId = form.SubCategoryId;
        product.BrandId = form.BrandId;
        product.ActiveStatus = form.ActiveStatus;
        product.UpdatedAt = DateTime.UtcNow;
        product.UpdatedById = userId;

        var webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var uploadsFolder = Path.Combine(webRoot, "uploads", "products");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        if (form.ImageFile != null)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(form.ImageFile.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
                await form.ImageFile.CopyToAsync(stream);

            product.Image = $"/uploads/products/{fileName}"; // relative path
        }

        await _context.SaveChangesAsync();

        return new ProductDto
        {
            ProductId = product.ProductId,
            ProductName = product.ProductName,
            CategoryId = product.CategoryId,
            SubCategoryId = product.SubCategoryId,
            BrandId = product.BrandId,
            ImageUrl = product.Image,
            ActiveStatus = product.ActiveStatus,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }


    // -----------------------------
    // BRANDS
    // -----------------------------
    public async Task<List<BrandDto>> GetBrandsAsync()
    {
        var brands = await _context.Brands.AsNoTracking().ToListAsync();
        return brands.Select(b => new BrandDto
        {
            Id = b.Id,
            Name = b.Name
        }).ToList();
    }

    public async Task<BrandDto> CreateBrandAsync(string name)
    {
        var brand = new Brand { Name = name };
        _context.Brands.Add(brand);
        await _context.SaveChangesAsync();

        return new BrandDto
        {
            Id = brand.Id,
            Name = brand.Name
        };
    }
}
