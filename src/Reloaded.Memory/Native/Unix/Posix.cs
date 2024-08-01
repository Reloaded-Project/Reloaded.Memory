using Reloaded.Memory.Native.Windows;

namespace Reloaded.Memory.Native.Unix;

/// <summary>
///     Helpers for various POSIX related functions.
/// </summary>
// ReSharper disable once PartialTypeWithSinglePart
public static partial class Posix
{
    /// <summary>
    ///     Creates a new mapping in the virtual address space of the calling process.
    /// </summary>
    /// <param name="addr">A hint to the kernel about where the mapping should start. NULL is commonly used for no hint.</param>
    /// <param name="length">The length of the mapping (number of bytes to map).</param>
    /// <param name="prot">Memory protection flags. Can be a combination of PROT_READ, PROT_WRITE, and PROT_EXEC.</param>
    /// <param name="flags">Flags for the mapping. MAP_PRIVATE | MAP_ANONYMOUS is commonly used for anonymous private mappings.</param>
    /// <param name="fd">File descriptor. Set to -1 for anonymous mappings.</param>
    /// <param name="offset">Offset in the file. Set to 0 for anonymous mappings.</param>
    /// <returns>
    ///     A pointer to the mapped memory region.
    ///     On success, returns the starting address of the mapped region.
    ///     On error, returns (IntPtr)(-1) and errno is set to indicate the error.
    /// </returns>
#if NET7_0_OR_GREATER
    [LibraryImport("libc", SetLastError = true)]
    public static partial IntPtr mmap(nuint addr, nuint length, int prot, int flags, int fd, long offset);
#else
    [DllImport("libc", SetLastError = true)]
    public static extern IntPtr mmap(nuint addr, nuint length, int prot, int flags, int fd, long offset);
#endif

    /// <summary>
    ///     Get configuration information at run time
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
#if NET7_0_OR_GREATER
    [DllImport("libc", SetLastError = true)]
    public static extern long sysconf(int name);
#else
    [DllImport("libc", SetLastError = true)]
    public static extern long sysconf(int name);
#endif

    /// <summary>
    ///     Sets the memory protection flags for Unix.
    /// </summary>
    /// <param name="addr">Address of memory.</param>
    /// <param name="len">Length of memory.</param>
    /// <param name="prot">Protection specified.</param>
    /// <returns>Old protection.</returns>
#if NET7_0_OR_GREATER
    [LibraryImport("libc", SetLastError = true)]
    public static partial int mprotect(nuint addr, UIntPtr len, UnixMemoryProtection prot);
#else
    [DllImport("libc", SetLastError = true)]
    public static extern int mprotect(nuint addr, UIntPtr len, UnixMemoryProtection prot);
#endif

    /// <summary>
    ///     Unmaps (frees) a region of memory.
    /// </summary>
    /// <param name="addr">Address to free.</param>
    /// <param name="length">Length of data to free.</param>
#if NET7_0_OR_GREATER
    [LibraryImport("libc")]
    public static partial int munmap(nuint addr, nuint length);
#else
    [DllImport("libc")]
    public static extern int munmap(nuint addr, nuint length);
#endif

    /// <summary>
    ///     Reads memory from a specified process.
    /// </summary>
    /// <param name="pid">Process id.</param>
    /// <param name="local_iov">Array of local memory segments to receive the data.</param>
    /// <param name="liovcnt">Number of <paramref name="local_iov" />.</param>
    /// <param name="remote_iov">Array of remote memory segments to pull data from.</param>
    /// <param name="riovcnt">Number of <paramref name="riovcnt" />.</param>
    /// <param name="flags">Flags to use in the operation.</param>
    /// <returns></returns>
#if NET7_0_OR_GREATER
    [LibraryImport("libc")]
    public static unsafe partial IntPtr process_vm_readv(int pid, IoVec* local_iov, ulong liovcnt, IoVec* remote_iov,
        ulong riovcnt, ulong flags);
#else
    [DllImport("libc")]
    public static extern unsafe IntPtr process_vm_readv(int pid, IoVec* local_iov, ulong liovcnt, IoVec* remote_iov,
        ulong riovcnt, ulong flags);
#endif

