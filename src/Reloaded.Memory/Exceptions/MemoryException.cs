using System.Diagnostics.CodeAnalysis;

namespace Reloaded.Memory.Exceptions;

/// <inheritdoc />
[ExcludeFromCodeCoverage]
public class MemoryException : Exception
{
    /// <inheritdoc />
    public MemoryException(string message) : base(message) { }
}
