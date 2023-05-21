# Ptr&lt;T&gt; and MarshalledPtr&lt;T&gt;

!!! info "Abstraction for a pointer to a value of type `T`."

!!! note "This type was formerly called `BlittablePointer<T>` and `ArrayPtr<T>` in old library versions."

!!! warning "Zero overhead but not 1:1 codegen in all scenarios. e.g. Multiplying an element in place via Setter can be slower (.NET 7)."

A helper to allow you to do two things:  

- Use pointer arithmetic on anything implementing `ICanReadWriteMemory`. (e.g. Memory of External Process).  
- To use in generics.  

The type `MarshalledPtr<T>` also exists, this is a special type of `Ptr<T>` that marshals the elements as they are read/written;
with pointer arithmetic being performed on the marshalled size of the object rather than the raw size.

## Examples

!!! tip "Basically, the `Ptr<T>` type is just a regular pointer, with all of the operations you would expect from a pointer; except that it can be used in Generics, and also read/write to more than just RAM."

### Initialization

!!! info "The `Ptr<T>` type supports implicit conversions to/from raw pointers."

```csharp
int someValue = 42;
Ptr<int> ptr = &someValue;
int* ptr2 = intPtr;
```

### AsRef

!!! info "Pointers can be converted for references."

```csharp
ref int valueFromPtr = ref intPtr.AsRef();
Console.WriteLine($"Value from pointer: {valueFromPtr}");
```

This can be especially useful for game modding APIs, the end user can for example do this:

```csharp
ref var playerCount = ref State.NumberOfPlayers.AsRef();
playerCount = 5;
```

Completely avoiding pointer arithmetic; which is friendly to non-programmers.

### Pointer Arithmetic

!!! info "You can do regular arithmetic on pointers, e.g. add 1 to go to next value."

```csharp
Ptr<int> arrayIntPtr = arrayPtr;
Ptr<int> offsetArrayIntPtr = arrayIntPtr + 2;

Console.WriteLine($"Value at original pointer: {arrayIntPtr.AsRef()}"); // Output: Value at original pointer: 1
Console.WriteLine($"Value at offset pointer: {offsetArrayIntPtr.AsRef()}"); // Output: Value at offset pointer: 3
Console.WriteLine($"Equal? {offsetArrayIntPtr != arrayIntPtr}"); // Equal? false

// You can also do ++ and --
arrayIntPtr++;
arrayIntPtr++;

// Now arrayIntPtr points to the third element in the array.
```

### Value Read / Write

!!! info "You can read/write values with an implementation of `ICanReadWriteMemory`"

Reading from RAM:  

```csharp
int valueFromSource = pointer.Get(); // implicit Memory.Instance
Console.WriteLine($"Value from RAM: {valueFromSource}");
```

Reading from RAM of another process:  

```csharp
// externalMemory = new ExternalMemory(anotherProcess);
int valueFromSource = pointer.Get(externalMemory);
Console.WriteLine($"Value from another process' RAM: {valueFromSource}");
```

You can also read/write to offsets:

```csharp
int valueAtOffset2 = pointer.Get(2);
Console.WriteLine($"Value from RAM (Offset 2): {valueAtOffset2}");
```

### Branch on Null Pointer

!!! info "Just like in C, you can branch into an if statement if a pointer isn't null."

```csharp
var notNullPointer = new Ptr<int>((int*)0x12345678);
if (notNullPointer)
    Console.WriteLine("Pointer is not null!");
```

### From External Libraries

Getting a ref to native memory/pointer:  

```csharp
// Defined in a library for modding a certain hoverboard racing game.
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

## SourcedPtr&lt;T&gt;

!!! info "This is a tuple of `Ptr<T>` and `TSource`."

Basically, it allows you to assign a `TSource` to a `Ptr<T>` and skip passing `TSource` as a parameter.  

Remaining usage is the same.