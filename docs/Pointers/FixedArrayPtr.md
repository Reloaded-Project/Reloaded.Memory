# FixedArrayPtr&lt;T&gt; and MarshalledFixedArrayPtr&lt;T&gt;

!!! info "Abstraction for a pointer to a value of type `T` with a known length."

!!! warning "Zero overhead but not 1:1 codegen in all scenarios. e.g. Multiplying an element in place via Setter can be slower (.NET 7)."

A helper to allow you to manage a collection of items in unmanaged, static memory; for example, find an item.

## Properties

- `Pointer`: A [Ptr<T>](./Ptr.md) representing the memory address of the first element in the fixed-size array.  
- `Count`: The number of elements in the fixed-size array.  
- `ArraySize`: The total size in bytes of the fixed-size array.  

## Examples

### Initialization

```csharp
var ptr = new FixedArrayPtr<int>(address, count);
```

- `Address`: The memory address of the first element in the fixed-size array.
- `Count`: The number of elements in the fixed-size array.

### Element Access

!!! note "The following methods should be used with in memory/RAM addresses. See [Memory Read/Write](#memory-readwrite) for APIs that work with `ICanReadWriteMemory` implementations."

```csharp
ref T AsRef(int index);
T Get(int index);
void Get(int index, out T value);
void Set(int index, in T value);
```

These methods provide access to individual elements in the fixed-size array. The `AsRef()` method returns a reference to 
an element, allowing for direct modification of the element's value. The `Get()` and `Set()` methods retrieve and update 
the values of elements at specified indices.

### Memory Read/Write

```csharp
T Get<TSource>(TSource source, int index);
void Get<TSource>(TSource source, int index, out T value);
void Set<TSource>(TSource source, int index, in T value);
```
These methods provide access to individual elements in the fixed-size array, allowing read and write operations to be 
performed on an implementation of `ICanReadWriteMemory`. This can be useful when working with external processes or 
other memory sources.

### Copying

```csharp
void CopyFrom(Span<T> sourceArray, int length);
void CopyFrom(Span<T> sourceArray, int length, int sourceIndex, int destinationIndex);
void CopyTo(Span<T> destinationArray, int length);
void CopyTo(Span<T> destinationArray, int length, int sourceIndex, int destinationIndex);
```

These methods provide copying functionality between the fixed-size array and managed arrays. `CopyFrom()` copies elements 
from a managed array to the fixed-size array, and `CopyTo()` copies elements from the fixed-size array to a managed array.  

Both methods support copying a specified number of elements, as well as specifying source and destination indices for 
the copy operation.


### Search

```csharp
bool Contains(in T item);
bool Contains<TSource>(TSource source, in T item);
int IndexOf(in T item);
int IndexOf<TSource>(TSource source, in T item);
```

These methods provide search functionality for the fixed-size array, allowing you to check for the existence of an element 
or find its index. The `Contains()` methods return true if the specified item is found in the array, and the `IndexOf()` methods 
return the index of the first occurrence of the specified item or -1 if the item is not found.

## SourcedFixedArrayPtr&lt;T&gt;

!!! info "This is a tuple of `FixedArrayPtr<T>` and `TSource`."

API surface is the same, all methods are delegated to underlying `FixedArrayPtr<T>`.  

Notably however, this API enables the use of LINQ; i.e. the following are now valid:

```csharp
foreach (int value in sourcedFixedArrayPtr)
{
    // ...
}
```

```csharp
sourcedFixedArrayPtr.Max(x => x);
```

etc.