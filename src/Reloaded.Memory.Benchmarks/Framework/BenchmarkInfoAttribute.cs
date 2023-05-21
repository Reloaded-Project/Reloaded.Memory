namespace Reloaded.Memory.Benchmarks.Framework;

public class BenchmarkInfoAttribute : Attribute
{
    public string? Name { get; set; }
    public string? Description { get; set; }

    public string? Category { get; set; }

    public BenchmarkInfoAttribute(string? name, string? description, string? category = null)
    {
        Name = name;
        Description = description;
        Category = category;
    }
}
