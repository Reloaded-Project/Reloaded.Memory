# ObjectMarshal

!!! info "Forked from [Community Toolkit](https://github.com/CommunityToolkit/dotnet)."

!!! info "Utility class providing methods for working with `object` instances."

ObjectMarshal is a utility class that provides methods for calculating byte offsets to specific fields within objects, retrieving references to data within objects at specific offsets, and unboxing values from objects.

## Methods

### DangerousGetObjectDataByteOffset

```csharp
public static IntPtr DangerousGetObjectDataByteOffset<T>(object obj, ref T data)
```

Calculates the byte offset to a specific field within a given `object`. The input parameters are not validated, and it's 
the responsibility of the caller to ensure that the `data` reference is actually pointing to a memory location within `obj`.

### DangerousGetObjectDataReferenceAt

```csharp
public static ref T DangerousGetObjectDataReferenceAt<T>(object obj, IntPtr offset)
```

Gets a `T` reference to data within a given object at a specified offset. None of the input arguments is validated, and 
it is the responsibility of the caller to ensure they are valid.

### TryUnbox

```csharp
public static bool TryUnbox<T>(this object obj, out T value) where T : struct
```

Tries to get a boxed `T` value from an input `object` instance. Returns true if a `T` value was retrieved correctly, 
`false` otherwise.

### DangerousUnbox

```csharp
public static ref T DangerousUnbox<T>(object obj) where T : struct
```

Unboxes a `T` value from an input object instance. Throws an `InvalidCastException` when obj is not of type `T`.

## Usage

### Calculate Byte Offset

```csharp
MyStruct myStruct = new MyStruct();
ref int fieldRef = ref myStruct.SomeIntField;
IntPtr offset = ObjectMarshal.DangerousGetObjectDataByteOffset(myStruct, ref fieldRef);
```

### Get Reference At Specific Offset

```csharp
MyStruct myStruct = new MyStruct();
IntPtr fieldOffset = /* calculated offset */;
ref int fieldRef = ref ObjectMarshal.DangerousGetObjectDataReferenceAt<MyStruct>(myStruct, fieldOffset);
```

### Try to Unbox a value

```csharp
object boxedInt = 42;
int unboxedInt;

if (boxedInt.TryUnbox(out unboxedInt))
    Console.WriteLine($"Unboxed value: {unboxedInt}");
```

### Unbox a value

```csharp
object boxedInt = 42;
ref int unboxedInt = ref ObjectMarshal.DangerousUnbox<int>(boxedInt);
```