---
hide:
  - toc
---

<div align="center">
	<h1>The Reloaded Memory Library</h1>
	<img src="Reloaded/Images/Reloaded-Icon.png" width="150" align="center" />
	<br/> <br/>
	<strong><i>Psssh, nothing personnel kid</i></strong>
	<br/> <br/>
	<!-- Coverage -->
	<a href="https://codecov.io/gh/Reloaded-Project/Reloaded.Memory">
		<img src="https://codecov.io/gh/Reloaded-Project/Reloaded.Memory/branch/master/graph/badge.svg" alt="Coverage" />
	</a>
	<!-- NuGet -->
	<a href="https://www.nuget.org/packages/Reloaded.Memory">
		<img src="https://img.shields.io/nuget/v/Reloaded.Memory.svg" alt="NuGet" />
	</a>
	<!-- Build Status -->
	<a href="https://github.com/Reloaded-Project/Reloaded.Memory/actions/workflows/build-and-publish.yml">
		<img src="https://img.shields.io/github/actions/workflow/status/Reloaded-Project/Reloaded.Memory/build-and-publish.yml" alt="Build Status" />
	</a>
</div>

## About

Reloaded.Memory is a high performance library which provides `zero-cost abstractions` for memory manipulation in C#.  
It is designed to be as fast as possible, with no overhead, while providing useful functionality to the user.  

!!! info "The library has the following characteristics."

- **Zero Cost Abstractions:** Performance equivalent to using raw pointers.  
- **Stable API:** Versions 9.0.0 and above have a fully stable, backwards compatible API.  
- **Trimming Safe:** The library is fully compatible with .NET Core's Linker.  
- **Fully Documented:** The library is fully tested & documented with XML comments.  
- **Cross Platform:** 99% of the library is fully compatible with Windows, Linux and MacOS across multiple CPU architectures.  
- **Large Address Aware:** The library can correctly leverage all 4GB in x86 processes.  

This project guarantees binary backwards compatibility; meaning you can substitute the library with any newer version
without recompiling the source code. Should the need to introduce any breaking changes occur; much like the runtime.

## Common Utilities

!!! info "Common Classes within this Package Include"

**Memory Manipulation:**

| Action                              | Description                                                                         |
|-------------------------------------|-------------------------------------------------------------------------------------|
| [Memory](./About-Memory.md)         | Allows you to Read, Write, Allocate & Change Memory Protection for Current Process. |
| [ExternalMemory](./About-Memory.md) | Read, Write, Allocate & Change Memory Protection but for Another Process.           |

**Streams Management:**

| Action                                                                                                                        | Description                                     |
|-------------------------------------------------------------------------------------------------------------------------------|-------------------------------------------------|
| BigEndian([Reader](./Streams/EndianReaders/BigEndianReader.md)/[Writer](./Streams/EndianReaders/BigEndianWriter.md))          | Read/write raw data in memory as Big Endian.    |
| LittleEndian([Reader](./Streams/EndianReaders/LittleEndianReader.md)/[Writer](./Streams/EndianReaders/LittleEndianWriter.md)) | Read/write raw data in memory as Little Endian. |
| [BufferedStreamReader](./Streams/BufferedStreamReader.md)                                                                     | High performance alternative to `BinaryReader`. |

**Extensions:**

| Action                                                                                      | Description                                                                       |
|---------------------------------------------------------------------------------------------|-----------------------------------------------------------------------------------|
| ([Array](./Extensions/ArrayExtensions.md)/[Span](./Extensions/SpanExtensions.md))Extensions | Unsafe slicing, references without bounds checks and SIMD accelerated extensions. |
| [StreamExtensions](./Extensions/StreamExtensions.md)                                        | Extensions for reading and writing from/to generics.                              |
| [StringExtensions](./Extensions/StringExtensions.md)                                        | Custom Hash Function(s) and unsafe character references.                          |

**Utilities:**

| Action                                                       | Description                                                                            |
|--------------------------------------------------------------|----------------------------------------------------------------------------------------|
| [ArrayRental & ArrayRentalSlice](./Utilities/ArrayRental.md) | Safe wrapper around `ArrayPool<T>` rentals.                                            |
| [Box<T>](./Utilities/Box.md)                                 | Represents a boxed value type, providing build-time validation and automatic unboxing. |
| [CircularBuffer](./Utilities/CircularBuffer.md)              | Basic high-performance circular buffer.                                                |
| [Pinnable<T>](./Utilities/Pinnable.md)                       | Utility for pinning C# objects for access from native code.                            |

**Base building blocks:**

| Action                                                                                   | Description                                     |
|------------------------------------------------------------------------------------------|-------------------------------------------------|
| [Ptr&lt;T&gt; / MarshalledPtr&lt;T&gt;](./Pointers/Ptr.md)                               | Abstraction over a pointer to arbitrary source. |
| [FixedArrayPtr&lt;T&gt; & MarshalledFixedArrayPtr&lt;T&gt;](./Pointers/FixedArrayPtr.md) | Abstraction over a pointer with known length.   |

(This list is not exhaustive, please see the API Documentation for complete API)

## Community Feedback

If you have questions/bug reports/etc. feel free to [Open an Issue](https://github.com/Reloaded-Project/Reloaded.Memory/issues/new).

Contributions are welcome and encouraged. Feel free to implement new features, make bug fixes or suggestions so long as 
they meet the quality standards set by the existing code in the repository.  

For an idea as to how things are set up, [see Reloaded Project Configurations.](https://github.com/Reloaded-Project/Reloaded.Project.Configurations)  

Happy Hacking 💜