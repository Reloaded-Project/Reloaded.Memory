# Pinnable&lt;T&gt;

!!! info "Allows you to pin an unmanaged object in a static location in memory, to be later accessible from native code."

!!! warning "Assume value is copied; modify the Pinnable instance to read/write values once created."

The `Pinnable<T>` class provides a way to pin native unmanaged objects in memory, ensuring that their memory addresses 
remain constant during the lifetime of the `Pinnable<T>` instance. This can be useful when working with native 
code that requires static memory addresses.  

On newer runtimes, the memory is allocated into the `Pinned Object Heap (POH)`; thus has no impact on effectiveness
of regular garbage collection.

## Properties

- `Value`: The value pointed to by the `Pointer`. If the class was instantiated using an array, this is the first element of the array.
- `Pointer`: Pointer to the native value in question. If the class was instantiated using an array, this is the pointer to the first element of the array.

## Constructors

```csharp
public Pinnable(T[] value);   // Pins an array of values to the heap.
public Pinnable(in T value);     // Pins a single value to the heap.
```

## Disposal

The `Pinnable<T>` class implements the `IDisposable` interface, which means you should dispose of the instance when you are done with it.  

This can be done using the `using` statement, or by explicitly calling the `Dispose()` method.  

## Examples

### Pinning a Single Value

```csharp
int value = 42;

using var pinnable = new Pinnable<int>(value);
// Access the pinned value through pinnable.Value
// Access the memory address of the pinned value through pinnable.Pointer
```

### Pinning an Array

```csharp
int[] array = new int[] { 1, 2, 3, 4, 5 };

using var pinnable = new Pinnable<int>(value);
// Access array element via `pinnable[x]`.
// Access the first element of the pinned array through pinnable.Value
// Access the memory address of the first element of the pinned array through pinnable.Pointer
```