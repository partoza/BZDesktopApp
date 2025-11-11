using BZDesktopApp.Shared;
using BZDesktopApp.Api.Services.Interfaces;

namespace BZDesktopApp.Facades
{
    public class CategoriesFacade
    {
        private readonly ICategoryService _categoryService;

        public CategoriesFacade(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // Equivalent to GET /api/categories
        public async Task<CategorySummaryDto> GetCategoriesAsync(string? search = null, string? type = null, string? status = null)
        {
            return await _categoryService.GetCategoriesAsync(search, type, status);
        }

        // Equivalent to POST /api/categories
        public async Task<CategoryDto> CreateCategoryAsync(CategoryForm form, int userId)
        {
            return await _categoryService.CreateCategoryAsync(form, userId);
        }

        // Equivalent to PUT /api/categories/{id}
        public async Task<CategoryDto?> UpdateCategoryAsync(long id, CategoryForm form)
        {
            return await _categoryService.UpdateCategoryAsync(id, form);
        }
    }
}
