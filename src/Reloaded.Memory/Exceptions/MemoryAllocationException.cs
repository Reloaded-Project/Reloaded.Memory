using System.Diagnostics.CodeAnalysis;

namespace Reloaded.Memory.Exceptions;

/// <inheritdoc />
[ExcludeFromCodeCoverage]
public class MemoryAllocationException : Exception
{
    /// <inheritdoc />
    public MemoryAllocationException(string message) : base(message) { }
}
