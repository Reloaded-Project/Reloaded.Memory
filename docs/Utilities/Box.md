# Box<T>

!!! info "Forked from [Community Toolkit](https://github.com/CommunityToolkit/dotnet)."

!!! info "A class representing a boxed value type, providing build-time validation and automatic unboxing."

Box<T> is a utility class that represents a boxed value type on the managed heap. It can be used in place of a non-generic object reference to a boxed value type, making the code more expressive and reducing the chances of errors.

## Methods

### GetFrom

```csharp
public static Box<T> GetFrom(object obj)
```

Returns a `Box<T>` reference from the input `object` instance, representing a boxed `T` value.

### DangerousGetFrom

```csharp
public static Box<T> DangerousGetFrom(object obj)
```

Returns a `Box<T>` reference from the input `object` instance, representing a boxed `T` value. This method doesn't check the actual type of `obj`, so it is the responsibility of the caller to ensure it actually represents a boxed `T` value and not some other instance.

### TryGetFrom

```csharp
public static bool TryGetFrom(object obj, out Box<T>? box)
```

Tries to get a `Box<T>` reference from an input `object` representing a boxed `T` value. Returns `true` if a `Box<T>` instance was retrieved correctly, `false` otherwise.

## Operators

### implicit operator T

```csharp
public static implicit operator T(Box<T> box)
```

Implicitly gets the `T` value from a given `Box<T>` instance.

### implicit operator Box<T>

```csharp
public static implicit operator Box<T>(T value)
```

Implicitly creates a new `Box<T>` instance from a given `T` value.

## Usage

### Box and Unbox Value Type with Build-time Validation

```csharp
Box<int> box = 42;
int sum = box.Value + 1;
```

### Retrieve a Mutable Reference to a Boxed Value

```csharp
Box<MyStruct> box = new MyStruct { Field1 = 1, Field2 = 2 };
ref MyStruct myStructRef = ref box.GetReference();
myStructRef.Field1 = 3;
```

## Extension Methods

### GetReference

```csharp
public static ref T GetReference<T>(this Box<T> box) where T : struct
```

Gets a `T` reference from a `Box<T>` instance.

### Usage

```csharp
Box<MyStruct> box = new MyStruct { Field1 = 1, Field2 = 2 };
ref MyStruct myStructRef = ref box.GetReference();
```