# EnumExtensions

!!! info "Provides high-performance extension methods for working with enums."

EnumExtensions is a utility class that , including checking for the presence of a specified flag in an enum value using unsafe code for faster execution.

## Methods

### HasFlagFast

```csharp
public static bool HasFlagFast<T>(this T value, T flag) where T : unmanaged, Enum
```

Determines if the given enum has a specified flag. This method uses unsafe code for faster execution and skips type check.

#### Parameters

- `value`: The value to check.
- `flag`: The flag to check.

#### Returns

`true` if the enum is contained in the value, `false` otherwise.

#### Type Parameters

- `T`: The type to check the flag of.

#### Exceptions

- `NotSupportedException`: This type of enum is not supported.

## Usage

### Check if an Enum Value Contains a Specific Flag

```csharp
[Flags]
enum MyEnum : int
{
    None = 0,
    Flag1 = 1,
    Flag2 = 2,
    Flag3 = 4,
}

MyEnum value = MyEnum.Flag1 | MyEnum.Flag3;
bool hasFlag1 = value.HasFlagFast(MyEnum.Flag1); // True
bool hasFlag2 = value.HasFlagFast(MyEnum.Flag2); // False
bool hasFlag3 = value.HasFlagFast(MyEnum.Flag3); // True
```