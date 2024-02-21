using System.Diagnostics.CodeAnalysis;

namespace Reloaded.Memory.Exceptions;

/// <inheritdoc />
[PublicAPI]
[ExcludeFromCodeCoverage]
public class MemoryException : Exception
{
    /// <inheritdoc />
    public MemoryException() { }

    /// <inheritdoc />
    public MemoryException(string message) : base(message) { }

    /// <inheritdoc />
    public MemoryException(string message, Exception innerException) : base(message, innerException) { }
}
