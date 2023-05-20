namespace Reloaded.Memory.Interfaces;

/// <summary>
///     Structures implementing this interface can reverse their own endian.
/// </summary>
public interface ICanReverseEndian
{
    /// <summary>
    ///     Reverses the endian of the current structure.
    /// </summary>
    void ReverseEndian();
}
