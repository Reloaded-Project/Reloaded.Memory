using System;
using BenchmarkDotNet.Running;

namespace Reloaded.Memory.Benchmark
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Trimmer Test: dotnet publish -c Release -f net6.0-windows -r win-x64 -p:PublishTrimmed=true -p:TrimmerDefaultAction=linkC:\Users\sewer\Desktop\Projects\Reloaded.Memory\Source\Reloaded.Memory.Benchmark\Reloaded.Memory.Benchmark.csproj

            //BenchmarkRunner.Run<Memory.Streams.SmallStruct.FileStream>();
            //BenchmarkRunner.Run<Memory.Streams.SmallStruct.MemoryStream>();

            //BenchmarkRunner.Run<Memory.Streams.Integers.FileStream>();
            //BenchmarkRunner.Run<Memory.Streams.Integers.MemoryStream>();
            //BenchmarkRunner.Run<Memory.Streams.Integers.UnrealisticMarshallingOverhead>();

            //BenchmarkRunner.Run<Memory.Streams.MediumStruct.FileStream>();
            //BenchmarkRunner.Run<Memory.Streams.MediumStruct.MemoryStream>();
            //BenchmarkRunner.Run<Memory.Streams.MediumStruct.UnrealisticMarshallingOverhead>();

            BenchmarkRunner.Run<Memory.Endian>();
            //BenchmarkRunner.Run<Memory.StructGetBytes>();
        }
    }
}
