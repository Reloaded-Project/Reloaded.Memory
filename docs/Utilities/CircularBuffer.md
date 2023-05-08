# CircularBuffer Class

!!! info "The `CircularBuffer` is a writable buffer useful for temporary storage of data. It's a buffer whereby once you reach the end of the buffer, it loops back over to the beginning of the buffer, overwriting old elements."

## Properties

- `Start`: The address of the `CircularBuffer`.
- `End`: The address of the `CircularBuffer`.
- `Current`: Address of the current item in the buffer.
- `Remaining`: Remaining space in the buffer.
- `Size`: The overall size of the buffer.

## Constructors

- `CircularBuffer(nuint start, int size)`: Creates a `CircularBuffer` within the target memory source.

## Methods

### Add

```csharp
public nuint Add(byte* data, uint length)
public nuint Add<TSource>(TSource source, byte* data, uint length)
public nuint Add<TSource, T>(TSource source, T value)
public nuint Add<T>(T value)
```

Adds a new item onto the circular buffer. Returns a pointer to the recently added item to the buffer, or zero if the item cannot fit.

### CanItemFit

```csharp
public ItemFit CanItemFit(uint itemSize)
public ItemFit CanItemFit<T>()
```

Returns an enumerable describing if an item can fit into the buffer.

## ItemFit Enum

!!! info "Describes whether an item can fit into a given buffer."

- `Yes`: The item can fit into the buffer.
- `StartOfBuffer`: The item can fit into the buffer, but not in the remaining space (will be placed at start of buffer).
- `No`: The item is too large to fit into the buffer.

## Examples

### Adding an Integer to The Buffer

```csharp
var circularBuffer = new CircularBuffer((nuint)bufferStartPtr, bufferSize);
nuint pointerToValue = circularBuffer.Add(42);

if (pointerToValue != UIntPtr.Zero)
    Console.WriteLine("Value added successfully!");
else
    Console.WriteLine("Failed to add value to the buffer.");
```

### Adding a Custom Struct to The Buffer

```csharp
var circularBuffer = new CircularBuffer((nuint)bufferStart, bufferSize);
var valueToAdd = new Vector2 { X = 1, Y = 2 };
nuint pointerToValue = circularBuffer.Add(valueToAdd);

if (pointerToValue != UIntPtr.Zero)
    Console.WriteLine("Value added successfully!");
else
    Console.WriteLine("Failed to add value to the buffer.");
```

### Checking if an item can fit in the buffer.

```csharp
var circularBuffer = new CircularBuffer((nuint)bufferStart, bufferSize);
var result = circularBuffer.CanItemFit<double>();

switch (result)
{
    case CircularBuffer.ItemFit.Yes:
        // "A double can fit in the buffer."
        break;
    case CircularBuffer.ItemFit.StartOfBuffer:
        // "A double can fit in the buffer, but it will be placed at the start of the buffer."
        break;
    case CircularBuffer.ItemFit.No:
        // "A double cannot fit in the buffer."
        break;
}
```