# ArrayExtensions

!!! info "Partially forked from [Community Toolkit](https://github.com/CommunityToolkit/dotnet)."

!!! info "Utility class providing high performance extension methods for working with arrays."

ArrayExtensions is a utility class that provides extension methods for arrays, including methods for getting references 
without bounds checks, counting occurrences of a value, checking covariance, and converting arrays to spans.

## Methods

### DangerousGetReference

```csharp
public static ref T DangerousGetReference<T>(this T[] array)
```

Returns a reference to the first element within a given `T[]` array, with no bounds checks. The caller is responsible 
for performing checks in case the returned value is dereferenced.

### DangerousGetReferenceAt

```csharp
public static ref T DangerousGetReferenceAt<T>(this T[] array, int i)
```

Returns a reference to an element at a specified index within a given `T[]` array, with no bounds checks. 
The caller is responsible for ensuring the `i` parameter is valid.

### Count

```csharp
public static int Count<T>(this T[] array, T value) where T : IEquatable<T>
```

Counts the number of occurrences of a given value in a target `T[]` array instance.

### IsCovariant

```csharp
public static bool IsCovariant<T>(this T[] array)
```

Checks whether or not a given `T[]` array is covariant.

### AsSpanFast

!!! warning "Only supported on .NET 5 and above. Older runtimes will use default `AsSpan`."

```csharp
public static Span<T> AsSpanFast<T>(this T[] data)
```

Converts a byte array to a Span without doing a null check.

## Usage

### Get Reference without Bounds Checks

```csharp
int[] array = new int[] { 1, 2, 3 };
ref int firstElement = ref array.DangerousGetReference();
```

### Get Reference at Specific Index without Bounds Checks

```csharp
int[] array = new int[] { 1, 2, 3 };
ref int elementAtTwo = ref array.DangerousGetReferenceAt(2);
```

### Count Occurrences of a Value

```csharp
int[] array = new int[] { 1, 2, 2, 3, 2 };
int count = array.Count(2);
```

### Check Covariance

```csharp
object[] array = new object[] { "a", "b", "c" };
bool isCovariant = array.IsCovariant<string>();
```

### Convert Array to Span without Null Check

```csharp
int[] array = new int[] { 1, 2, 3 };
Span<int> span = array.AsSpanFast();
```