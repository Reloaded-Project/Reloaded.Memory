# BufferedStreamReader Class

!!! info "The `BufferedStreamReader` is a buffering mechanism for reading data from streams, allowing for fast reading of data while preserving stream-like semantics."

!!! tip "`BufferedStreamReader` is a high-performance replacement for [BinaryReader](https://learn.microsoft.com/en-us/dotnet/api/system.io.binaryreader?view=net-7.0) with minimal error checking."

!!! warning "`BufferedStreamReader` is not thread-safe."

!!! warning "`BufferedStreamReader` cannot read values larger than buffer size it was initialised with."

!!! warning "`BufferedStreamReader` has minimal error checking at runtime."

!!! tip 

    Remember, always ensure the stream and reader are properly disposed of after use. 
    The best practice is to use the `using` statement or `try-finally` block in C# to ensure resources are correctly released.

## Performance

Amount of time taken to read 8 MiB of data (library version 9.0.0):

Legend:  
- `BinaryReader` read via BinaryReader.  
- `BufferedStreamReader` read via BufferedStreamReader.  
- `BufferedStreamReader` read via BufferedStreamReader (ReadRaw method).  
- `NativePointer` no stream; no copy; access existing data from array (baseline reference).  

### MemoryStream

Benchmarks on MemoryStream (near zero overhead) allow us to compare the performance against `BinaryReader` and 
array access (baseline reference).  

`Byte`:  

| Method                   |        Mean | Code Size |
|--------------------------|------------:|----------:|
| BinaryReader             | 15,452.7 us |     171 B |
| BufferedStreamReader     |  6,298.7 us |     703 B |
| BufferedStreamReader Raw |  1,997.3 us |     669 B |
| NativePointer            |  1,845.2 us |      38 B |

`Int`:  

| Method                   |        Mean | Code Size |
|--------------------------|------------:|----------:|
| BinaryReader             |  3,253.2 us |     637 B |
| BufferedStreamReader     |  1,708.7 us |     981 B |
| BufferedStreamReader Raw |    606.8 us |     760 B |
| NativePointer            |    464.1 us |      54 B |

`Long`:  

| Method                   |        Mean | Code Size |
|--------------------------|------------:|----------:|
| BinaryReader             |  1,665.2 us |     639 B |
| BufferedStreamReader     |    890.8 us |     709 B |
| BufferedStreamReader Raw |    370.7 us |     761 B |
| NativePointer            |    227.3 us |      55 B |

### FileStream

Benchmarks on FileStream allow us to compare the performance against `BinaryReader` more closely.

`Byte`:  

| Method                   |        Mean | Code Size |
|--------------------------|------------:|----------:|
| BinaryReader             | 21,309.7 us |     169 B |
| BufferedStreamReader     |  6,731.9 us |   1,052 B |
| BufferedStreamReader Raw |  2,763.3 us |     661 B |


`Int`:  

| Method                   |        Mean | Code Size |
|--------------------------|------------:|----------:|
| BinaryReader             | 22,279.2 us |     640 B |
| BufferedStreamReader     |  2,219.4 us |   1,487 B |
| BufferedStreamReader Raw |  1,310.4 us |     753 B |

`Long`:  

| Method                   |        Mean | Code Size |
|--------------------------|------------:|----------:|
| BinaryReader             | 13,025.8 us |     642 B |
| BufferedStreamReader     |  1,425.6 us |   1,068 B |
| BufferedStreamReader Raw |    943.7 us |     754 B |

## Properties

- `BaseStream`: The stream this class was instantiated with.
- `BufferBytesAvailable`: The remaining number of bytes that are currently buffered.
- `CurrentBufferSize`: The total size of the current buffered data at this moment in time.
- `IsEndOfStream`: This is true if end of stream was reached while refilling the internal buffer.
- `OnEndOfStream`: This method is executed if a buffer refill does not fill the whole buffer, indicating end of stream was reached.

