// BZDesktopApp.Api/Models/Product.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BZDesktopApp.Api.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ProductId { get; set; }            

        [Required]
        [MaxLength(250)]
        public string ProductName { get; set; } = string.Empty;

        // Relationships (nullable foreign keys)
        public long? CategoryId { get; set; }
        public long? SubCategoryId { get; set; }
        public long? BrandId { get; set; }

        // Navigation
        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }

        [ForeignKey(nameof(SubCategoryId))]
        public Category? SubCategory { get; set; }

        [ForeignKey(nameof(BrandId))]
        public Brand? Brand { get; set; }

        // General product info
        [Column(TypeName = "nvarchar(max)")]
        public string? Image { get; set; }

        public bool ActiveStatus { get; set; } = true;

        // Audit fields
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }

        [ForeignKey(nameof(CreatedById))]
        public Employee? CreatedBy { get; set; }

        [ForeignKey(nameof(UpdatedById))]
        public Employee? UpdatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
