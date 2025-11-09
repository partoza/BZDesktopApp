using BZDesktopApp.Api.Data;
using BZDesktopApp.Api.Models;
using BZDesktopApp.Api.Services.Interfaces;
using BZDesktopApp.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BZDesktopApp.Api.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
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
            UpdatedAt = p.UpdatedAt
        }).ToList();

        return summary;
    }




    // -----------------------------
    // CREATE PRODUCT
    // -----------------------------
    public async Task<ProductDto> CreateProductAsync(ProductForm form, long userId)
    {
        var product = new Product
        {
            ProductName = form.ProductName,
            CategoryId = form.CategoryId,
            SubCategoryId = form.SubCategoryId,
            BrandId = form.BrandId,
            Image = form.ImageBase64,
            ActiveStatus = form.ActiveStatus,
            CreatedAt = DateTime.UtcNow,
            CreatedById = userId
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return new ProductDto
        {
            ProductId = product.ProductId,
            ProductName = product.ProductName,
            CategoryId = product.CategoryId,
            BrandId = product.BrandId,
            ActiveStatus = product.ActiveStatus
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
        product.Image = form.Image;
        product.ActiveStatus = form.ActiveStatus;
        product.UpdatedAt = DateTime.UtcNow;
        product.UpdatedById = userId;

        await _context.SaveChangesAsync();

        return new ProductDto
        {
            ProductId = product.ProductId,
            ProductName = product.ProductName,
            CategoryId = product.CategoryId,
            BrandId = product.BrandId,
            ActiveStatus = product.ActiveStatus
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