## Constructors

- `BufferedStreamReader(TStream stream, int bufferSize = 65536)`: Constructs a `BufferedStreamReader`.

## Methods

!!! note "Just like with regular Stream APIs, less data can be returned than requested if end of stream was reached. Please note `BufferedStreamReader` does not throw in these scenarios."

### Seek

```csharp
public void Seek(long offset, SeekOrigin origin)
```

Seeks the underlying stream to a specified position.

### Advance

```csharp
public void Advance(long offset)
```

Advances the underlying stream by a specified number of bytes.  
(This is equivalent to `Seek(offset, SeekOrigin.Current)`)

### Read

```csharp
public T Read<T>() where T : unmanaged
public void Read<T>(out T value) where T : unmanaged
```

Reads an unmanaged, generic type from the stream.

### ReadRaw

!!! warning "The returned values point to internal buffers. *DO NOT MODIFY THE DATA!*"

```csharp
public byte* ReadRaw(int length, out int available)
public T* ReadRaw<T>(int numItems, out int available) where T : unmanaged
```

Provides a pointer to the buffered data; buffering sufficient data if needed.

### ReadRaw (with Output)

!!! info "Variant of [ReadRaw](#readraw) that copies the data to user's own destination."

```csharp
public int ReadRaw<T>(Span<T> buffer) where T : unmanaged
public int ReadRaw<T>(T* buffer, int numItems) where T : unmanaged
```

Reads raw data from the stream, without conversion. The output is written to the supplied pointer or span.

### ReadBytesUnbuffered

!!! info "This method is useful for reading large blobs (e.g. a compressed file from an archive) without discarding the buffered data."

```csharp
public int ReadBytesUnbuffered(long offset, Span<byte> data)
```

Reads a specified amount of bytes at a specific offset from the underlying stream without resetting the buffers or advancing the read pointer.

### ReadMarshalled

```csharp
public T ReadMarshalled<T>()
```

Reads a value that requires marshalling from the stream.

### Peek

```csharp
public T Peek<T>() where T : unmanaged
public void Peek<T>(out T value) where T : unmanaged
```

Reads an unmanaged, generic type from the stream without incrementing the position.

### PeekMarshalled

```csharp
public T PeekMarshalled<T>()
public void PeekMarshalled<T>(out T value)
```

Reads a value that requires marshalling from the stream without advancing the position.

## Methods (Endian Extensions)

### PeekLittleEndian

```csharp
public Int16 PeekLittleEndianInt16()
public Int16 PeekLittleEndian(out Int16 value)
public UInt16 PeekLittleEndianUInt16()
public UInt16 PeekLittleEndian(out UInt16 value)
public Int32 PeekLittleEndianInt32()
public Int32 PeekLittleEndian(out Int32 value)
public UInt32 PeekLittleEndianUInt32()
public UInt32 PeekLittleEndian(out UInt32 value)
public Int64 PeekLittleEndianInt64()
public Int64 PeekLittleEndian(out Int64 value)
public UInt64 PeekLittleEndianUInt64()
public UInt64 PeekLittleEndian(out UInt64 value)
public Single PeekLittleEndianSingle()
public Single PeekLittleEndian(out Single value)
public Double PeekLittleEndianDouble()
public Double PeekLittleEndian(out Double value)
```

Peeks a little endian value of the specified type from the stream without incrementing the position.

### PeekBigEndian

```csharp
public Int16 PeekBigEndianInt16()
public Int16 PeekBigEndian(out Int16 value)
public UInt16 PeekBigEndianUInt16()
public UInt16 PeekBigEndian(out UInt16 value)
public Int32 PeekBigEndianInt32()
public Int32 PeekBigEndian(out Int32 value)
public UInt32 PeekBigEndianUInt32()
public UInt32 PeekBigEndian(out UInt32 value)
public Int64 PeekBigEndianInt64()
public Int64 PeekBigEndian(out Int64 value)
public UInt64 PeekBigEndianUInt64()
public UInt64 PeekBigEndian(out UInt64 value)
public Single PeekBigEndianSingle()
public Single PeekBigEndian(out Single value)
public Double PeekBigEndianDouble()
public Double PeekBigEndian(out Double value)
```

