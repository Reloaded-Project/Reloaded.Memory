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
		<img src="https://img.shields.io/github/actions/workflow/status/Reloaded-Project/Reloaded.Memory/build-and-publish.yml?branch=main" alt="Build Status" />
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

This project guarantees binary backwards compatibility; meaning you can substitute the library with any newer version
without recompiling the source code. Should the need to introduce any breaking changes occur; much like the runtime.

## Community Feedback

If you have questions/bug reports/etc. feel free to [Open an Issue](https://github.com/Reloaded-Project/Reloaded.Memory/issues/new).

Contributions are welcome and encouraged. Feel free to implement new features, make bug fixes or suggestions so long as 
they meet the quality standards set by the existing code in the repository.  

For an idea as to how things are set up, [see Reloaded Project Configurations.](https://github.com/Reloaded-Project/Reloaded.Project.Configurations)  

Happy Hacking 💜