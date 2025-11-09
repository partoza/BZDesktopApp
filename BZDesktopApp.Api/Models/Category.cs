using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BZDesktopApp.Api.Models
{
    public class Category
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }

        public string? Description { get; set; }

        // Self-referencing parent
        public long? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public Category? Parent { get; set; }

        public ICollection<Category>? SubCategories { get; set; }

        [Required]
        public required string CategoryType { get; set; }

        [Required]
        public string Status { get; set; } = "Active";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }
    }
}
