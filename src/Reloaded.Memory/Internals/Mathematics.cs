namespace Reloaded.Memory.Internals;

internal static class Mathematics
{
    internal static int RoundUp(int number, int multiple)
    {
        if (multiple == 0)
            return number;

        var remainder = number % multiple;
        if (remainder == 0)
            return number;

        return number + multiple - remainder;
    }
}
