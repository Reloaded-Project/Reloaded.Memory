# SpanExtensions

!!! info "Partially forked from [Community Toolkit](https://github.com/CommunityToolkit/dotnet)."

!!! info "Utility class providing high performance extension methods for working with spans."

SpanExtensions is a utility class that provides extension methods for spans, including methods for casting, slicing, 
replacing elements, and finding offsets.

## Methods

### CastFast

!!! warning "Not accelerated on .NET Framework and Standard 2.0"

```csharp
public static Span<TTo> CastFast<TFrom, TTo>(this Span<TFrom> data) where TFrom : struct where TTo : struct
```
Casts a `Span<TFrom>` to a `Span<TTo>` without copying the underlying data.

### DangerousGetReference
```csharp
public static ref T DangerousGetReference<T>(this Span<T> span)
```
Returns a reference to the first element within a given `Span<T>`, with no bounds checks. 

### DangerousGetReferenceAt
```csharp
public static ref T DangerousGetReferenceAt<T>(this Span<T> span, int i)
```
Returns a reference to an element at a specified index within a given `Span<T>`, with no bounds checks.  
The caller is responsible for ensuring the `i` parameter is valid.  

### AsBytes
```csharp
public static Span<byte> AsBytes<T>(this Span<T> span) where T : unmanaged
```
Converts a `Span<T>` to a `Span<byte>` without copying the underlying data.

### Cast
```csharp
public static Span<TTo> Cast<TFrom, TTo>(this Span<TFrom> span) where TFrom : unmanaged where TTo : unmanaged
```
Casts a `Span<TFrom>` to a `Span<TTo>` without copying the underlying data, when both types are unmanaged.

### IndexOf
```csharp
public static unsafe int IndexOf<T>(this Span<T> span, ref T value)
```

Gets the index of an element within given `Span<T>` based on a reference to an element inside the `Span<T>`.

### Count
```csharp
public static int Count<T>(this Span<T> span, T value) where T : IEquatable<T>
```
Counts the number of occurrences of a given value in a target `Span<T>` instance.

### SliceFast

!!! warning "Not accelerated on .NET Framework and Standard 2.0"

```csharp
public static Span<T> SliceFast<T>(this Span<T> data, int start, int length)
public static ReadOnlySpan<T> SliceFast<T>(this ReadOnlySpan<T> data, int start, int length)
public static ReadOnlySpan<T> SliceFast<T>(this ReadOnlySpan<T> data, Range range)
public static Span<T> SliceFast<T>(this Span<T> data, Range range)
public static Span<T> SliceFast<T>(this Span<T> data, int start)
public static ReadOnlySpan<T> SliceFast<T>(this ReadOnlySpan<T> data, int start)
```
Performs a slice operation on a `Span<T>` or `ReadOnlySpan<T>` without performing bounds checks.
This supports ranges, so `span.SliceFast(1..3)` is valid.  

### Replace
```csharp
public static Span<char> Replace(this Span<char> data, char oldValue, char newValue, Span<char> buffer)
public static unsafe Span<T> Replace<T>(this Span<T> data, T oldValue, T newValue, Span<T> buffer) where T : unmanaged, IEquatable<T>
```
Replaces all occurrences of a specified value with another value in a given `Span<T>`.

### FindAllOffsetsOfByte

!!! warning "Missing SIMD path for non-x86/x64 platforms. PRs are welcome."

```csharp
public static List<int> FindAllOffsetsOfByte(this ReadOnlySpan<byte> data, byte value)
public static List<int> FindAllOffsetsOfByte(this ReadOnlySpan<byte> data, byte value, int offsetCountHint)
```
Finds all the offsets of a given byte value in a target `ReadOnlySpan<byte>` instance.   
The second overload allows specifying an optional `offsetCountHint` parameter to preallocate the list capacity.  

## Usage

### Get Reference without Bounds Checks
```csharp
Span<int> span = new int[] { 1, 2, 3 };
ref int firstElement = ref span.DangerousGetReference();
```

### Get Reference at Specific Index without Bounds Checks
```csharp
Span<int> span = new int[] { 1, 2, 3 };
ref int elementAtTwo = ref span.DangerousGetReferenceAt(2);
```

### Convert Span to Byte Span
```csharp
Span<int> intSpan = new int[] { 1, 2, 3 };
Span<byte> byteSpan = intSpan.AsBytes();
```

### Cast Span
```csharp
Span<byte> byteSpan = new byte[] { 1, 2, 3, 4 };
Span<int> intSpan = byteSpan.Cast<byte, int>();
```

### Find Index of an Element
```csharp
Span<int> span = new int[] { 1, 2, 3, 4, 5 };
ref int value = ref span[3];
int index = span.IndexOf(ref value); // index = 3
```

### Count Occurrences of a Value
```csharp
Span<int> span = new int[] { 1, 2, 2, 3, 2 };
int count = span.Count(2); // 3
```

### Slice Span Without Bounds Checks
```csharp
Span<int> span = new int[] { 1, 2, 3, 4, 5 };
Span<int> slicedSpan = span.SliceFast(1, 3);
```

### Replace Elements in a Span
```csharp
Span<char> span = "hello world".ToCharArray();
Span<char> buffer = new char[span.Length];
Span<char> replacedSpan = span.Replace('l', 'x', buffer);
```

### Find All Offsets of a Byte
```csharp
byte[] data = new byte[] { 1, 2, 3, 2, 4, 2 };
ReadOnlySpan<byte> span = data;
List<int> offsets = span.FindAllOffsetsOfByte(2);
```

### Find All Offsets of a Byte with Offset Count Hint
```csharp
byte[] data = new byte[] { 1, 2, 3, 2, 4, 2 };
ReadOnlySpan<byte> span = data;
List<int> offsets = span.FindAllOffsetsOfByte(2, 3);
```
