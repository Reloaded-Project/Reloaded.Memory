using System.Security;
#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif

// ReSharper disable InconsistentNaming

namespace Reloaded.Memory.Native.Windows;

/// <summary>
///     Contains all Kernel32 API methods used by this library.
/// </summary>
[PublicAPI]
#if NET5_0_OR_GREATER
[SupportedOSPlatform("windows")]
#endif
// ReSharper disable once PartialTypeWithSinglePart
public static partial class Kernel32
{
    /// <summary>
    ///     Reads data from an area of memory in a specified process. The entire area to be read must be accessible or the
    ///     operation fails.
    /// </summary>
    /// <param name="hProcess">
    ///     A handle to the process with memory that is being read. The handle must have PROCESS_VM_READ
    ///     access to the process.
    /// </param>
    /// <param name="lpBaseAddress">
    ///     A pointer to the base address in the specified process from which to read. Before any data transfer occurs, the
    ///     system verifies that all data in the
    ///     base address and memory of the specified size is accessible for read access, and if it is not accessible the
    ///     function fails.
    /// </param>
    /// <param name="lpBuffer">
    ///     A pointer to a buffer that receives the contents from the address space of the specified
    ///     process.
    /// </param>
    /// <param name="nSize">The number of bytes to be read from the specified process.</param>
    /// <param name="lpNumberOfBytesRead">
    ///     A pointer to a variable that receives the number of bytes transferred into the specified buffer. If
    ///     lpNumberOfBytesRead is <c>NULL</c>, the parameter
    ///     is ignored.
    /// </param>
    /// <returns>
    ///     <para>If the function succeeds, the return value is nonzero.</para>
    ///     <para>
    ///         If the function fails, the return value is 0 (zero). To get extended error information, call
    ///         <c>GetLastError</c>.
    ///     </para>
    ///     <para>The function fails if the requested read operation crosses into an area of the process that is inaccessible.</para>
    /// </returns>
    [SuppressUnmanagedCodeSecurity]
#if NET5_0_OR_GREATER
    [SuppressGCTransition]
#endif
#if NET7_0_OR_GREATER
    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool ReadProcessMemory(IntPtr hProcess, UIntPtr lpBaseAddress, UIntPtr lpBuffer,
        UIntPtr nSize,
        out UIntPtr lpNumberOfBytesRead);
#else
    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ReadProcessMemory(IntPtr hProcess, UIntPtr lpBaseAddress, UIntPtr lpBuffer, UIntPtr nSize,
        out UIntPtr lpNumberOfBytesRead);
#endif


    /// <summary>
    ///     Writes data to an area of memory in a specified process. The entire area to be written to must be accessible
    ///     or the operation fails.
    /// </summary>
    /// <param name="hProcess">
    ///     A handle to the process memory to be modified. The handle must have PROCESS_VM_WRITE and PROCESS_VM_OPERATION
    ///     access to the process.
    /// </param>
    /// <param name="lpBaseAddress">
    ///     A pointer to the base address in the specified process to which data is written. Before data transfer occurs, the
    ///     system verifies that all data in
    ///     the base address and memory of the specified size is accessible for write access, and if it is not accessible, the
    ///     function fails.
    /// </param>
    /// <param name="lpBuffer">
    ///     A pointer to the buffer that contains data to be written in the address space of the specified
    ///     process.
    /// </param>
    /// <param name="nSize">The number of bytes to be written to the specified process.</param>
    /// <param name="lpNumberOfBytesWritten">
    ///     A pointer to a variable that receives the number of bytes transferred into the specified process. This parameter is
    ///     optional. If
    ///     lpNumberOfBytesWritten is <c>NULL</c>, the parameter is ignored.
    /// </param>
    /// <returns>
    ///     <para>If the function succeeds, the return value is nonzero.</para>
    ///     <para>
    ///         If the function fails, the return value is 0 (zero). To get extended error information, call
    ///         <c>GetLastError</c>. The function fails if the requested
    ///         write operation crosses into an area of the process that is inaccessible.
    ///     </para>
    /// </returns>
    [SuppressUnmanagedCodeSecurity]
    #if NET5_0_OR_GREATER
    [SuppressGCTransition]
    #endif
#if NET7_0_OR_GREATER
    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool WriteProcessMemory(IntPtr hProcess, UIntPtr lpBaseAddress, UIntPtr lpBuffer,
        UIntPtr nSize, out UIntPtr lpNumberOfBytesWritten);
#else
    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool WriteProcessMemory(IntPtr hProcess, UIntPtr lpBaseAddress, UIntPtr lpBuffer,
        UIntPtr nSize, out UIntPtr lpNumberOfBytesWritten);
#endif


