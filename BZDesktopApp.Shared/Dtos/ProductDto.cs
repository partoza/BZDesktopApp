using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BZDesktopApp.Shared.Dtos
{
    public class ProductDto
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;

        // Category / subcategory
        public long? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public long? SubCategoryId { get; set; }
        public string? SubCategoryName { get; set; }

        // Brand
        public long? BrandId { get; set; }
        public string? BrandName { get; set; }

        // Extra fields page expects
        public string? Description { get; set; }
        public string? PromoCode { get; set; }

        // Image
        public string? Image { get; set; }      // existing DB field
        public string? ImageUrl { get; set; }   // used by page for preview

        // Active / audit
        public bool ActiveStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    // ProductForm used by Create/Update APIs / Blazor form
    public class ProductForm
    {
        // Keep ProductName to match service and DB naming
        [Required(ErrorMessage = "Product name is required")]
        public string ProductName { get; set; } = string.Empty;

        // Extra form fields your page uses
        public string? Description { get; set; }
        public string? PromoCode { get; set; }

        // relationships
        [Required(ErrorMessage = "Please select a category")]
        public long? CategoryId { get; set; }    // main category
        public long? SubCategoryId { get; set; }
        public long? BrandId { get; set; }

        // Image upload / preview
        public string? Image { get; set; }        // persisted filename / path
        public string? ImageUrl { get; set; }     // preview url
        public string? ImageBase64 { get; set; }  // upload payload (optional)

        // Active flag
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
