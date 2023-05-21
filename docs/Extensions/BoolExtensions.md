# BoolExtensions

!!! info "Forked from [Community Toolkit](https://github.com/CommunityToolkit/dotnet)."

!!! info "Utility class providing high performance extension methods for working with the `bool` type."

BoolExtensions is a utility class that provides extension methods for the `bool` type, including converting `bool` values 
to `byte`, `int`, and `long` masks.

## Methods

### ToByte

```csharp
public static unsafe byte ToByte(this bool flag)
```

Converts the given `bool` value into a `byte`. Returns 1 if `flag` is `true`, 0 otherwise.
This method does not contain branching instructions.

### ToBitwiseMask32

```csharp
public static unsafe int ToBitwiseMask32(this bool flag)
```

Converts the given `bool` value to an `int` mask with all bits representing the value of the input flag
(either 0xFFFFFFFF or 0x00000000). This method does not contain branching instructions.

### ToBitwiseMask64

```csharp
public static unsafe long ToBitwiseMask64(this bool flag)
```

Converts the given `bool` value to a `long` mask with all bits representing the value of the input flag
(either all 1s or 0s). This method does not contain branching instructions.

## Usage

### Convert Bool to Byte

```csharp
bool flag = true;
byte result = flag.ToByte();
```

### Convert Bool to Int Mask

```csharp
bool flag = true;
int mask = flag.ToBitwiseMask32();
```

### Convert Bool to Long Mask

```csharp
bool flag = true;
long mask = flag.ToBitwiseMask64();
```