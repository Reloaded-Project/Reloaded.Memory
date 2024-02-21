using System.Diagnostics.CodeAnalysis;

namespace Reloaded.Memory.Exceptions;

/// <inheritdoc />
[PublicAPI]
[ExcludeFromCodeCoverage]
public class MemoryPermissionException : Exception
{
    /// <inheritdoc />
    public MemoryPermissionException() { }

    /// <inheritdoc />
    public MemoryPermissionException(string message) : base(message) { }

    /// <inheritdoc />
    public MemoryPermissionException(string message, Exception innerException) : base(message, innerException) { }
}