    /// <summary>
    ///     <para>
    ///         Reserves, commits, or changes the state of a region of memory within the virtual address space of a specified
    ///         process. The function initializes the
    ///         memory it allocates to zero.
    ///     </para>
    ///     <para>To specify the NUMA node for the physical memory, see <c>VirtualAllocExNuma</c>.</para>
    /// </summary>
    /// <param name="hProcess">
    ///     <para>The handle to a process. The function allocates memory within the virtual address space of this process.</para>
    ///     <para>
    ///         The handle must have the <c>PROCESS_VM_OPERATION</c> access right. For more information, see Process Security
    ///         and Access Rights.
    ///     </para>
    /// </param>
    /// <param name="lpAddress">
    ///     <para>The pointer that specifies a desired starting address for the region of pages that you want to allocate.</para>
    ///     <para>
    ///         If you are reserving memory, the function rounds this address down to the nearest multiple of the allocation
    ///         granularity.
    ///     </para>
    ///     <para>
    ///         If you are committing memory that is already reserved, the function rounds this address down to the nearest
    ///         page boundary. To determine the size of a
    ///         page and the allocation granularity on the host computer, use the <c>GetSystemInfo</c> function.
    ///     </para>
    ///     <para>If lpAddress is <c>NULL</c>, the function determines where to allocate the region.</para>
    ///     <para>
    ///         If this address is within an enclave that you have not initialized by calling <c>InitializeEnclave</c>,
    ///         <c>VirtualAllocEx</c> allocates a page of
    ///         zeros for the enclave at that address. The page must be previously uncommitted, and will not be measured with
    ///         the EEXTEND instruction of the Intel
    ///         Software Guard Extensions programming model.
    ///     </para>
    ///     <para>
    ///         If the address in within an enclave that you initialized, then the allocation operation fails with the
    ///         <c>ERROR_INVALID_ADDRESS</c> error.
    ///     </para>
    /// </param>
    /// <param name="dwSize">
    ///     <para>The size of the region of memory to allocate, in bytes.</para>
    ///     <para>If lpAddress is <c>NULL</c>, the function rounds dwSize up to the next page boundary.</para>
    ///     <para>
    ///         If lpAddress is not <c>NULL</c>, the function allocates all pages that contain one or more bytes in the range
    ///         from lpAddress to lpAddress+dwSize.
    ///         This means, for example, that a 2-byte range that straddles a page boundary causes the function to allocate
    ///         both pages.
    ///     </para>
    /// </param>
    /// <param name="flAllocationType">
    ///     <para>The type of memory allocation. This parameter must contain one of the following values.</para>
    ///     <para>
    ///         <list type="table">
    ///             <listheader>
    ///                 <term>Value</term>
    ///                 <term>Meaning</term>
    ///             </listheader>
    ///             <item>
    ///                 <term>MEM_COMMIT0x00001000</term>
    ///                 <term>
    ///                     Allocates memory charges (from the overall size of memory and the paging files on disk) for the
    ///                     specified reserved memory pages. The function also
    ///                     guarantees that when the caller later initially accesses the memory, the contents will be zero.
    ///                     Actual physical pages are not allocated unless/until
    ///                     the virtual addresses are actually accessed.To reserve and commit pages in one step, call
    ///                     VirtualAllocEx with .Attempting to commit a specific
    ///                     address range by specifying MEM_COMMIT without MEM_RESERVE and a non-NULL lpAddress fails unless
    ///                     the entire range has already been reserved. The
    ///                     resulting error code is ERROR_INVALID_ADDRESS.An attempt to commit a page that is already committed
    ///                     does not cause the function to fail. This means
    ///                     that you can commit pages without first determining the current commitment state of each page.If
    ///                     lpAddress specifies an address within an enclave,
    ///                     flAllocationType must be MEM_COMMIT.
    ///                 </term>
    ///             </item>
    ///             <item>
    ///                 <term>MEM_RESERVE0x00002000</term>
    ///                 <term>
    ///                     Reserves a range of the process's virtual address space without allocating any actual physical
    ///                     storage in memory or in the paging file on
    ///                     disk.You commit reserved pages by calling VirtualAllocEx again with MEM_COMMIT. To reserve and
    ///                     commit pages in one step, call VirtualAllocEx with
    ///                     .Other memory allocation functions, such as malloc and LocalAlloc, cannot use reserved memory until
    ///                     it has been released.
    ///                 </term>
    ///             </item>
    ///             <item>
    ///                 <term>MEM_RESET0x00080000</term>
    ///                 <term>
    ///                     Indicates that data in the memory range specified by lpAddress and dwSize is no longer of interest.
    ///                     The pages should not be read from or written to
    ///                     the paging file. However, the memory block will be used again later, so it should not be
    ///                     decommitted. This value cannot be used with any other
    ///                     value.Using this value does not guarantee that the range operated on with MEM_RESET will contain
    ///                     zeros. If you want the range to contain zeros,
    ///                     decommit the memory and then recommit it.When you use MEM_RESET, the VirtualAllocEx function
    ///                     ignores the value of fProtect. However, you must still
    ///                     set fProtect to a valid protection value, such as PAGE_NOACCESS.VirtualAllocEx returns an error if
    ///                     you use MEM_RESET and the range of memory is
    ///                     mapped to a file. A shared view is only acceptable if it is mapped to a paging file.
    ///                 </term>
    ///             </item>
    ///             <item>
    ///                 <term>MEM_RESET_UNDO0x1000000</term>
    ///                 <term>
    ///                     MEM_RESET_UNDO should only be called on an address range to which MEM_RESET was successfully
    ///                     applied earlier. It indicates that the data in the
    ///                     specified memory range specified by lpAddress and dwSize is of interest to the caller and attempts
    ///                     to reverse the effects of MEM_RESET. If the
    ///                     function succeeds, that means all data in the specified address range is intact. If the function
    ///                     fails, at least some of the data in the address
    ///                     range has been replaced with zeroes.This value cannot be used with any other value. If
    ///                     MEM_RESET_UNDO is called on an address range which was not
    ///                     MEM_RESET earlier, the behavior is undefined. When you specify MEM_RESET, the VirtualAllocEx
    ///                     function ignores the value of flProtect. However, you
    ///                     must still set flProtect to a valid protection value, such as PAGE_NOACCESS.Windows Server 2008 R2,
    ///                     Windows 7, Windows Server 2008, Windows Vista,
    ///                     Windows Server 2003 and Windows XP: The MEM_RESET_UNDO flag is not supported until Windows 8 and
    ///                     Windows Server 2012.
    ///                 </term>
    ///             </item>
    ///         </list>
    ///     </para>
    ///     <para>This parameter can also specify the following values as indicated.</para>
    ///     <para>
    ///         <list type="table">
    ///             <listheader>
    ///                 <term>Value</term>
    ///                 <term>Meaning</term>
    ///             </listheader>
    ///             <item>
    ///                 <term>MEM_LARGE_PAGES0x20000000</term>
    ///                 <term>
    ///                     Allocates memory using large page support.The size and alignment must be a multiple of the
    ///                     large-page minimum. To obtain this value, use the
    ///                     GetLargePageMinimum function.If you specify this value, you must also specify MEM_RESERVE and
    ///                     MEM_COMMIT.
    ///                 </term>
    ///             </item>
    ///             <item>
    ///                 <term>MEM_PHYSICAL0x00400000</term>
    ///                 <term>
    ///                     Reserves an address range that can be used to map Address Windowing Extensions (AWE) pages.This
    ///                     value must be used with MEM_RESERVE and no other values.
    ///                 </term>
    ///             </item>
    ///             <item>
    ///                 <term>MEM_TOP_DOWN0x00100000</term>
    ///                 <term>
    ///                     Allocates memory at the highest possible address. This can be slower than regular allocations,
    ///                     especially when there are many allocations.
    ///                 </term>
    ///             </item>
    ///         </list>
    ///     </para>
    /// </param>
    /// <param name="flProtect">
    ///     <para>
    ///         The memory protection for the region of pages to be allocated. If the pages are being committed, you can
    ///         specify any one of the memory protection constants.
    ///     </para>
    ///     <para>If lpAddress specifies an address within an enclave, flProtect cannot be any of the following values:</para>
    /// </param>
    /// <returns>
    ///     <para>If the function succeeds, the return value is the base address of the allocated region of pages.</para>
    ///     <para>
    ///         If the function fails, the return value is <c>NULL</c>. To get extended error information, call
    ///         <c>GetLastError</c>.
    ///     </para>
    /// </returns>
    [SuppressUnmanagedCodeSecurity]
#if NET5_0_OR_GREATER
    [SuppressGCTransition]
#endif
#if NET7_0_OR_GREATER
    [LibraryImport("kernel32.dll", SetLastError = true)]
    public static partial nuint VirtualAllocEx(IntPtr hProcess, UIntPtr lpAddress, UIntPtr dwSize,
        MEM_ALLOCATION_TYPE flAllocationType, MEM_PROTECTION flProtect);
#else
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern nuint VirtualAllocEx(IntPtr hProcess, UIntPtr lpAddress, UIntPtr dwSize,
        MEM_ALLOCATION_TYPE flAllocationType, MEM_PROTECTION flProtect);
#endif


