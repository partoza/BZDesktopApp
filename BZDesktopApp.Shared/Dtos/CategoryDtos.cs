// BZDesktopApp.Shared/CategoryDtos.cs
public class CategoryForm
{
    public string Name { get; set; } = string.Empty;
    public string CategoryType { get; set; } = "Product";
    public long? ParentId { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}

public class CategoryDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CategoryType { get; set; } = "Product";
    public long? ParentId { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; } = "Active";
}

public class CategorySummaryDto
{
    public List<CategoryDto> Categories { get; set; } = new();
    public int TotalCount { get; set; }
    public int ActiveCount { get; set; }
    public int InactiveCount { get; set; }
    public int MainActiveCount { get; set; }
    public int SubActiveCount { get; set; }
    public int MainInactiveCount { get; set; }
    public int SubInactiveCount { get; set; }
    public int ProductCount { get; set; }
    public int ServiceCount { get; set; }
}
