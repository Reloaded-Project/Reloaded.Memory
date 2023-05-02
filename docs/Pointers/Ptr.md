# Ptr&lt;T&gt;

!!! info "Abstraction for a pointer to a value of type `T`."

!!! note "This type was formerly called BlittablePointer<T> in old library versions."

A helper to allow you to use pointers with generics.  
This type allows you to make extension methods for pointers; and is mainly used with external libraries.  

## Example

Getting a ref to native memory/pointer:  
```csharp
// Defined in a library for modding a game.
public static readonly Ptr<int> NumberOfRacers = new Ptr<int>((int*)0x64B758);

// End user can do either
int* racers = NumberOfRacers;

// or avoid pointer arithmetic entirely.
ref int racers = ref NumberOfRacers.AsRef();
```

Hooking a function with [Reloaded.Hooks](https://github.com/Reloaded-Project/Reloaded.Memory/blob/master/Docs/Getting-Started.md):  

```csharp
// Function pointer declatation (can also use delegate).
[Function(CallingConventions.MicrosoftThiscall)]
public struct OpenFileFnPtr { public FuncPtr<Ptr<byte>, Ptr<byte>, int, Ptr<byte>> Value; }

_openFile = FileSystemFuncs.OpenFile.HookAs<FileSystemFuncs.OpenFileFnPtr>(typeof(FileAccessServer), nameof(OpenBfsFileImpl)).Activate();
```

