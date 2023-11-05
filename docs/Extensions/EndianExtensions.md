# EndianExtensions

!!! info "Provides extension methods for converting between endianness."

`EndianExtensions` is a static class that offers methods to convert primitive data types and any structure implementing the `ICanReverseEndian` interface to big or little endian format.

The conversions check the system's endianness and only perform byte-swapping if necessary, making the operation efficient by avoiding redundant processing on systems with matching endianness.

!!! note "The JIT will eliminate no-operations here, so e.g. calling `AsLittleEndian` on a Little Endian machine has 0 overhead."

## Methods

### AsLittleEndian (Overloads for various types)

```csharp
public static byte AsLittleEndian(this byte value)
public static sbyte AsLittleEndian(this sbyte value)
public static short AsLittleEndian(this short value)
public static ushort AsLittleEndian(this ushort value)
public static int AsLittleEndian(this int value)
public static uint AsLittleEndian(this uint value)
public static long AsLittleEndian(this long value)
public static ulong AsLittleEndian(this ulong value)
public static float AsLittleEndian(this float value)
public static double AsLittleEndian(this double value)
public static T AsLittleEndian<T>(this T value) where T : struct, ICanReverseEndian
```

Converts the given value to little endian format. If the system is already little endian, no conversion is performed.

#### Parameters

- `value`: The value to convert to little endian.

#### Returns

The value in little endian format.

---

### AsBigEndian (Overloads for various types)

```csharp
public static byte AsBigEndian(this byte value)
public static sbyte AsBigEndian(this sbyte value)
public static short AsBigEndian(this short value)
public static ushort AsBigEndian(this ushort value)
public static int AsBigEndian(this int value)
public static uint AsBigEndian(this uint value)
public static long AsBigEndian(this long value)
public static ulong AsBigEndian(this ulong value)
public static float AsBigEndian(this float value)
public static double AsBigEndian(this double value)
public static T AsBigEndian<T>(this T value) where T : struct, ICanReverseEndian
```

Converts the given value to big endian format. If the system is already big endian, no conversion is performed.

#### Parameters

- `value`: The value to convert to big endian.

#### Returns

The value in big endian format.

## Usage

### Convert an Integer to Little Endian Format

```csharp
int myValue = 12345678;
int littleEndianValue = myValue.AsLittleEndian();
```

### Convert a Double to Big Endian Format

```csharp
double myValue = 123.456;
double bigEndianValue = myValue.AsBigEndian();
```

### Convert a Custom Structure to Big Endian Format

!!! info "For structs which implement `ICanReverseEndian`"

```csharp
var myStruct = new MyStruct { /* ... */ };
var asBig = myStruct.AsBigEndian();
```