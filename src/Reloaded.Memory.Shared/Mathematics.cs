namespace Reloaded.Memory.Shared
{
    public class Mathematics
    {
        public static int MegaBytesToBytes(int megaBytes)  => megaBytes * 1000 * 1000;
        public static int BytesToStructCount<T>(int bytes) => bytes / Struct.GetSize<T>(true);
    }
}
