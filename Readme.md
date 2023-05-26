
<div align="center">
	<h1>The Reloaded Memory Library</h1>
	<img src="https://raw.githubusercontent.com/Reloaded-Project/Reloaded.MkDocsMaterial.Themes.R2/adc12754862c5107fcd1357c7501e4d9d9f09d07/Images/Reloaded-Icon.png" width="150" align="center" />
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

- **Zero Cost Abstractions:** Performance equivalent to using raw pointers.  
- **Stable API:** Versions 9.0.0 and above have a fully stable, backwards compatible API.  
- **Trimming Safe:** The library is fully compatible with .NET Core's Linker.  
- **Fully Documented:** The library is fully tested & documented with XML comments.  
- **Cross Platform:** 99% of the library is fully compatible with Windows, Linux and MacOS.  
- **Large Address Aware:** The library can correctly leverage all 4GB in x86 processes.  

This project guarantees binary backwards compatibility; meaning you can substitute the library with any newer version
without recompiling the source code. Should the need to introduce any breaking changes occur; much like the runtime.

## Common Utilities

Common Classes within this Package Include:  

**Memory Manipulation:**  

| Action           | Description                                                                         |
|------------------|-------------------------------------------------------------------------------------|
| `Memory`         | Allows you to Read, Write, Allocate & Change Memory Protection for Current Process. |
| `ExternalMemory` | Read, Write, Allocate & Change Memory Protection but for Another Process.           |

**Streams Management:**  

| Action                        | Description                                     |
|-------------------------------|-------------------------------------------------|
| `BigEndian(Reader/Writer)`    | Read/write raw data in memory as Big Endian.    |
| `LittleEndian(Reader/Writer)` | Read/write raw data in memory as Little Endian. |
| `BufferedStreamReader`        | High performance alternative to `BinaryReader`. |

**Extensions:**  

| Action                   | Description                                                                       |
|--------------------------|-----------------------------------------------------------------------------------|
| `(Array/Span)Extensions` | Unsafe slicing, references without bounds checks and SIMD accelerated extensions. |
| `StreamExtensions`       | Extensions for reading and writing from/to generics.                              |
| `StringExtensions`       | Custom Hash Function(s) and unsafe character references.                          |

**Utilities:**  

| Action                             | Description                                                                            |
|------------------------------------|----------------------------------------------------------------------------------------|
| `ArrayRental` & `ArrayRentalSlice` | Safe wrapper around `ArrayPool<T>` rentals.                                            |
| `Box<T>`                           | Represents a boxed value type, providing build-time validation and automatic unboxing. |
| `CircularBuffer`                   | Basic high-performance circular buffer.                                                |
| `Pinnable<T>`                      | Utility for pinning C# objects for access from native code.                            |

**Base building blocks:**  

| Action                                          | Description                                     |
|-------------------------------------------------|-------------------------------------------------|
| `Ptr<T> / MarshalledPtr<T>`                     | Abstraction over a pointer to arbitrary source. |
| `FixedArrayPtr<T> & MarshalledFixedArrayPtr<T>` | Abstraction over a pointer with known length.   |

(This list is not exhaustive, please see the API Documentation for complete API)

## Wiki & Documentation

[For more information on how to use the library, please see the Wiki](https://reloaded-project.github.io/Reloaded.Memory/).  
The wiki contains a lot of useful information on how to use the library, as well as many examples.  

## Community Feedback

If you have questions/bug reports/etc. feel free to [Open an Issue](https://github.com/Reloaded-Project/Reloaded.Memory/issues/new).

Contributions are welcome and encouraged. Feel free to implement new features, make bug fixes or suggestions so long as
they meet the quality standards set by the existing code in the repository.

For an idea as to how things are set up, [see Reloaded Project Configurations.](https://github.com/Reloaded-Project/Reloaded.Project.Configurations)

Happy Hacking 💜