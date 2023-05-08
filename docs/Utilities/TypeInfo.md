# TypeInfo

!!! info "Utility class providing methods for obtaining information about different types."

## Methods
### ApproximateIsBlittable
```csharp
public static bool ApproximateIsBlittable<T>()
```
Returns `true` if a type is blittable, else `false`. This method uses an approximation, and may not work with generic types with blittable (unmanaged) constraints.

### ApproximateIsBlittable
```csharp
public static bool ApproximateIsBlittable(Type type)
```
Returns `true` if a type is blittable, else `false`. This method uses an approximation, and may not work with generic types with blittable (unmanaged) constraints.

## Usage
### Check if a Type is Blittable (Generic)
```csharp
bool isBlittable = TypeInfo.ApproximateIsBlittable<int>();
```

### Check if a Type is Blittable (Non-Generic)
```csharp
bool isBlittable = TypeInfo.ApproximateIsBlittable(typeof(int));
```