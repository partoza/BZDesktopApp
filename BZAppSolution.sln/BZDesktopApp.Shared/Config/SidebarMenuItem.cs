public class SidebarMenuItem
{
    public string Title { get; set; } = "";
    public string? Uri { get; set; }
    public string? IconSvg { get; set; }
    public List<string>? Roles { get; set; }
    public List<SidebarMenuItem>? Children { get; set; }
}
