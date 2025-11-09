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