Peeks a big endian value of the specified type from the stream without incrementing the position.

### ReadLittleEndian

```csharp
public Int16 ReadLittleEndianInt16()
public Int16 ReadLittleEndian(out Int16 value)
public UInt16 ReadLittleEndianUInt16()
public UInt16 ReadLittleEndian(out UInt16 value)
public Int32 ReadLittleEndianInt32()
public Int32 ReadLittleEndian(out Int32 value)
public UInt32 ReadLittleEndianUInt32()
public UInt32 ReadLittleEndian(out UInt32 value)
public Int64 ReadLittleEndianInt64()
public Int64 ReadLittleEndian(out Int64 value)
public UInt64 ReadLittleEndianUInt64()
public UInt64 ReadLittleEndian(out UInt64 value)
public Single ReadLittleEndianSingle()
public Single ReadLittleEndian(out Single value)
public Double ReadLittleEndianDouble()
public Double ReadLittleEndian(out Double value)
```

Reads a little endian value of the specified type from the stream and advances the position by the size of the type.

### ReadBigEndian

```csharp
public Int16 ReadBigEndianInt16()
public Int16 ReadBigEndian(out Int16 value)
public UInt16 ReadBigEndianUInt16()
public UInt16 ReadBigEndian(out UInt16 value)
public Int32 ReadBigEndianInt32()
public Int32 ReadBigEndian(out Int32 value)
public UInt32 ReadBigEndianUInt32()
public UInt32 ReadBigEndian(out UInt32 value)
public Int64 ReadBigEndianInt64()
public Int64 ReadBigEndian(out Int64 value)
public UInt64 ReadBigEndianUInt64()
public UInt64 ReadBigEndian(out UInt64 value)
public Single ReadBigEndianSingle()
public Single ReadBigEndian(out Single value)
public Double ReadBigEndianDouble()
public Double ReadBigEndian(out Double value)
```

Reads a big endian value of the specified type from the stream and advances the position by the size of the type.

### AsLittleEndian

!!! tip "Implements, `IEndianedBufferStreamReader<TStream>`. Use constraint `where T : IEndianedBufferStreamReader<TStream>` to write endian agnostic code without any overhead."

```csharp
public LittleEndianBufferedStreamReader<TStream> AsLittleEndian();
```

Returns a reader that can be used to read little endian values from the stream.  

### AsBigEndian

!!! tip "Implements, `IEndianedBufferStreamReader<TStream>`. Use constraint `where T : IEndianedBufferStreamReader<TStream>` to write endian agnostic code without any overhead."

```csharp
public BigEndianBufferedStreamReader<TStream> AsBigEndian();
```

Returns a reader that can be used to read big endian values from the stream.  

## Methods (Endian Struct Extensions)

!!! info "The following methods are valid for structs which implement `ICanReverseEndian`"

### PeekLittleEndianStruct

```csharp
public T PeekLittleEndianStruct<T>() where T : unmanaged, ICanReverseEndian
public void PeekLittleEndianStruct<T>(out T value) where T : unmanaged, ICanReverseEndian
```

Peeks a little endian unmanaged, generic type from the stream without incrementing the position.

### PeekBigEndianStruct

```csharp
public T PeekBigEndianStruct<T>() where T : unmanaged, ICanReverseEndian
public void PeekBigEndianStruct<T>(out T value) where T : unmanaged, ICanReverseEndian
```

Peeks a big endian unmanaged, generic type from the stream without incrementing the position.

### ReadLittleEndianStruct

