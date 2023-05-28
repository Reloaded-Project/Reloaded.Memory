# ArrayRental

!!! info "`ArrayRental` is a struct that represents an instance of a rented array. It should be disposed of after use with the `using` statement."

!!! tip "If you need a generic version, use `ArrayRental<T>`."
  
The underlying array is managed by the shared `ArrayPool`.  

## Properties

- `Array`: The underlying array for this rental.  
- `Span`: Span for the underlying array. May be larger than requested length.

## Constructor

```csharp
public ArrayRental(int numBytes)
```

Rents a requested minimum number of bytes. Amount of data rented might be larger.

## Slices

`ArrayRentalSlice` represents a slice of an `ArrayRental`. This API is meant to be used as a return value from methods, 
and transfers ownership of the rental from the internal `ArrayRental`.

```csharp
public ArrayRentalSlice(ArrayRental rental, int length)
```

Represents a slice of the array rental.

## Usage

### Rent an Array and Create a Slice

!!! tip "Make sure to dispose with `using` statement or explicit dispose."

```csharp
// Will create a rental of at least 4096 bytes.
using var rental = new ArrayRental(4096);
```

### Rent an Array and Create a Slice

!!! warning "When you create an `ArrayRentalSlice`, the responsibility of disposing the rental is transferred to the slice. Make sure to not double dispose."

```csharp
// Some Method
ArrayRentalSlice CompressData(byte* data, int length) 
{
    var rental = new ArrayRental(numBytes);
    // Compress into rental....
    // And return a slice with just the info needed.
    return new ArrayRentalSlice(rental, sliceLength);
}

// Method consumer
using var compressed = CompressData(data, length);
```