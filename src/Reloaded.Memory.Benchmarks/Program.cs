// See https://aka.ms/new-console-template for more information

using System.Linq.Expressions;
using System.Reflection;
using BenchmarkDotNet.Running;
using Reloaded.Memory.Benchmarks.Framework;
using Spectre.Console;

SelectableBenchmark[] benchmarks = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(x => x.GetCustomAttribute<BenchmarkInfoAttribute>() != null)
    .Select(x => new SelectableBenchmark(x))
    .ToArray();

Console.WriteLine(
    "Note: Benchmarks can be ran directly by passing arguments in CLI, e.g. NexusMods.Benchmarks.dll 0 1\n");
if (args.Length > 0)
{
    foreach (var arg in args)
    {
        if (int.TryParse(arg, out var result) && result >= 0 && result < benchmarks.Length)
            BenchmarkRunner.Run(benchmarks[result].Type);
    }

    return;
}

while (true)
{
    PrintTable(benchmarks);
    Console.WriteLine("\nEnter any invalid number to exit.");

    var line = Console.ReadLine();
    if (int.TryParse(line, out var result))
    {
        if (result >= 0 && result < benchmarks.Length)
            BenchmarkRunner.Run(benchmarks[result].Type);
        else
            return;
    }
}

void PrintTable(SelectableBenchmark[] benches)
{
    var columns = new List<(string Name, Expression<Func<SelectableBenchmark, string>> Selector)>
    {
        ("Name", x => x.Name), ("Category", x => x.Category!), ("Description", x => x.Description)
    };

    // Filter out columns with no values
    List<(string Name, Expression<Func<SelectableBenchmark, string>> Selector)> filteredColumns = columns
        .Where(column => benches.Any(x => !string.IsNullOrEmpty(column.Selector.Compile().Invoke(x))))
        .ToList();

    // Create a table
    var table = new Table();

    // Add filtered columns
    foreach ((string Name, Expression<Func<SelectableBenchmark, string>> Selector) column in filteredColumns)
        table.AddColumn(column.Name);

    // Add rows
    for (var x = 0; x < benches.Length; x++)
    {
        SelectableBenchmark benchmark = benches[x];
        var rowValues = filteredColumns
            .Select(column => column.Selector.Compile().Invoke(benchmark))
            .ToArray();

        rowValues[0] = $"{x}. {rowValues[0]}";
        table.AddRow(rowValues);
    }

    // Render the table to the console
    AnsiConsole.Write(table);
}
