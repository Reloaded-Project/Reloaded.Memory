using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage] // Taken from Runtime
internal static class Utf8Utility
{
    /// <summary>
    ///     Given a UInt32 that represents four ASCII UTF-8 characters, returns the invariant
    ///     lowercase representation of those characters. Requires the input value to contain
    ///     four ASCII UTF-8 characters in machine endianness.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <remarks>
    ///     This is a branchless implementation.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static uint ConvertAllAsciiBytesInUInt32ToLowercase(uint value)
    {
        // the 0x80 bit of each byte of 'lowerIndicator' will be set iff the word has value >= 'A'
        var lowerIndicator = value + 0x8080_8080u - 0x4141_4141u;

        // the 0x80 bit of each byte of 'upperIndicator' will be set iff the word has value > 'Z'
        var upperIndicator = value + 0x8080_8080u - 0x5B5B_5B5Bu;

        // the 0x80 bit of each byte of 'combinedIndicator' will be set iff the word has value >= 'A' and <= 'Z'
        var combinedIndicator = lowerIndicator ^ upperIndicator;

        // the 0x20 bit of each byte of 'mask' will be set iff the word has value >= 'A' and <= 'Z'
        var mask = (combinedIndicator & 0x8080_8080u) >> 2;

        return value ^ mask; // bit flip uppercase letters [A-Z] => [a-z]
    }

    /// <summary>
    ///     Given a UInt32 that represents four ASCII UTF-8 characters, returns the invariant
    ///     uppercase representation of those characters. Requires the input value to contain
    ///     four ASCII UTF-8 characters in machine endianness.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <remarks>
    ///     This is a branchless implementation.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static uint ConvertAllAsciiBytesInUInt32ToUppercase(uint value)
    {
        // the 0x80 bit of each byte of 'lowerIndicator' will be set iff the word has value >= 'a'
        var lowerIndicator = value + 0x8080_8080u - 0x6161_6161u;

        // the 0x80 bit of each byte of 'upperIndicator' will be set iff the word has value > 'z'
        var upperIndicator = value + 0x8080_8080u - 0x7B7B_7B7Bu;

        // the 0x80 bit of each byte of 'combinedIndicator' will be set iff the word has value >= 'a' and <= 'z'
        var combinedIndicator = lowerIndicator ^ upperIndicator;

        // the 0x20 bit of each byte of 'mask' will be set iff the word has value >= 'a' and <= 'z'
        var mask = (combinedIndicator & 0x8080_8080u) >> 2;

        return value ^ mask; // bit flip lowercase letters [a-z] => [A-Z]
    }

    /// <summary>
    ///     Given a UInt64 that represents eight ASCII UTF-8 characters, returns the invariant
    ///     uppercase representation of those characters. Requires the input value to contain
    ///     eight ASCII UTF-8 characters in machine endianness.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <remarks>
    ///     This is a branchless implementation.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ulong ConvertAllAsciiBytesInUInt64ToUppercase(ulong value)
    {
        // the 0x80 bit of each byte of 'lowerIndicator' will be set iff the word has value >= 'a'
        var lowerIndicator = value + 0x8080_8080_8080_8080ul - 0x6161_6161_6161_6161ul;

        // the 0x80 bit of each byte of 'upperIndicator' will be set iff the word has value > 'z'
        var upperIndicator = value + 0x8080_8080_8080_8080ul - 0x7B7B_7B7B_7B7B_7B7Bul;

        // the 0x80 bit of each byte of 'combinedIndicator' will be set iff the word has value >= 'a' and <= 'z'
        var combinedIndicator = lowerIndicator ^ upperIndicator;

        // the 0x20 bit of each byte of 'mask' will be set iff the word has value >= 'a' and <= 'z'
        var mask = (combinedIndicator & 0x8080_8080_8080_8080ul) >> 2;

        return value ^ mask; // bit flip lowercase letters [a-z] => [A-Z]
    }

    /// <summary>
    ///     Given a UInt64 that represents eight ASCII UTF-8 characters, returns the invariant
    ///     uppercase representation of those characters. Requires the input value to contain
    ///     eight ASCII UTF-8 characters in machine endianness.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <remarks>
    ///     This is a branchless implementation.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ulong ConvertAllAsciiBytesInUInt64ToLowercase(ulong value)
    {
        // the 0x80 bit of each byte of 'lowerIndicator' will be set iff the word has value >= 'A'
        var lowerIndicator = value + 0x8080_8080_8080_8080ul - 0x4141_4141_4141_4141ul;

        // the 0x80 bit of each byte of 'upperIndicator' will be set iff the word has value > 'Z'
        var upperIndicator = value + 0x8080_8080_8080_8080ul - 0x5B5B_5B5B_5B5B_5B5Bul;

        // the 0x80 bit of each byte of 'combinedIndicator' will be set iff the word has value >= 'a' and <= 'z'
        var combinedIndicator = lowerIndicator ^ upperIndicator;

        // the 0x20 bit of each byte of 'mask' will be set iff the word has value >= 'a' and <= 'z'
        var mask = (combinedIndicator & 0x8080_8080_8080_8080ul) >> 2;

        return value ^ mask; // bit flip uppercase letters [A-Z] => [a-z]
    }
}
