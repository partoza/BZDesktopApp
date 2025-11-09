using BZDesktopApp.Api.Data;
using BZDesktopApp.Api.Models;
using BZDesktopApp.Shared;
using BZDesktopApp.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BZDesktopApp.Api.Services;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;

    public CategoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CategorySummaryDto> GetCategoriesAsync(string? search = null, string? type = null, string? status = null)
    {
        var allCategories = await _context.Categories.AsNoTracking().ToListAsync();

        // Full summary for cards (unfiltered)
        var fullSummary = new CategorySummaryDto
        {
            TotalCount = allCategories.Count,
            ActiveCount = allCategories.Count(c => c.Status == "Active"),
            InactiveCount = allCategories.Count(c => c.Status == "Inactive"),
            MainActiveCount = allCategories.Count(c => c.Status == "Active" && c.ParentId == null),
            SubActiveCount = allCategories.Count(c => c.Status == "Active" && c.ParentId != null),
            MainInactiveCount = allCategories.Count(c => c.Status == "Inactive" && c.ParentId == null),
            SubInactiveCount = allCategories.Count(c => c.Status == "Inactive" && c.ParentId != null),
            ProductCount = allCategories.Count(c => c.CategoryType == "Product"),
            ServiceCount = allCategories.Count(c => c.CategoryType == "Service")
        };

        // Now apply filters for table only
        var query = allCategories.AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(c => c.Name.Contains(search));
        if (!string.IsNullOrWhiteSpace(type) && type != "all")
            query = query.Where(c => c.CategoryType == type);
        if (!string.IsNullOrWhiteSpace(status) && status != "all")
            query = query.Where(c => c.Status == status);

        var filteredCategories = query.ToList();

        // Attach filtered categories to summary
        fullSummary.Categories = filteredCategories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            CategoryType = c.CategoryType,
            ParentId = c.ParentId,
            Description = c.Description,
            Status = c.Status
        }).ToList();

        return fullSummary;
    }




    public async Task<CategoryDto> CreateCategoryAsync(CategoryForm form, int userId)
    {
        var category = new Category
        {
            Name = form.Name,
            CategoryType = form.CategoryType,
            ParentId = form.ParentId,
            Description = form.Description,
            Status = form.IsActive ? "Active" : "Inactive",
            CreatedAt = DateTime.UtcNow,
            CreatedById = userId
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            CategoryType = category.CategoryType,
            ParentId = category.ParentId,
            Description = category.Description,
            Status = category.Status
        };
    }

    public async Task<CategoryDto?> UpdateCategoryAsync(long id, CategoryForm form)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return null;

        category.Name = form.Name;
        category.CategoryType = form.CategoryType;
        category.ParentId = form.ParentId;
        category.Description = form.Description;
        category.Status = form.IsActive ? "Active" : "Inactive";
        category.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            CategoryType = category.CategoryType,
            ParentId = category.ParentId,
            Description = category.Description,
            Status = category.Status
        };
    }
}