```csharp
public T ReadLittleEndianStruct<T>() where T : unmanaged, ICanReverseEndian
public void ReadLittleEndianStruct<T>(out T value) where T : unmanaged, ICanReverseEndian
```

Reads a little endian unmanaged, generic type from the stream.

### ReadBigEndianStruct

```csharp
public T ReadBigEndianStruct<T>() where T : unmanaged, ICanReverseEndian
public void ReadBigEndianStruct<T>(out T value) where T : unmanaged, ICanReverseEndian
```

Reads a big endian unmanaged, generic type from the stream.

## Examples

### Creating a BufferedStreamReader

```csharp
var stream = File.OpenRead("myFile.txt");
using var reader = new BufferedStreamReader<FileStream>(stream);
```

### Reading an Integer from the Stream

```csharp
int value = reader.Read<int>();
Console.WriteLine($"Read value: {value}");
```

### Reading Raw Bytes

!!! warning "[Do not modify the returned data!](#readraw) Copy it elsewhere first!"

```csharp
int length = 10;
int available;
byte* rawBytes = reader.ReadRaw(length, out available);
```

### Reading Raw Structs

```csharp
using var reader = new BufferedStreamReader<FileStream>(fileStream);
Span<Vector3> span = stackalloc Vector3[10];
int readItems = reader.ReadRaw(span);

Console.WriteLine($"{readItems} Vector3 items were read from the stream.");
```

In this example, we create a `BufferedStreamReader` using a `FileStream` and then declare a `Span<Vector3>` where `Vector3` 
is a struct representing a 3D vector. We then read from the stream directly into the `Span<Vector3>`. After reading, we 
print out how many `Vector3` items were read from the stream.

### Seeking and Advancing Stream

```csharp
var reader = new BufferedStreamReader<FileStream>(fileStream);
reader.Seek(100, SeekOrigin.Begin); // Seek to 100 bytes from the start
reader.Advance(50); // Advance 50 bytes from the current position

Console.WriteLine($"Current stream position is {reader.Position()}");
```

In this example, we first seek to 100 bytes from the start of the stream. Then we advance 50 bytes from the current position. After that, we print out the current stream position.

### Reading Unmanaged Types

```csharp
using var reader = new BufferedStreamReader<FileStream>(fileStream);
int value = reader.Read<int>(); // Reads an integer from the stream

Console.WriteLine($"The read integer value is {value}");
```

In this example, we read an integer directly from the stream and print it out.

### Reading Large Raw Data

```csharp
using var reader = new BufferedStreamReader<FileStream>(fileStream);
Span<byte> dataSpan = stackalloc byte[1024];
int bytesRead = reader.ReadBytesUnbuffered(200, dataSpan);

Console.WriteLine($"{bytesRead} bytes were read from the stream.");
```

In this example, we read 1024 bytes starting from 200 bytes offset into a `Span<byte>` without resetting the buffers or 
advancing the read pointer. After reading, we print out how many bytes were read from the stream.

!!! note "Value of `bytesRead` may be less than length of `dataSpan` if end of stream was reached." 

### Peeking Data

```csharp
using var reader = new BufferedStreamReader<FileStream>(fileStream);
int value = reader.Peek<int>(); // Peeks an integer from the stream

Console.WriteLine($"The peeked integer value is {value}. Stream did not advance.");
```

### Using Customized Buffer Size

!!! note "The default buffer size of 64KBytes should be sufficient for most use cases, including file reads."

```csharp
int bufferSize = 8192; // 8 KB
using var reader = new BufferedStreamReader<FileStream>(fileStream, bufferSize);

Console.WriteLine($"Custom buffer size of {bufferSize} bytes is used for the reader.");
```

In this example, a custom buffer size is set when creating a `BufferedStreamReader`. This allows you to tune the buffer 
size to match your specific use case, potentially improving performance.

### Combining Multiple Operations

