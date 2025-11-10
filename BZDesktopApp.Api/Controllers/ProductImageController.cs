using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace BZDesktopApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImagesController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public ProductImagesController(IWebHostEnvironment env)
        {
            _env = env;
        }

        // GET: api/ProductImages/{filename}
        [HttpGet("{filename}")]
        public IActionResult GetImage(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                return BadRequest("Filename is required.");

            // Path to the uploaded images folder
            var uploadsFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads", "products");
            var filePath = Path.Combine(uploadsFolder, filename);

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            // Determine content type
            var contentType = "image/png"; // default
            var ext = Path.GetExtension(filePath).ToLower();
            if (ext == ".jpg" || ext == ".jpeg") contentType = "image/jpeg";
            else if (ext == ".gif") contentType = "image/gif";

            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, contentType);
        }
    }
}
