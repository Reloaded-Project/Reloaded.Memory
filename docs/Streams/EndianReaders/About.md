# About

!!! info "Common Info About Endian Readers and Writers"

## Interfaces

!!! info "Structs can implement `ICanBeReadByAnEndianReader` and `ICanWriteToAnEndianWriter` to allow for easy reading and writing."

Example:



## About the Offset Methods

!!! info "The structs provide methods named `WriteAtOffset` / `ReadAtOffset` to operate on an offset of the current pointer without advancing the pointer itself."

These methods offer some minor performance advantages.

### Improved Pipelining

By reducing the dependency of future instructions on earlier instructions, these offset methods allow for better pipelining.
For example, a future read operation does not need to wait for the `Ptr` value to be updated from a previous operation.

### JIT Optimization

The Just-In-Time (JIT) compiler can recognize when the `offset` parameters are specified as constants and can optimize
the instructions accordingly. This can lead to more efficient code execution.

```csharp
writer.WriteAtOffset(Hash, 0);
writer.WriteAtOffset((int)DecompressedSize, 8);
writer.WriteAtOffset(new OffsetPathIndexTuple(DecompressedBlockOffset, FilePathIndex, FirstBlockIndex).Data, 12);
writer.Seek(NativeFileEntryV0.SizeBytes);
```

Because write on `line 1`, does not depend on modified pointer after `line 0`, execution is faster, as the CPU can
better pipeline the instructions as there is no dependency on the ptr result of the previous method call.
