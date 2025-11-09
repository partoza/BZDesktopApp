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

    public async Task<List<CategoryDto>> GetCategoriesAsync()
    {
        var categories = await _context.Categories.AsNoTracking().ToListAsync();
        return categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            CategoryType = c.CategoryType,
            ParentId = c.ParentId,
            Description = c.Description,
            Status = c.Status
        }).ToList();
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
