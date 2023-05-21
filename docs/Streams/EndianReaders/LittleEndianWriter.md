# LittleEndianWriter

!!! info "Utility struct for writing to a pointer in Big Endian."

`LittleEndianWriter` is a struct providing methods for writing various types of data to a memory location, with the data
being written in `Big Endian` byte order. This includes writing integers, floating point values, and arrays of bytes,
either advancing the pointer after each write or writing at a specific offset without advancing the pointer.

## Properties

### Ptr

```csharp
public byte* Ptr;
```

Current pointer being written to.

## Constructors

### LittleEndianWriter

```csharp
public LittleEndianWriter(byte* ptr)
```

Creates a simple wrapper around a pointer that writes in Big Endian. `ptr` is the pointer to the item behind the writer.

## Methods

### Write

```csharp
public void Write(sbyte value)
public void Write(byte value)
public void Write(short value)
public void Write(ushort value)
public void Write(int value)
public void Write(uint value)
public void Write(long value)
public void Write(ulong value)
public void Write(float value)
public void Write(double value)
public void Write(Span<byte> data)
```

Writes a value to the current pointer and advances the pointer. Overloads exist for various data types including `sbyte`, `byte`, `short`, `ushort`, `int`, `uint`, `long`, `ulong`, `float`, `double`, and `Span<byte>`.

### WriteAtOffset

```csharp
public void WriteAtOffset(sbyte value, int offset)
public void WriteAtOffset(byte value, int offset)
public void WriteAtOffset(short value, int offset)
public void WriteAtOffset(ushort value, int offset)
public void WriteAtOffset(int value, int offset)
public void WriteAtOffset(uint value, int offset)
public void WriteAtOffset(long value, int offset)
public void WriteAtOffset(ulong value, int offset)
public void WriteAtOffset(float value, int offset)
public void WriteAtOffset(double value, int offset)
```

Writes a value to the specified offset without advancing the pointer. Overloads exist for various data types including `sbyte`, `byte`, `short`, `ushort`, `int`, `uint`, `long`, `ulong`, `float`, `double`.

### Seek

```csharp
public void Seek(int offset)
```

Advances the stream by a specified number of bytes. `offset` is the number of bytes to advance by.

## About the Offset Methods

The `LittleEndianWriter` struct provides several methods that operate on a specific offset without advancing the pointer.

While these methods do not significantly reduce the instruction count, they offer some minor performance advantages,
you can read more about this in [About Section](./About.md#about-the-offset-methods)

## Usage

Example usage of the `LittleEndianWriter` struct:

```csharp
var writer = new LittleEndianWriter(ptr);

writer.Write((int)12345);  // Write an int
writer.Write((byte)65);    // Write a byte
writer.WriteAtOffset((long)9876543210, 5);  // Write a long at offset 5 from current pointer
```