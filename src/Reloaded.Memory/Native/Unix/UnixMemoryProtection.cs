namespace Reloaded.Memory.Native.Unix;

/// <summary>
///     Memory protection flags.
/// </summary>
[Flags]
public enum UnixMemoryProtection
{
    /// <summary>
    ///     No access.
    /// </summary>
    PROT_NONE = 0x0,

    /// <summary>
    ///     Read access.
    /// </summary>
    PROT_READ = 0x1,

    /// <summary>
    ///     Write access.
    /// </summary>
    PROT_WRITE = 0x2,

    /// <summary>
    ///     Execute access.
    /// </summary>
    PROT_EXEC = 0x4
}
