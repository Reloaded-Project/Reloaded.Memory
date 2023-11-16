# StringExtensions

!!! info "Partially forked from [Community Toolkit](https://github.com/CommunityToolkit/dotnet)."

!!! info "Utility class providing high performance extension methods for working with the `string` type."

StringExtensions is a utility class that provides extension methods for the `string` type, including getting references to string elements, and counting the number of occurrences of a character in a string.

## Methods

### DangerousGetReference

```csharp
public static ref char DangerousGetReference(this string text)
```

Returns a reference to the first element within a given `string`, with no bounds checks. It is the caller's responsibility to perform bounds checks when dereferencing the returned value.

### DangerousGetReferenceAt

```csharp
public static ref char DangerousGetReferenceAt(this string text, int i)
```

Returns a reference to an element at a specified index within a given `string`, with no bounds checks. It is the caller's responsibility to ensure the `i` parameter is valid.

### Count

```csharp
public static int Count(this string text, char c)
```

Counts the number of occurrences of a given character in a target `string` instance.

### GetHashCodeFast

!!! warning "SIMD method currently restricted to .NET 7+. PRs for backports are welcome."

!!! warning "Will produce different hashes depending on runtime or CPU."

!!! info "Optimised for File Paths specifically"

```csharp
public static nuint GetHashCodeFast(string text)
public static unsafe nuint GetHashCodeFast(this ReadOnlySpan<char> text)
```

Faster hashcode for strings; but does not randomize between application runs.

Use this method if and only if 'Denial of Service' attacks are not a concern
(i.e. never used for free-form user input), or are otherwise mitigated.

This method does not provide guarantees about producing the same hash across different machines or library versions,
or runtime; only for the current process. Instead, it prioritises speed over all.

### GetHashCodeLowerFast

!!! warning "SIMD method currently restricted to .NET 7+. PRs for backports are welcome."

!!! warning "Will produce different hashes depending on runtime or CPU."

!!! info "Optimised for File Paths specifically"

```csharp
public static nuint GetHashCodeLowerFast(this string text)
public static unsafe nuint GetHashCodeLowerFast(this ReadOnlySpan<char> text)
```

Faster hashcode for strings, hashed in lower (invariant) case; does not randomize between application runs.

Use this method if and only if 'Denial of Service' attacks are not a concern
(i.e. never used for free-form user input), or are otherwise mitigated.

This method does not provide guarantees about producing the same hash across different machines or library versions,
or runtime; only for the current process. Instead, it prioritises speed over all.

### ToLowerInvariantFast

```csharp
public static string ToLowerInvariantFast(this string text)
public static unsafe void ToLowerInvariantFast(this ReadOnlySpan<char> text, Span<char> target)
```

Converts the given string to lower case (invariant casing) using the fastest possible implementation.
This method is optimized for performance but currently has limitations for short non-ASCII inputs.

### ToUpperInvariantFast

```csharp
public static string ToUpperInvariantFast(this string text)
public static unsafe void ToUpperInvariantFast(this ReadOnlySpan<char> text, Span<char> target)
```

Converts the given string to upper case (invariant casing) using the fastest possible implementation.
This method is optimized for performance but currently has limitations for short non-ASCII inputs.

## Usage

### Get Reference to First Element in String

```csharp
string text = "Hello, world!";
ref char firstCharRef = ref text.DangerousGetReference();
```

### Get Reference to Element at Index in String

```csharp
string text = "Hello, world!";
int index = 4;
ref char charAtIndexRef = ref text.DangerousGetReferenceAt(index);
```

### Count Character Occurrences in String

```csharp
string text = "Hello, world!";
char targetChar = 'l';
int count = text.Count(targetChar);
```

### Get Fast Hash Code

```csharp
string text = "Hello, world!";
nuint fastHashCode = text.GetHashCodeFast();
```

### Get Lower Case Hash Code

```csharp
string text = "Hello, World!";
nuint lowerCaseHashCode = text.GetHashCodeLowerFast();
```

## Convert String to Lower Case Invariant Fast

```csharp
string text = "Hello, WORLD!";
string lowerInvariant = text.ToLowerInvariantFast(); // hello, world!
```

### Convert String to Upper Case Invariant Fast

```csharp
string text = "hello, world!";
string upperInvariant = text.ToUpperInvariantFast(); // HELLO, WORLD!
```

### Convert ReadOnlySpan to Lower Case Invariant Fast

```csharp
string text = "Hello, WORLD!";
Span<char> target = stackalloc char[textSpan.Length]; // Careful with string length!
text.AsSpan().ToLowerInvariantFast(target); // hello, world! (on stack)
```

### Convert ReadOnlySpan to Upper Case Invariant Fast

```csharp
string text = "hello, world!";
Span<char> target = stackalloc char[textSpan.Length]; // Careful with string length!
text.AsSpan().ToLowerInvariantFast(target); // HELLO, WORLD! (on stack)
```