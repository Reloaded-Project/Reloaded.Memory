using System.Reflection;

namespace Reloaded.Memory.Benchmarks.Framework;

/// <summary>
///     A benchmark that can be ran by the user.
/// </summary>
public record SelectableBenchmark
{
    /// <summary>
    ///     Type behind the benchmark.
    /// </summary>
    public Type Type { get; init; }

    /// <summary>
    ///     Name of the benchmark.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    ///     Description of the benchmark.
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    ///     Category of the benchmark.
    /// </summary>
    public string? Category { get; init; }

    public SelectableBenchmark(Type type)
    {
        Type = type;
        Name = Type.Name;
        Description = string.Empty;

        var info = Type.GetCustomAttribute<BenchmarkInfoAttribute>(true);
        if (info == null)
            return;

        Name = info.Name ?? Name;
        Description = info.Description ?? Description;
        Category = info.Category ?? Category;
    }
}
