# LittleEndianReader

!!! info "A struct for reading from a pointer in Big Endian."

!!! tip "LittleEndianReader is preferred over [BufferedStreamReader](../BufferedStreamReader.md) when all data is already in memory."

The `LittleEndianReader` struct provides utility methods for reading various data types from a pointer in Big Endian format.

## Methods

### Read Methods

These methods read the specified data type from the current pointer in Big Endian format and advance the pointer.

```csharp
public byte ReadByte()
public sbyte ReadSByte()
public short ReadShort()
public ushort ReadUShort()
public uint ReadUInt()
public int ReadInt()
public ulong ReadULong()
public long ReadLong()
public float ReadFloat()
public double ReadDouble()
```

### ReadAtOffset Methods

These methods read the specified data type from the specified offset in Big Endian format without advancing the pointer.

```csharp
public byte ReadByteAtOffset(int offset)
public sbyte ReadSByteAtOffset(int offset)
public short ReadShortAtOffset(int offset)
public ushort ReadUShortAtOffset(int offset)
public uint ReadUIntAtOffset(int offset)
public int ReadIntAtOffset(int offset)
public ulong ReadULongAtOffset(int offset)
public long ReadLongAtOffset(int offset)
public float ReadFloatAtOffset(int offset)
public double ReadDoubleAtOffset(int offset)
```

### Seek Method

This method advances the pointer by a specified number of bytes.

```csharp
public void Seek(int offset)
```

## About the Offset Methods

The `LittleEndianReader` struct provides several methods that read from a specific offset without advancing the pointer.
These methods include `ReadShortAtOffset`, `ReadIntAtOffset`, `ReadLongAtOffset`, and `ReadUlongAtOffset`.

While these methods do not significantly reduce the instruction count, they offer some minor performance advantages,
you can read more about this in [About Section](./LittleEndianWriter.md#about-the-offset-methods)

## Usage

### Reading from Pointer

```csharp
LittleEndianReader reader = new LittleEndianReader(GetPointer());

byte byteValue = reader.ReadByte();
uint uintValue = reader.ReadUInt();
double doubleValue = reader.ReadDouble();
```

### Reading from Offset

```csharp
LittleEndianReader reader = new LittleEndianReader(GetPointer());

byte byteValue = reader.ReadByteAtOffset(5);
uint uintValue = reader.ReadUIntAtOffset(10);
double doubleValue = reader.ReadDoubleAtOffset(15);
```

### Advancing the Pointer

```csharp
LittleEndianReader reader = new LittleEndianReader(GetPointer(););

reader.Seek(10); // Advances the pointer by 10 bytes.
```