    /// <summary>
    ///     Writes memory to a specified process.
    /// </summary>
    /// <param name="pid">Process id.</param>
    /// <param name="local_iov">Array of local memory segments to get the data from.</param>
    /// <param name="liovcnt">Number of <paramref name="local_iov" />.</param>
    /// <param name="remote_iov">Array of remote memory segments to write the data to.</param>
    /// <param name="riovcnt">Number of <paramref name="riovcnt" />.</param>
    /// <param name="flags">Flags to use in the operation.</param>
    /// <returns></returns>
#if NET7_0_OR_GREATER
    [LibraryImport("libc")]
    public static unsafe partial IntPtr process_vm_writev(int pid, IoVec* local_iov, ulong liovcnt, IoVec* remote_iov,
        ulong riovcnt, ulong flags);
#else
    [DllImport("libc")]
    public static extern unsafe IntPtr process_vm_writev(int pid, IoVec* local_iov, ulong liovcnt, IoVec* remote_iov,
        ulong riovcnt, ulong flags);
#endif

    /// <summary>
    ///     Size and length tuple for Unix memory operations.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct IoVec
    {
        /// <summary>
        ///     Address to read/write to.
        /// </summary>
        public nuint iov_base;

        /// <summary>
        ///     Length of the address at said memory location.
        /// </summary>
        public nuint iov_len;
    }

    /// <summary>
    ///     Helper around
    ///     <see
    ///         cref="process_vm_readv(int,Reloaded.Memory.Native.Unix.Posix.IoVec*,ulong,Reloaded.Memory.Native.Unix.Posix.IoVec*,ulong,ulong)" />
    ///     but with an API similar to <see cref="Kernel32.ReadProcessMemory" />.
    /// </summary>
    /// <param name="processId">Id of the process to read from.</param>
    /// <param name="localIov">Local memory address.</param>
    /// <param name="remoteIov">Remote memory address.</param>
    /// <param name="numBytes">Memory size.</param>
    /// <returns>True on success, else false.</returns>
    [Obsolete("Use process_vm_readv_k32_2 instead. This function has incorrect parameter order.")]
    [PublicAPI]
    public static bool process_vm_readv_k32(nint processId, nuint localIov, nuint remoteIov, nuint numBytes) => process_vm_readv_k32_2(processId, remoteIov, localIov, numBytes);

    /// <summary>
    ///     Helper around
    ///     <see
    ///         cref="process_vm_readv(int,Reloaded.Memory.Native.Unix.Posix.IoVec*,ulong,Reloaded.Memory.Native.Unix.Posix.IoVec*,ulong,ulong)" />
    ///     but with an API similar to <see cref="Kernel32.ReadProcessMemory" />.
    /// </summary>
    /// <param name="processId">Id of the process to read from.</param>
    /// <param name="remoteIov">Remote memory address.</param>
    /// <param name="localIov">Local memory address.</param>
    /// <param name="numBytes">Memory size.</param>
    /// <returns>True on success, else false.</returns>
    public static unsafe bool process_vm_readv_k32_2(nint processId, nuint remoteIov, nuint localIov, nuint numBytes)
    {
        IoVec local = new() { iov_base = localIov, iov_len = numBytes };
        IoVec remote = new() { iov_base = remoteIov, iov_len = numBytes };
        return process_vm_readv((int)processId, &local, 1, &remote, 1, 0) != new IntPtr(-1);
    }

    /// <summary>
    ///     Helper around
    ///     <see
    ///         cref="process_vm_readv(int,Reloaded.Memory.Native.Unix.Posix.IoVec*,ulong,Reloaded.Memory.Native.Unix.Posix.IoVec*,ulong,ulong)" />
    ///     but with an API similar to <see cref="Kernel32.ReadProcessMemory" />.
    /// </summary>
    /// <param name="processId">Id of the process to read from.</param>
    /// <param name="remoteIov">Local memory address.</param>
    /// <param name="localIov">Remote memory address.</param>
    /// <param name="numBytes">Memory size.</param>
    /// <returns>True on success, else false.</returns>
    public static unsafe bool process_vm_writev_k32(nint processId, nuint remoteIov, nuint localIov, nuint numBytes)
    {
        IoVec remote = new() { iov_base = remoteIov, iov_len = numBytes };
        IoVec local = new() { iov_base = localIov, iov_len = numBytes };
        return process_vm_writev((int)processId, &local, 1, &remote, 1, 0) != new IntPtr(-1);
    }
}
