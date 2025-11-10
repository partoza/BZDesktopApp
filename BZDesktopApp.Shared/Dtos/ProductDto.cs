using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BZDesktopApp.Shared.Dtos
{
    public class ProductDto
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;

        // Category / Subcategory
        public long? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public long? SubCategoryId { get; set; }
        public string? SubCategoryName { get; set; }

        // Brand
        public long? BrandId { get; set; }
        public string? BrandName { get; set; }

        // Description / Promo
        public string? Description { get; set; }
        public string? PromoCode { get; set; }

        // ✅ Image
        // Database stores only the filename or relative path (e.g. "uploads/products/img123.jpg")
        public string? ImageUrl { get; set; }

        // Active / Audit
        public bool ActiveStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    // ProductForm used by Create/Update APIs / Blazor form
    public class ProductForm
    {
        [Required(ErrorMessage = "Product name is required")]
        public string ProductName { get; set; } = string.Empty;

        public string? Description { get; set; }
        public string? PromoCode { get; set; }

        [Required(ErrorMessage = "Please select a category")]
        public long? CategoryId { get; set; }
        public long? SubCategoryId { get; set; }
        public long? BrandId { get; set; }

        // ✅ Image handling
        public string? ImageUrl { get; set; }      // stored filename or path
        public IFormFile? ImageFile { get; set; }  // actual uploaded file

        public bool ActiveStatus { get; set; } = true;
    }


    public class ProductSummaryDto
    {
        public int TotalCount { get; set; }
        public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }
        public List<ProductDto> Products { get; set; } = new();
    }

    public class BrandDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class BrandForm
    {
        [Required(ErrorMessage = "Brand name is required")]
        public string Name { get; set; } = string.Empty;
    }
}
