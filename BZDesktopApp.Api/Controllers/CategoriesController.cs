using BZDesktopApp.Shared;
using BZDesktopApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories([FromQuery] string? search = null, [FromQuery] string? type = null, [FromQuery] string? status = null)
    {
        var summary = await _categoryService.GetCategoriesAsync(search, type, status);
        return Ok(summary);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryForm form)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out int userId))
            return Unauthorized("User ID not found in token");

        var category = await _categoryService.CreateCategoryAsync(form, userId);
        return Ok(category);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(long id, [FromBody] CategoryForm form)
    {
        var category = await _categoryService.UpdateCategoryAsync(id, form);
        if (category == null) return NotFound();
        return Ok(category);
    }
}
