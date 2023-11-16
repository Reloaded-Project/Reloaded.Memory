using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage] // Taken from Runtime
internal static class UnicodeUtility
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsInRangeInclusive(uint value, uint lowerBound, uint upperBound) => (value - lowerBound) <= (upperBound - lowerBound);
}
