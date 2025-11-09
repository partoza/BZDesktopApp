using BZDesktopApp.Shared;

namespace BZDesktopApp.Api.Services.Interfaces;

public interface ICategoryService
{
    Task<CategorySummaryDto> GetCategoriesAsync(string? search = null, string? type = null, string? status = null);
    Task<CategoryDto> CreateCategoryAsync(CategoryForm form, int userId);
    Task<CategoryDto?> UpdateCategoryAsync(long id, CategoryForm form);
}
