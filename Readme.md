

<div align="center">
	<h1>Project Reloaded: Memory Library</h1>
	<img src="https://i.imgur.com/BjPn7rU.png" width="150" align="center" />
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
	<a href="https://ci.appveyor.com/project/sewer56lol/reloaded-memory">
		<img src="https://ci.appveyor.com/api/projects/status/qlks2pma4lrr6c4f?svg=true" alt="Build Status" />
	</a>
</div>

# Introduction
Reloaded.Memory is a managed, high performance, fully featured memory manipulation library written in C#, providing a very easy to use API.

## Features
This is not a true feature set, rather a list of ideas as to what you can do/should expect with this library:

+ Write/Read *generic* structures to/from memory..
+ Write/Read *generic* structure arrays to/from memory.
+ Obtain the size of a *generic* data type before/after marshalling.
+ Convert *generic* structures to *byte[]*, convert *byte[]* into *generic* structures.
+ *Allocate*, *Free*, *Change Permissions* of regions of memory in current or another process.
+ Use LINQ over structure arrays in memory of current or another process. 
+ High performance generic stream implementations, including big endian support.
+ Utility classes, e.g. Circular Buffers.


## Documentation

The following below are links aimed to help you get started with the library, they cover the basics of use:

+ [Getting Started](Docs/Getting-Started.md)

For extra ideas of how to use the library, you may always take a look at `Reloaded.Memory.Tests`, the test suite for the main library.

## Contributions
As with the standard for all of the `Reloaded-Project`, repositories; contributions are very welcome and encouraged.

Feel free to implement new features, make bug fixes or suggestions so long as they are accompanied by an issue with a clear description of the pull request.

If you are implementing new features, please do provide the appropriate unit tests to cover the new features you have implemented; try to keep the coverage near 100% 😊.
