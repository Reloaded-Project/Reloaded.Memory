# Endian Class

!!! info "The `Endian` class provides various utilities for converting primitives and structures between endians."

## Methods

### Reverse

```csharp
public static byte Reverse(byte value)
public static sbyte Reverse(sbyte value)
public static short Reverse(short value)
public static ushort Reverse(ushort value)
public static int Reverse(int value)
public static uint Reverse(uint value)
public static long Reverse(long value)
public static ulong Reverse(ulong value)
public static float Reverse(float value)
public static double Reverse(double value)
```

Reverses the byte order of the specified value.

### Reverse (Generic)

!!! info

    Utility method for structs with single values.

!!! warning

    This method is provided for convenience.  
    If the item size is not 1/2/4/8 bytes, this method will throw.  

```csharp
public static T Reverse<T>(T value) where T : unmanaged
```

Reverses the endian of a primitive value such as int, short, float, double etc.

## Examples

### Reversing an Integer's Endian

```csharp
int value = 42;
int reversedValue = Endian.Reverse(value);

Console.WriteLine($"Original Value: {value}, Reversed Value: {reversedValue}");
```

This will print something like:

```
Original Value: 42, Reversed Value: 704643072
```

### Reversing a Custom Struct's Endian

!!! warning

    This will only work if the struct is a blittable type.

```csharp
struct NamedOffset
{
    public int Offset;
}

MyStruct value = new NamedOffset { Offset = 1 };
MyStruct reversedValue = Endian.Reverse(value);

Console.WriteLine($"Original Value: {value.X}, Reversed Value: {reversedValue.X}");
```

This will print something like:

```
Original Value: 1 Reversed Value: 16777216
```