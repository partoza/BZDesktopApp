using BZDesktopApp.Api.Services.Interfaces;
using BZDesktopApp.Shared;
using BZDesktopApp.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BZDesktopApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(
        [FromQuery] string? search = null,
        [FromQuery] string? category = null,
        [FromQuery] string? brand = null,
        [FromQuery] string? status = null)
        {
            var summary = await _productService.GetProductsAsync(search, category, brand, status);
            return Ok(summary);
        }


        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductForm form)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized("User ID not found in token");

            var product = await _productService.CreateProductAsync(form, userId);
            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(long id, [FromBody] ProductForm form)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized("User ID not found in token");

            var product = await _productService.UpdateProductAsync(id, form, userId);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpGet("brands")]
        public async Task<IActionResult> GetBrands()
        {
            var brands = await _productService.GetBrandsAsync();
            return Ok(brands);
        }

        [HttpPost("brands")]
        public async Task<IActionResult> CreateBrand([FromBody] BrandForm form)
        {
            if (string.IsNullOrWhiteSpace(form.Name))
                return BadRequest("Brand name is required");

            var brand = await _productService.CreateBrandAsync(form.Name);
            return Ok(brand);
        }


    }
}
