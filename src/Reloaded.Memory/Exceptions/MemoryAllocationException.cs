using System.Diagnostics.CodeAnalysis;

namespace Reloaded.Memory.Exceptions;

/// <inheritdoc />
[PublicAPI]
[ExcludeFromCodeCoverage]
public class MemoryAllocationException : Exception
{
    /// <inheritdoc />
    public MemoryAllocationException() { }

    /// <inheritdoc />
    public MemoryAllocationException(string message) : base(message) { }

    /// <inheritdoc />
    public MemoryAllocationException(string message, Exception innerException) : base(message, innerException) { }
}