```csharp
var reader = new BufferedStreamReader<FileStream>(fileStream);
reader.Seek(100, SeekOrigin.Begin); 
int value = reader.Read<int>(); 
reader.Advance(50); 
Vector3 vector = reader.Read<Vector3>();

Console.WriteLine($"Read integer value: {value}, Vector3: {vector.X}, {vector.Y}, {vector.Z}.");
```

In this final example, multiple operations are combined. First, the reader seeks to a specific position in the stream. 
Then, an integer is read, the reader advances a certain number of bytes, and finally, a `Vector3` struct is read. 
The results are then printed to the console.

## Examples (Extensions)

### Writing Endian Agnostic Code

!!! info "Shows you how to write code that works with both Big and Little Endian data."

```csharp
using var reader = new BufferedStreamReader<FileStream>(fileStream);

// Read in Big Endian
Read(reader.AsBigEndian());
// or... as Little Endian
Read(reader.AsLittleEndian());

private void Read<TReader>(TReader reader) where TReader : IEndianedBufferStreamReader
{
    // Some neat parsing code here.
    var i16 = reader.ReadInt16();
    var i32 = reader.PeekInt32();
}
```

This approach allows you to write code which uses the same logic for both big and little endian.  
You can either pass `reader.AsLittleEndian()` or `reader.AsBigEndian()` to your parsing code. Either way, your reader
will be devirtualized and you'll be able to parse away with 0 overhead.

### Reading Big/Little Endian Primitives

```csharp
using var reader = new BufferedStreamReader<FileStream>(fileStream);

// Little Endian
Int16 littleEndianInt16 = reader.PeekLittleEndianInt16();
Console.WriteLine($"The read Int16 value at offset 0 in Little Endian is {littleEndianInt16}");

// Big Endian
Int16 bigEndianInt16 = reader.PeekBigEndianInt16();
Console.WriteLine($"The read Int16 value at offset 0 in Big Endian is {bigEndianInt16}");
```

## Examples (Endian Struct Extensions)

!!! info "CustomStruct must implement `ICanReverseEndian`"

Example:

```csharp
public struct CustomStruct : ICanReverseEndian
{
    public int Value1;
    public int Value2;
    public int Value3;

    // ICanReverseEndian
    public void ReverseEndian()
    {
        Value1 = Endian.Reverse(Value1);
        Value2 = Endian.Reverse(Value2);
        Value3 = Endian.Reverse(Value3);
    }
}
```

### Reading a Little Endian Struct

```csharp
using var reader = new BufferedStreamReader<FileStream>(fileStream);
var value = reader.ReadLittleEndianStruct<CustomStruct>();

Console.WriteLine($"The read CustomStruct value is {value}");
```

In this example, a `CustomStruct` is read directly from the stream using little endian byte order.

### Reading a Big Endian Struct

```csharp
using var reader = new BufferedStreamReader<FileStream>(fileStream);
var value = reader.ReadBigEndianStruct<CustomStruct>();

Console.WriteLine($"The read CustomStruct value is {value}");
```

In this example, a `CustomStruct` is read directly from the stream using big endian byte order.

### Peeking a Little Endian Struct

```csharp
using var reader = new BufferedStreamReader<FileStream>(fileStream);
var value = reader.PeekLittleEndianStruct<CustomStruct>();

Console.WriteLine($"The peeked CustomStruct value is {value}. Stream did not advance.");
```

In this example, a `CustomStruct` is peeked from the stream without advancing the stream position, using little endian byte order.

### Peeking a Big Endian Struct

```csharp
using var reader = new BufferedStreamReader<FileStream>(fileStream);
var value = reader.PeekBigEndianStruct<CustomStruct>();

Console.WriteLine($"The peeked CustomStruct value is {value}. Stream did not advance.");
```

In this example, a `CustomStruct` is peeked from the stream without advancing the stream position, using big endian byte order.

!!! note "`CustomStruct` must be an unmanaged type and implement `ICanReverseEndian` interface."