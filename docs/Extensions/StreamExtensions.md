# StreamExtensions

!!! info "Utility class providing high performance extension methods for working with `Stream` objects."

StreamExtensions is a utility class that provides extension methods for the `Stream` type, including padding, writing, and reading unmanaged and marshalled structures.

## Methods

### AddPadding
```csharp
public static void AddPadding<TStream>(this TStream stream, int alignment)
```
Pads the stream with any/random bytes until it is aligned to the specified alignment. The padding bytes depend on the implementation of `Stream.SetLength`.

### AddPadding
```csharp
public static void AddPadding<TStream>(this TStream stream, byte value, int alignment = 2048)
```
Pads the stream with the specified `value` bytes until it is aligned to the specified alignment.

### Write
```csharp
public static void Write<TStream, T>(this TStream stream, Span<T> structure) where T : unmanaged
```
Appends an unmanaged structure onto the given stream and advances the position.

### Write
```csharp
public static void Write<TStream, T>(this TStream stream, in T structure) where T : unmanaged
```
Appends an unmanaged structure onto the given stream and advances the position.

### WriteMarshalled
```csharp
public static void WriteMarshalled<TStream, T>(this TStream stream, T item)
```
Appends a managed/marshalled structure onto the given stream and advances the position.

### WriteMarshalled
```csharp
public static void WriteMarshalled<TStream, T>(this TStream stream, T[] item)
```
Appends an array of managed/marshalled structures onto the given stream and advances the position.

### WriteMarshalled
```csharp
public static void WriteMarshalled<TStream, T>(this TStream stream, Span<T> item)
```
Appends a span of managed/marshalled structures onto the given stream and advances the position.

### Read
```csharp
public static void Read<TStream, T>(this TStream stream, out T result) where T : unmanaged
```
Reads a single unmanaged structure of type T from the stream.

### Read
```csharp
public static void Read<TStream, T>(this TStream stream, T[] output) where T : unmanaged
```
Reads a span of unmanaged structures of type T from the stream into the provided output array.

### Read
```csharp
public static void Read<TStream, T>(this TStream stream, Span<T> output) where T : unmanaged
```
Reads a span of unmanaged structures of type T from the stream into the provided output span.

### ReadMarshalled
```csharp
public static void ReadMarshalled<TStream, T>(this TStream stream, out T result)
```
Reads a single marshalled structure of type T from the stream.

### ReadMarshalled
```csharp
public static void ReadMarshalled<TStream, T>(this TStream stream, T[] output)
```
Reads a span of marshalled structures of type T from the stream into the provided output array.

### ReadMarshalled

```csharp
public static void ReadMarshalled<TStream, T>(this TStream stream, Span<T> output)
```
Reads a span of marshalled structures of type T from the stream into the provided output span.

## Usage

### Pad Stream with Random Bytes
```csharp
using var stream = new MemoryStream();
stream.AddPadding(4096);
```

### Pad Stream with Specific Byte Value
```csharp
using var stream = new MemoryStream();
stream.AddPadding(0xFF, 4096);
```

### Write Unmanaged Structure
```csharp
using var stream = new MemoryStream();
Vector2 structure = new Vector2(1, 2);
stream.Write(structure);
```

### Write Marshalled Structure
```csharp
using var stream = new MemoryStream();
MyStruct item = new MyStruct { Value1 = 1, Value2 = "Hello" };
stream.WriteMarshalled(item);
```

### Read Unmanaged Structure
```csharp
using var stream = new MemoryStream();
stream.Read(out Vector2 result);
```

### Read Marshalled Structure
```csharp
using var stream = new MemoryStream();
stream.ReadMarshalled(out MyStruct result);
```

### Read Span of Unmanaged Structures
```csharp
using var stream = new MemoryStream();
Vector2[] output = new Vector2[10];
stream.Read(output);
```

### Read Span of Marshalled Structures
```csharp
using var stream = new MemoryStream();
MyStruct[] output = new MyStruct[10];
stream.ReadMarshalled(output);
```