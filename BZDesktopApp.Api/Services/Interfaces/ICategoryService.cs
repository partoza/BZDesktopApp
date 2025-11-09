using BZDesktopApp.Shared;

namespace BZDesktopApp.Api.Services.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetCategoriesAsync();
    Task<CategoryDto> CreateCategoryAsync(CategoryForm form, int userId);
    Task<CategoryDto?> UpdateCategoryAsync(long id, CategoryForm form);
}
