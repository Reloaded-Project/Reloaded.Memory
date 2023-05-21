using System.Diagnostics.CodeAnalysis;

namespace Reloaded.Memory.Exceptions;

/// <inheritdoc />
[ExcludeFromCodeCoverage]
public class MemoryPermissionException : Exception
{
    /// <inheritdoc />
    public MemoryPermissionException(string message) : base(message) { }
}