    /// <summary>
    ///     Releases, decommits, or releases and decommits a region of memory within the virtual address space of a
    ///     specified process.
    /// </summary>
    /// <param name="hProcess">
    ///     <para>A handle to a process. The function frees memory within the virtual address space of the process.</para>
    ///     <para>
    ///         The handle must have the <c>PROCESS_VM_OPERATION</c> access right. For more information, see Process Security
    ///         and Access Rights.
    ///     </para>
    /// </param>
    /// <param name="lpAddress">
    ///     <para>A pointer to the starting address of the region of memory to be freed.</para>
    ///     <para>
    ///         If the dwFreeType parameter is <c>MEM_RELEASE</c>, lpAddress must be the base address returned by the
    ///         <c>VirtualAllocEx</c> function when the region
    ///         is reserved.
    ///     </para>
    /// </param>
    /// <param name="dwSize">
    ///     <para>The size of the region of memory to free, in bytes.</para>
    ///     <para>
    ///         If the dwFreeType parameter is <c>MEM_RELEASE</c>, dwSize must be 0 (zero). The function frees the entire
    ///         region that is reserved in the initial
    ///         allocation call to <c>VirtualAllocEx</c>.
    ///     </para>
    ///     <para>
    ///         If dwFreeType is <c>MEM_DECOMMIT</c>, the function decommits all memory pages that contain one or more bytes in
    ///         the range from the lpAddress
    ///         parameter to . This means, for example, that a 2-byte region of memory that straddles a page boundary causes
    ///         both pages to be decommitted. If
    ///         lpAddress is the base address returned by <c>VirtualAllocEx</c> and dwSize is 0 (zero), the function decommits
    ///         the entire region that is allocated by
    ///         <c>VirtualAllocEx</c>. After that, the entire region is in the reserved state.
    ///     </para>
    /// </param>
    /// <param name="dwFreeType">
    ///     <para>The type of free operation. This parameter can be one of the following values.</para>
    ///     <para>
    ///         <list type="table">
    ///             <listheader>
    ///                 <term>Value</term>
    ///                 <term>Meaning</term>
    ///             </listheader>
    ///             <item>
    ///                 <term>MEM_DECOMMIT0x4000</term>
    ///                 <term>
    ///                     Decommits the specified region of committed pages. After the operation, the pages are in the
    ///                     reserved state. The function does not fail if you
    ///                     attempt to decommit an uncommitted page. This means that you can decommit a range of pages without
    ///                     first determining their current commitment
    ///                     state.Do not use this value with MEM_RELEASE.The MEM_DECOMMIT value is not supported when the
    ///                     lpAddress parameter provides the base address for an enclave.
    ///                 </term>
    ///             </item>
    ///             <item>
    ///                 <term>MEM_RELEASE0x8000</term>
    ///                 <term>
    ///                     Releases the specified region of pages. After the operation, the pages are in the free state. If
    ///                     you specify this value, dwSize must be 0 (zero), and
    ///                     lpAddress must point to the base address returned by the VirtualAllocEx function when the region is
    ///                     reserved. The function fails if either of these
    ///                     conditions is not met.If any pages in the region are committed currently, the function first
    ///                     decommits, and then releases them.The function does not
    ///                     fail if you attempt to release pages that are in different states, some reserved and some
    ///                     committed. This means that you can release a range of pages
    ///                     without first determining the current commitment state.Do not use this value with MEM_DECOMMIT.
    ///                 </term>
    ///             </item>
    ///         </list>
    ///     </para>
    /// </param>
    /// <returns>
    ///     <para>If the function succeeds, the return value is a nonzero value.</para>
    ///     <para>
    ///         If the function fails, the return value is 0 (zero). To get extended error information, call
    ///         <c>GetLastError</c>.
    ///     </para>
    /// </returns>
    [SuppressUnmanagedCodeSecurity]
#if NET5_0_OR_GREATER
    [SuppressGCTransition]
#endif
#if NET7_0_OR_GREATER
    [LibraryImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool VirtualFreeEx(IntPtr hProcess, UIntPtr lpAddress, UIntPtr dwSize,
        MEM_ALLOCATION_TYPE dwFreeType);
#else
    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool VirtualFreeEx(IntPtr hProcess, UIntPtr lpAddress, UIntPtr dwSize,
        MEM_ALLOCATION_TYPE dwFreeType);
#endif


    /// <summary>Changes the protection on a region of committed pages in the virtual address space of a specified process.</summary>
    /// <param name="hProcess">
    ///     A handle to the process whose memory protection is to be changed. The handle must have the
    ///     <c>PROCESS_VM_OPERATION</c> access right. For more
    ///     information, see Process Security and Access Rights.
    /// </param>
    /// <param name="lpAddress">
    ///     <para>A pointer to the base address of the region of pages whose access protection attributes are to be changed.</para>
    ///     <para>
    ///         All pages in the specified region must be within the same reserved region allocated when calling the
    ///         <c>VirtualAlloc</c> or <c>VirtualAllocEx</c>
    ///         function using <c>MEM_RESERVE</c>. The pages cannot span adjacent reserved regions that were allocated by
    ///         separate calls to <c>VirtualAlloc</c> or
    ///         <c>VirtualAllocEx</c> using <c>MEM_RESERVE</c>.
    ///     </para>
    /// </param>
    /// <param name="dwSize">
    ///     The size of the region whose access protection attributes are changed, in bytes. The region of affected pages
    ///     includes all pages containing one or
    ///     more bytes in the range from the lpAddress parameter to . This means that a 2-byte range straddling a page boundary
    ///     causes the protection attributes
    ///     of both pages to be changed.
    /// </param>
    /// <param name="flNewProtect">
    ///     <para>The memory protection option. This parameter can be one of the memory protection constants.</para>
    ///     <para>
    ///         For mapped views, this value must be compatible with the access protection specified when the view was mapped
    ///         (see <c>MapViewOfFile</c>,
    ///         <c>MapViewOfFileEx</c>, and <c>MapViewOfFileExNuma</c>).
    ///     </para>
    /// </param>
    /// <param name="lpflOldProtect">
    ///     A pointer to a variable that receives the previous access protection of the first page in the specified region of
    ///     pages. If this parameter is
    ///     <c>NULL</c> or does not point to a valid variable, the function fails.
    /// </param>
    /// <returns>
    ///     <para>If the function succeeds, the return value is nonzero.</para>
    ///     <para>If the function fails, the return value is zero. To get extended error information, call <c>GetLastError</c>.</para>
    /// </returns>
    [SuppressUnmanagedCodeSecurity]
#if NET5_0_OR_GREATER
    [SuppressGCTransition]
#endif
#if NET7_0_OR_GREATER
    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool VirtualProtectEx(IntPtr hProcess, UIntPtr lpAddress, UIntPtr dwSize,
        MEM_PROTECTION flNewProtect, out MEM_PROTECTION lpflOldProtect);
#else
    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool VirtualProtectEx(IntPtr hProcess, UIntPtr lpAddress, UIntPtr dwSize,
        MEM_PROTECTION flNewProtect, out MEM_PROTECTION lpflOldProtect);
#endif


    /// <summary>
    ///     <para>
    ///         Reserves, commits, or changes the state of a region of pages in the virtual address space of the calling
    ///         process. Memory allocated by this function
    ///         is automatically initialized to zero.
    ///     </para>
    ///     <para>To allocate memory in the address space of another process, use the <c>VirtualAllocEx</c> function.</para>
    /// </summary>
    /// <param name="lpAddress">
    ///     <para>
    ///         The starting address of the region to allocate. If the memory is being reserved, the specified address is
    ///         rounded down to the nearest multiple of the
    ///         allocation granularity. If the memory is already reserved and is being committed, the address is rounded down
    ///         to the next page boundary. To determine
    ///         the size of a page and the allocation granularity on the host computer, use the <c>GetSystemInfo</c> function.
    ///         If this parameter is <c>NULL</c>, the
    ///         system determines where to allocate the region.
    ///     </para>
    ///     <para>
    ///         If this address is within an enclave that you have not initialized by calling <c>InitializeEnclave</c>,
    ///         <c>VirtualAlloc</c> allocates a page of zeros
    ///         for the enclave at that address. The page must be previously uncommitted, and will not be measured with the
    ///         EEXTEND instruction of the Intel Software
    ///         Guard Extensions programming model.
    ///     </para>
    ///     <para>
    ///         If the address in within an enclave that you initialized, then the allocation operation fails with the
    ///         <c>ERROR_INVALID_ADDRESS</c> error.
    ///     </para>
    /// </param>
    /// <param name="dwSize">
    ///     The size of the region, in bytes. If the lpAddress parameter is <c>NULL</c>, this value is rounded up to the next
    ///     page boundary. Otherwise, the
    ///     allocated pages include all pages containing one or more bytes in the range from lpAddress to lpAddress+dwSize.
    ///     This means that a 2-byte range
    ///     straddling a page boundary causes both pages to be included in the allocated region.
    /// </param>
    /// <param name="flAllocationType">
    ///     <para>The type of memory allocation. This parameter must contain one of the following values.</para>
    ///     <para>
    ///         <list type="table">
    ///             <listheader>
    ///                 <term>Value</term>
    ///                 <term>Meaning</term>
    ///             </listheader>
    ///             <item>
    ///                 <term>MEM_COMMIT0x00001000</term>
    ///                 <term>
    ///                     Allocates memory charges (from the overall size of memory and the paging files on disk) for the
    ///                     specified reserved memory pages. The function also
    ///                     guarantees that when the caller later initially accesses the memory, the contents will be zero.
    ///                     Actual physical pages are not allocated unless/until
    ///                     the virtual addresses are actually accessed.To reserve and commit pages in one step, call
    ///                     VirtualAlloc with .Attempting to commit a specific address
    ///                     range by specifying MEM_COMMIT without MEM_RESERVE and a non-NULL lpAddress fails unless the entire
    ///                     range has already been reserved. The resulting
    ///                     error code is ERROR_INVALID_ADDRESS.An attempt to commit a page that is already committed does not
    ///                     cause the function to fail. This means that you
    ///                     can commit pages without first determining the current commitment state of each page.If lpAddress
    ///                     specifies an address within an enclave,
    ///                     flAllocationType must be MEM_COMMIT.
    ///                 </term>
    ///             </item>
    ///             <item>
    ///                 <term>MEM_RESERVE0x00002000</term>
    ///                 <term>
    ///                     Reserves a range of the process's virtual address space without allocating any actual physical
    ///                     storage in memory or in the paging file on
    ///                     disk.You can commit reserved pages in subsequent calls to the VirtualAlloc function. To reserve and
    ///                     commit pages in one step, call VirtualAlloc with
    ///                     MEM_COMMIT | MEM_RESERVE.Other memory allocation functions, such as malloc and LocalAlloc, cannot
    ///                     use a reserved range of memory until it is released.
    ///                 </term>
    ///             </item>
    ///             <item>
    ///                 <term>MEM_RESET0x00080000</term>
    ///                 <term>
    ///                     Indicates that data in the memory range specified by lpAddress and dwSize is no longer of interest.
    ///                     The pages should not be read from or written to
    ///                     the paging file. However, the memory block will be used again later, so it should not be
    ///                     decommitted. This value cannot be used with any other
    ///                     value.Using this value does not guarantee that the range operated on with MEM_RESET will contain
    ///                     zeros. If you want the range to contain zeros,
    ///                     decommit the memory and then recommit it.When you specify MEM_RESET, the VirtualAlloc function
    ///                     ignores the value of flProtect. However, you must
    ///                     still set flProtect to a valid protection value, such as PAGE_NOACCESS.VirtualAlloc returns an
    ///                     error if you use MEM_RESET and the range of memory is
    ///                     mapped to a file. A shared view is only acceptable if it is mapped to a paging file.
    ///                 </term>
    ///             </item>
    ///             <item>
    ///                 <term>MEM_RESET_UNDO0x1000000</term>
    ///                 <term>
    ///                     MEM_RESET_UNDO should only be called on an address range to which MEM_RESET was successfully
    ///                     applied earlier. It indicates that the data in the
    ///                     specified memory range specified by lpAddress and dwSize is of interest to the caller and attempts
    ///                     to reverse the effects of MEM_RESET. If the
    ///                     function succeeds, that means all data in the specified address range is intact. If the function
    ///                     fails, at least some of the data in the address
    ///                     range has been replaced with zeroes.This value cannot be used with any other value. If
    ///                     MEM_RESET_UNDO is called on an address range which was not
    ///                     MEM_RESET earlier, the behavior is undefined. When you specify MEM_RESET, the VirtualAlloc function
    ///                     ignores the value of flProtect. However, you must
    ///                     still set flProtect to a valid protection value, such as PAGE_NOACCESS.Windows Server 2008 R2,
    ///                     Windows 7, Windows Server 2008, Windows Vista, Windows
    ///                     Server 2003 and Windows XP: The MEM_RESET_UNDO flag is not supported until Windows 8 and Windows
    ///                     Server 2012.
    ///                 </term>
    ///             </item>
    ///         </list>
    ///     </para>
    ///     <para>This parameter can also specify the following values as indicated.</para>
    ///     <para>
    ///         <list type="table">
    ///             <listheader>
    ///                 <term>Value</term>
    ///                 <term>Meaning</term>
    ///             </listheader>
    ///             <item>
    ///                 <term>MEM_LARGE_PAGES0x20000000</term>
    ///                 <term>
    ///                     Allocates memory using large page support.The size and alignment must be a multiple of the
    ///                     large-page minimum. To obtain this value, use the
    ///                     GetLargePageMinimum function.If you specify this value, you must also specify MEM_RESERVE and
    ///                     MEM_COMMIT.
    ///                 </term>
    ///             </item>
    ///             <item>
    ///                 <term>MEM_PHYSICAL0x00400000</term>
    ///                 <term>
    ///                     Reserves an address range that can be used to map Address Windowing Extensions (AWE) pages.This
    ///                     value must be used with MEM_RESERVE and no other values.
    ///                 </term>
    ///             </item>
    ///             <item>
    ///                 <term>MEM_TOP_DOWN0x00100000</term>
    ///                 <term>
    ///                     Allocates memory at the highest possible address. This can be slower than regular allocations,
    ///                     especially when there are many allocations.
    ///                 </term>
    ///             </item>
    ///             <item>
    ///                 <term>MEM_WRITE_WATCH0x00200000</term>
    ///                 <term>
    ///                     Causes the system to track pages that are written to in the allocated region. If you specify this
    ///                     value, you must also specify MEM_RESERVE.To
    ///                     retrieve the addresses of the pages that have been written to since the region was allocated or the
    ///                     write-tracking state was reset, call the
    ///                     GetWriteWatch function. To reset the write-tracking state, call GetWriteWatch or ResetWriteWatch.
    ///                     The write-tracking feature remains enabled for the
    ///                     memory region until the region is freed.
    ///                 </term>
    ///             </item>
    ///         </list>
    ///     </para>
    /// </param>
    /// <param name="flProtect">
    ///     <para>
    ///         The memory protection for the region of pages to be allocated. If the pages are being committed, you can
    ///         specify any one of the memory protection constants.
    ///     </para>
    ///     <para>If lpAddress specifies an address within an enclave, flProtect cannot be any of the following values:</para>
    /// </param>
    /// <returns>
    ///     <para>If the function succeeds, the return value is the base address of the allocated region of pages.</para>
    ///     <para>
    ///         If the function fails, the return value is <c>NULL</c>. To get extended error information, call
    ///         <c>GetLastError</c>.
    ///     </para>
    /// </returns>
    [SuppressUnmanagedCodeSecurity]
#if NET5_0_OR_GREATER
    [SuppressGCTransition]
#endif
#if NET7_0_OR_GREATER
    [LibraryImport("kernel32.dll", SetLastError = true)]
    public static partial nuint VirtualAlloc(UIntPtr lpAddress, UIntPtr dwSize,
        MEM_ALLOCATION_TYPE flAllocationType, MEM_PROTECTION flProtect);
#else
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern nuint VirtualAlloc(UIntPtr lpAddress, UIntPtr dwSize,
        MEM_ALLOCATION_TYPE flAllocationType, MEM_PROTECTION flProtect);
#endif

    /// <summary>
    ///     <para>
    ///         Releases, decommits, or releases and decommits a region of pages within the virtual address space of the
    ///         calling process.
    ///     </para>
    ///     <para>
    ///         To free memory allocated in another process by the <c>VirtualAllocEx</c> function, use the
    ///         <c>VirtualFreeEx</c> function.
    ///     </para>
    /// </summary>
    /// <param name="lpAddress">
    ///     <para>A pointer to the base address of the region of pages to be freed.</para>
    ///     <para>
    ///         If the dwFreeType parameter is <c>MEM_RELEASE</c>, this parameter must be the base address returned by the
    ///         <c>VirtualAlloc</c> function when the
    ///         region of pages is reserved.
    ///     </para>
    /// </param>
    /// <param name="dwSize">
    ///     <para>The size of the region of memory to be freed, in bytes.</para>
    ///     <para>
    ///         If the dwFreeType parameter is <c>MEM_RELEASE</c>, this parameter must be 0 (zero). The function frees the
    ///         entire region that is reserved in the
    ///         initial allocation call to <c>VirtualAlloc</c>.
    ///     </para>
    ///     <para>
    ///         If the dwFreeType parameter is <c>MEM_DECOMMIT</c>, the function decommits all memory pages that contain one or
    ///         more bytes in the range from the
    ///         lpAddress parameter to . This means, for example, that a 2-byte region of memory that straddles a page boundary
    ///         causes both pages to be decommitted.
    ///         If lpAddress is the base address returned by <c>VirtualAlloc</c> and dwSize is 0 (zero), the function decommits
    ///         the entire region that is allocated
    ///         by <c>VirtualAlloc</c>. After that, the entire region is in the reserved state.
    ///     </para>
    /// </param>
    /// <param name="dwFreeType">
    ///     <para>The type of free operation. This parameter can be one of the following values.</para>
    ///     <para>
    ///         <list type="table">
    ///             <listheader>
    ///                 <term>Value</term>
    ///                 <term>Meaning</term>
    ///             </listheader>
    ///             <item>
    ///                 <term>MEM_DECOMMIT0x4000</term>
    ///                 <term>
    ///                     Decommits the specified region of committed pages. After the operation, the pages are in the
    ///                     reserved state. The function does not fail if you
    ///                     attempt to decommit an uncommitted page. This means that you can decommit a range of pages without
    ///                     first determining the current commitment state.Do
    ///                     not use this value with MEM_RELEASE.The MEM_DECOMMIT value is not supported when the lpAddress
    ///                     parameter provides the base address for an enclave.
    ///                 </term>
    ///             </item>
    ///             <item>
    ///                 <term>MEM_RELEASE0x8000</term>
    ///                 <term>
    ///                     Releases the specified region of pages. After this operation, the pages are in the free state. If
    ///                     you specify this value, dwSize must be 0 (zero),
    ///                     and lpAddress must point to the base address returned by the VirtualAlloc function when the region
    ///                     is reserved. The function fails if either of these
    ///                     conditions is not met.If any pages in the region are committed currently, the function first
    ///                     decommits, and then releases them.The function does not
    ///                     fail if you attempt to release pages that are in different states, some reserved and some
    ///                     committed. This means that you can release a range of pages
    ///                     without first determining the current commitment state.Do not use this value with MEM_DECOMMIT.
    ///                 </term>
    ///             </item>
    ///         </list>
    ///     </para>
    /// </param>
    /// <returns>
    ///     <para>If the function succeeds, the return value is nonzero.</para>
    ///     <para>
    ///         If the function fails, the return value is 0 (zero). To get extended error information, call
    ///         <c>GetLastError</c>.
    ///     </para>
    /// </returns>
    [SuppressUnmanagedCodeSecurity]
#if NET5_0_OR_GREATER
    [SuppressGCTransition]
#endif
#if NET7_0_OR_GREATER
    [LibraryImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool VirtualFree(UIntPtr lpAddress, UIntPtr dwSize, MEM_ALLOCATION_TYPE dwFreeType);
#else
    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool VirtualFree(UIntPtr lpAddress, UIntPtr dwSize, MEM_ALLOCATION_TYPE dwFreeType);
#endif


    /// <summary>
    ///     <para>Changes the protection on a region of committed pages in the virtual address space of the calling process.</para>
    ///     <para>To change the access protection of any process, use the <c>VirtualProtectEx</c> function.</para>
    /// </summary>
    /// <param name="lpAddress">
    ///     <para>
    ///         A pointer an address that describes the starting page of the region of pages whose access protection
    ///         attributes are to be changed.
    ///     </para>
    ///     <para>
    ///         All pages in the specified region must be within the same reserved region allocated when calling the
    ///         <c>VirtualAlloc</c> or <c>VirtualAllocEx</c>
    ///         function using <c>MEM_RESERVE</c>. The pages cannot span adjacent reserved regions that were allocated by
    ///         separate calls to <c>VirtualAlloc</c> or
    ///         <c>VirtualAllocEx</c> using <c>MEM_RESERVE</c>.
    ///     </para>
    /// </param>
    /// <param name="dwSize">
    ///     The size of the region whose access protection attributes are to be changed, in bytes. The region of affected pages
    ///     includes all pages containing one
    ///     or more bytes in the range from the lpAddress parameter to . This means that a 2-byte range straddling a page
    ///     boundary causes the protection
    ///     attributes of both pages to be changed.
    /// </param>
    /// <param name="flNewProtect">
    ///     <para>The memory protection option. This parameter can be one of the memory protection constants.</para>
    ///     <para>
    ///         For mapped views, this value must be compatible with the access protection specified when the view was mapped
    ///         (see <c>MapViewOfFile</c>,
    ///         <c>MapViewOfFileEx</c>, and <c>MapViewOfFileExNuma</c>).
    ///     </para>
    /// </param>
    /// <param name="lpflOldProtect">
    ///     A pointer to a variable that receives the previous access protection value of the first page in the specified
    ///     region of pages. If this parameter is
    ///     <c>NULL</c> or does not point to a valid variable, the function fails.
    /// </param>
    /// <returns>
    ///     <para>If the function succeeds, the return value is nonzero.</para>
    ///     <para>If the function fails, the return value is zero. To get extended error information, call <c>GetLastError</c>.</para>
    /// </returns>
    [SuppressUnmanagedCodeSecurity]
#if NET5_0_OR_GREATER
    [SuppressGCTransition]
#endif
#if NET7_0_OR_GREATER
    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool VirtualProtect(UIntPtr lpAddress, UIntPtr dwSize, MEM_PROTECTION flNewProtect,
        out MEM_PROTECTION lpflOldProtect);
#else
    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool VirtualProtect(UIntPtr lpAddress, UIntPtr dwSize, MEM_PROTECTION flNewProtect,
        out MEM_PROTECTION lpflOldProtect);
#endif

    /// <summary>The type of memory allocation.</summary>
    [Flags]
    public enum MEM_ALLOCATION_TYPE : uint
    {
        /// <summary>
        ///     Allocates memory charges (from the overall size of memory and the paging files on disk) for the specified reserved
        ///     memory pages. The function
        ///     also guarantees that when the caller later initially accesses the memory, the contents will be zero. Actual
        ///     physical pages are not allocated
        ///     unless/until the virtual addresses are actually accessed.To reserve and commit pages in one step, call VirtualAlloc
        ///     with .Attempting to commit a
        ///     specific address range by specifying MEM_COMMIT without MEM_RESERVE and a non-NULL lpAddress fails unless the
        ///     entire range has already been
        ///     reserved. The resulting error code is ERROR_INVALID_ADDRESS.An attempt to commit a page that is already committed
        ///     does not cause the function to
        ///     fail. This means that you can commit pages without first determining the current commitment state of each page.If
        ///     lpAddress specifies an address
        ///     within an enclave, flAllocationType must be MEM_COMMIT.
        /// </summary>
        MEM_COMMIT = 4096, // 0x00001000

        /// <summary>
        ///     Reserves a range of the process's virtual address space without allocating any actual physical storage in memory or
        ///     in the paging file on
        ///     disk.You can commit reserved pages in subsequent calls to the VirtualAlloc function. To reserve and commit pages in
        ///     one step, call VirtualAlloc
        ///     with MEM_COMMIT | MEM_RESERVE.Other memory allocation functions, such as malloc and LocalAlloc, cannot use a
        ///     reserved range of memory until it is released.
        /// </summary>
        MEM_RESERVE = 8192, // 0x00002000

        /// <summary>
        ///     Decommits the specified region of committed pages. After the operation, the pages are in the reserved state. The
        ///     function does not fail if you
        ///     attempt to decommit an uncommitted page. This means that you can decommit a range of pages without first
        ///     determining the current commitment
        ///     state.Do not use this value with MEM_RELEASE.The MEM_DECOMMIT value is not supported when the lpAddress parameter
        ///     provides the base address for
        ///     an enclave.
        /// </summary>
        MEM_DECOMMIT = 16384, // 0x00004000

        /// <summary>
        ///     Releases the specified region of pages. After this operation, the pages are in the free state. If you specify this
        ///     value, dwSize must be 0
        ///     (zero), and lpAddress must point to the base address returned by the VirtualAlloc function when the region is
        ///     reserved. The function fails if
        ///     either of these conditions is not met. If any pages in the region are committed currently, the function first
        ///     decommits, and then releases
        ///     them.The function does not fail if you attempt to release pages that are in different states, some reserved and
        ///     some committed. This means that
        ///     you can release a range of pages without first determining the current commitment state.Do not use this value with
        ///     MEM_DECOMMIT.
        /// </summary>
        MEM_RELEASE = 32768, // 0x00008000

        /// <summary>
        ///     Indicates free pages not accessible to the calling process and available to be allocated. For free pages, the
        ///     information in the AllocationBase,
        ///     AllocationProtect, Protect, and Type members is undefined.
        /// </summary>
        MEM_FREE = 65536, // 0x00010000

        /// <summary>Indicates that the memory pages within the region are private (that is, not shared by other processes).</summary>
        MEM_PRIVATE = 131072, // 0x00020000

        /// <summary>Indicates that the memory pages within the region are mapped into the view of a section.</summary>
        MEM_MAPPED = 262144, // 0x00040000

        /// <summary>
        ///     Indicates that data in the memory range specified by lpAddress and dwSize is no longer of interest. The pages
        ///     should not be read from or written
        ///     to the paging file. However, the memory block will be used again later, so it should not be decommitted. This value
        ///     cannot be used with any other
        ///     value.Using this value does not guarantee that the range operated on with MEM_RESET will contain zeros. If you want
        ///     the range to contain zeros,
        ///     decommit the memory and then recommit it.When you specify MEM_RESET, the VirtualAlloc function ignores the value of
        ///     flProtect. However, you must
        ///     still set flProtect to a valid protection value, such as PAGE_NOACCESS.VirtualAlloc returns an error if you use
        ///     MEM_RESET and the range of memory
        ///     is mapped to a file. A shared view is only acceptable if it is mapped to a paging file.
        /// </summary>
        MEM_RESET = 524288, // 0x00080000

        /// <summary>
        ///     Allocates memory at the highest possible address. This can be slower than regular allocations, especially when
        ///     there are many allocations.
        /// </summary>
        MEM_TOP_DOWN = 1048576, // 0x00100000

        /// <summary>
        ///     Causes the system to track pages that are written to in the allocated region. If you specify this value, you must
        ///     also specify MEM_RESERVE.To
        ///     retrieve the addresses of the pages that have been written to since the region was allocated or the write-tracking
        ///     state was reset, call the
        ///     GetWriteWatch function. To reset the write-tracking state, call GetWriteWatch or ResetWriteWatch. The
        ///     write-tracking feature remains enabled for
        ///     the memory region until the region is freed.
        /// </summary>
        MEM_WRITE_WATCH = 2097152, // 0x00200000

        /// <summary>
        ///     Reserves an address range that can be used to map Address Windowing Extensions (AWE) pages.This value must be used
        ///     with MEM_RESERVE and no other values.
        /// </summary>
        MEM_PHYSICAL = 4194304, // 0x00400000

        /// <summary>.</summary>
        MEM_ROTATE = 8388608, // 0x00800000

        /// <summary>.</summary>
        MEM_DIFFERENT_IMAGE_BASE_OK = MEM_ROTATE, // 0x00800000

        /// <summary>
        ///     MEM_RESET_UNDO should only be called on an address range to which MEM_RESET was successfully applied earlier. It
        ///     indicates that the data in the
        ///     specified memory range specified by lpAddress and dwSize is of interest to the caller and attempts to reverse the
        ///     effects of MEM_RESET. If the
        ///     function succeeds, that means all data in the specified address range is intact. If the function fails, at least
        ///     some of the data in the address
        ///     range has been replaced with zeroes.This value cannot be used with any other value. If MEM_RESET_UNDO is called on
        ///     an address range which was not
        ///     MEM_RESET earlier, the behavior is undefined. When you specify MEM_RESET, the VirtualAlloc function ignores the
        ///     value of flProtect. However, you
        ///     must still set flProtect to a valid protection value, such as PAGE_NOACCESS.Windows Server 2008 R2, Windows 7,
        ///     Windows Server 2008, Windows
        ///     Vista, Windows Server 2003 and Windows XP: The MEM_RESET_UNDO flag is not supported until Windows 8 and Windows
        ///     Server 2012.
        /// </summary>
        MEM_RESET_UNDO = 16777216, // 0x01000000

        /// <summary>
        ///     Allocates memory using large page support.The size and alignment must be a multiple of the large-page minimum. To
        ///     obtain this value, use the
        ///     GetLargePageMinimum function.If you specify this value, you must also specify MEM_RESERVE and MEM_COMMIT.
        /// </summary>
        MEM_LARGE_PAGES = 536870912, // 0x20000000

        /// <summary>.</summary>
        MEM_4MB_PAGES = 2147483648, // 0x80000000

        /// <summary>.</summary>
        MEM_64K_PAGES = MEM_LARGE_PAGES | MEM_PHYSICAL // 0x20400000
    }

    /// <summary>
    ///     The following are the memory-protection options; you must specify one of the following values when allocating or
    ///     protecting a page in memory.
    ///     Protection attributes cannot be assigned to a portion of a page; they can only be assigned to a whole page.
    /// </summary>
    [Flags]
    public enum MEM_PROTECTION : uint
    {
        /// <summary>
        ///     Disables all access to the committed region of pages. An attempt to read from, write to, or execute the committed
        ///     region results in an access violation.
        ///     <para>This flag is not supported by the CreateFileMapping function.</para>
        /// </summary>
        PAGE_NOACCESS = 1,

        /// <summary>
        ///     Enables read-only access to the committed region of pages. An attempt to write to the committed region results in
        ///     an access violation. If Data
        ///     Execution Prevention is enabled, an attempt to execute code in the committed region results in an access violation.
        /// </summary>
        PAGE_READONLY = 2,

        /// <summary>
        ///     Enables read-only or read/write access to the committed region of pages. If Data Execution Prevention is enabled,
        ///     attempting to execute code in
        ///     the committed region results in an access violation.
        /// </summary>
        PAGE_READWRITE = 4,

        /// <summary>
        ///     Enables read-only or copy-on-write access to a mapped view of a file mapping object. An attempt to write to a
        ///     committed copy-on-write page
        ///     results in a private copy of the page being made for the process. The private page is marked as PAGE_READWRITE, and
        ///     the change is written to the
        ///     new page. If Data Execution Prevention is enabled, attempting to execute code in the committed region results in an
        ///     access violation.
        ///     <para>This flag is not supported by the VirtualAlloc or VirtualAllocEx functions.</para>
        /// </summary>
        PAGE_WRITECOPY = 8,

        /// <summary>
        ///     Enables execute access to the committed region of pages. An attempt to write to the committed region results in an
        ///     access violation.
        ///     <para>This flag is not supported by the CreateFileMapping function.</para>
        /// </summary>
        PAGE_EXECUTE = 16, // 0x00000010

        /// <summary>
        ///     Enables execute or read-only access to the committed region of pages. An attempt to write to the committed region
        ///     results in an access violation.
        ///     <para>
        ///         Windows Server 2003 and Windows XP: This attribute is not supported by the CreateFileMapping function until
        ///         Windows XP with SP2 and Windows
        ///         Server 2003 with SP1.
        ///     </para>
        /// </summary>
        PAGE_EXECUTE_READ = 32, // 0x00000020

        /// <summary>
        ///     Enables execute, read-only, or read/write access to the committed region of pages.
        ///     <para>
        ///         Windows Server 2003 and Windows XP: This attribute is not supported by the CreateFileMapping function until
        ///         Windows XP with SP2 and Windows
        ///         Server 2003 with SP1.
        ///     </para>
        /// </summary>
        PAGE_EXECUTE_READWRITE = 64, // 0x00000040

        /// <summary>
        ///     Enables execute, read-only, or copy-on-write access to a mapped view of a file mapping object. An attempt to write
        ///     to a committed copy-on-write
        ///     page results in a private copy of the page being made for the process. The private page is marked as
        ///     PAGE_EXECUTE_READWRITE, and the change is
        ///     written to the new page.
        ///     <para>This flag is not supported by the VirtualAlloc or VirtualAllocEx functions.</para>
        ///     <para>
        ///         Windows Vista, Windows Server 2003 and Windows XP: This attribute is not supported by the CreateFileMapping
        ///         function until Windows Vista with SP1
        ///         and Windows Server 2008.
        ///     </para>
        /// </summary>
        PAGE_EXECUTE_WRITECOPY = 128, // 0x00000080

        /// <summary>
        ///     Pages in the region become guard pages. Any attempt to access a guard page causes the system to raise a
        ///     STATUS_GUARD_PAGE_VIOLATION exception and
        ///     turn off the guard page status. Guard pages thus act as a one-time access alarm. For more information, see Creating
        ///     Guard Pages.
        ///     <para>
        ///         When an access attempt leads the system to turn off guard page status, the underlying page protection takes
        ///         over.
        ///     </para>
        ///     <para>
        ///         If a guard page exception occurs during a system service, the service typically returns a failure status
        ///         indicator.
        ///     </para>
        ///     <para>This value cannot be used with PAGE_NOACCESS.</para>
        ///     <para>This flag is not supported by the CreateFileMapping function.</para>
        /// </summary>
        PAGE_GUARD = 256, // 0x00000100

        /// <summary>
        ///     Sets all pages to be non-cachable. Applications should not use this attribute except when explicitly required for a
        ///     device. Using the interlocked
        ///     functions with memory that is mapped with SEC_NOCACHE can result in an EXCEPTION_ILLEGAL_INSTRUCTION exception.
        ///     <para>The PAGE_NOCACHE flag cannot be used with the PAGE_GUARD, PAGE_NOACCESS, or PAGE_WRITECOMBINE flags.</para>
        ///     <para>
        ///         The PAGE_NOCACHE flag can be used only when allocating private memory with the VirtualAlloc, VirtualAllocEx, or
        ///         VirtualAllocExNuma functions. To
        ///         enable non-cached memory access for shared memory, specify the SEC_NOCACHE flag when calling the
        ///         CreateFileMapping function.
        ///     </para>
        /// </summary>
        PAGE_NOCACHE = 512, // 0x00000200

        /// <summary>
        ///     Sets all pages to be write-combined.
        ///     <para>
        ///         Applications should not use this attribute except when explicitly required for a device. Using the interlocked
        ///         functions with memory that is
        ///         mapped as write-combined can result in an EXCEPTION_ILLEGAL_INSTRUCTION exception.
        ///     </para>
        ///     <para>The PAGE_WRITECOMBINE flag cannot be specified with the PAGE_NOACCESS, PAGE_GUARD, and PAGE_NOCACHE flags.</para>
        ///     <para>
        ///         The PAGE_WRITECOMBINE flag can be used only when allocating private memory with the VirtualAlloc,
        ///         VirtualAllocEx, or VirtualAllocExNuma
        ///         functions. To enable write-combined memory access for shared memory, specify the SEC_WRITECOMBINE flag when
        ///         calling the CreateFileMapping function.
        ///     </para>
        ///     <para>Windows Server 2003 and Windows XP: This flag is not supported until Windows Server 2003 with SP1.</para>
        /// </summary>
        PAGE_WRITECOMBINE = 1024, // 0x00000400

        /// <summary>
        ///     The page contents that you supply are excluded from measurement with the EEXTEND instruction of the Intel SGX
        ///     programming model.
        /// </summary>
        PAGE_ENCLAVE_UNVALIDATED = 536870912, // 0x20000000

        /// <summary>
        ///     Sets all locations in the pages as invalid targets for CFG. Used along with any execute page protection like
        ///     PAGE_EXECUTE, PAGE_EXECUTE_READ,
        ///     PAGE_EXECUTE_READWRITE and PAGE_EXECUTE_WRITECOPY. Any indirect call to locations in those pages will fail CFG
        ///     checks and the process will be
        ///     terminated. The default behavior for executable pages allocated is to be marked valid call targets for CFG.
        ///     <para>This flag is not supported by the VirtualProtect or CreateFileMapping functions.</para>
        /// </summary>
        PAGE_TARGETS_INVALID = 1073741824, // 0x40000000

        /// <summary>
        ///     Pages in the region will not have their CFG information updated while the protection changes for VirtualProtect.
        ///     For example, if the pages in the
        ///     region was allocated using PAGE_TARGETS_INVALID, then the invalid information will be maintained while the page
        ///     protection changes. This flag is
        ///     only valid when the protection changes to an executable type like PAGE_EXECUTE, PAGE_EXECUTE_READ,
        ///     PAGE_EXECUTE_READWRITE and
        ///     PAGE_EXECUTE_WRITECOPY. The default behavior for VirtualProtect protection change to executable is to mark all
        ///     locations as valid call targets
        ///     for CFG.
        ///     <para>
        ///         The following are modifiers that can be used in addition to the options provided in the previous table,
        ///         except as noted.
        ///     </para>
        /// </summary>
        PAGE_TARGETS_NO_UPDATE = PAGE_TARGETS_INVALID, // 0x40000000

        /// <summary>The page contains a thread control structure (TCS).</summary>
        PAGE_ENCLAVE_THREAD_CONTROL = 2147483648, // 0x80000000

        /// <summary>.</summary>
        PAGE_REVERT_TO_FILE_MAP = PAGE_ENCLAVE_THREAD_CONTROL // 0x80000000
    }
}
