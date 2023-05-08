# VectorExtensions

!!! info "Utility class providing high performance extension methods for working with the `Vector` struct."

VectorExtensions is a utility class that provides extension methods for the `Vector` struct.

## Methods

### LoadUnsafe

```csharp
internal static Vector<T> LoadUnsafe<T>(ref T source, nuint elementOffset) where T : struct
```

Loads an element at a specified offset into a vector. Unsafe operations are used for performance reasons.

### StoreUnsafe

```csharp
internal static void StoreUnsafe<T>(this Vector<T> source, ref T destination, nuint elementOffset) where T : struct
```

Stores an element from a vector into the destination + offset. Unsafe operations are used for performance reasons.

## Usage

### Load Element into a Vector at an Offset

```csharp
int[] array = new int[] { 1, 2, 3, 4, 5 };
ref int source = ref array[0];
nuint elementOffset = 2;
Vector<int> vector = VectorExtensions.LoadUnsafe(ref source, elementOffset);
```

### Store Element from a Vector to Destination + Offset

```csharp
Vector<int> vector = new Vector<int>(new int[] { 1, 2, 3, 4 });
int[] destinationArray = new int[vector.Count];
ref int destination = ref destinationArray[0];
nuint elementOffset = 1;
vector.StoreUnsafe(ref destination, elementOffset);
```