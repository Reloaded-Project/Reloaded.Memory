

using System;
using System.IO;

namespace Reloaded.Memory.Shared.Generator
{
  
	public class RandomByteGenerator
    {
        /* Amount of randomized data in Megabytes. */
        public const string TestFileName = "Bytes.bin";
        private static Random _random = new Random();
        private static byte[] _randomBuffer = new byte[8];

        /* Target size of buffers for testing. */
        public Byte[] Structs { get; set; }
        public byte[] Bytes { get; set; }
        private bool _fileInDisk = false;

        /* Construction/Destruction */
        public RandomByteGenerator(int megabytes)
        {
            int totalBytes = Mathematics.MegaBytesToBytes(megabytes);
            int structs = Mathematics.BytesToStructCount<Byte>(totalBytes);
            Structs = new Byte[structs];

            for (int x = 0; x < structs; x++)
                Structs[x] = (Byte)NextRandom(Byte.MinValue, Byte.MaxValue);

            Bytes = StructArray.GetBytes(Structs);
        }

        public System.IO.FileStream GetFileStream()
        {
			WriteFileToDisk();
            return new System.IO.FileStream(TestFileName, FileMode.Open);
        }

        public System.IO.FileStream GetFileStreamWithBufferSize(int bufferSize)
        {
			WriteFileToDisk();
            return new System.IO.FileStream(TestFileName, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize);
        }

        public System.IO.MemoryStream GetMemoryStream()
        {
            return new System.IO.MemoryStream(Bytes);
        }

        private void WriteFileToDisk() 
        {
			if (! _fileInDisk) 
            {
				File.WriteAllBytes(TestFileName, Bytes);
				_fileInDisk = true;
            }
        }

		private int NextRandom(int minimum, int maximum)
        {
			return _random.Next(minimum, maximum);
        }

		private long NextRandom(long minimum, long maximum)
        {
			_random.NextBytes(_randomBuffer);
            Struct.FromArray(_randomBuffer, out long longRand, 0);
			return (Math.Abs(longRand % (maximum - minimum)) + minimum);
        }

		private ulong NextRandom(ulong minimum, ulong maximum)
        {
			_random.NextBytes(_randomBuffer);
            Struct.FromArray(_randomBuffer, out ulong longRand, 0);
			return (longRand % (maximum - minimum)) + minimum;
        }

        private double NextRandom(double minimum, double maximum)
        {
			_random.NextBytes(_randomBuffer);
            Struct.FromArray(_randomBuffer, out long longRand, 0);
			return (double)(Math.Abs(longRand % (maximum - minimum)) + minimum);
        }
    }

  
	public class RandomSByteGenerator
    {
        /* Amount of randomized data in Megabytes. */
        public const string TestFileName = "SBytes.bin";
        private static Random _random = new Random();
        private static byte[] _randomBuffer = new byte[8];

        /* Target size of buffers for testing. */
        public SByte[] Structs { get; set; }
        public byte[] Bytes { get; set; }
        private bool _fileInDisk = false;

        /* Construction/Destruction */
        public RandomSByteGenerator(int megabytes)
        {
            int totalBytes = Mathematics.MegaBytesToBytes(megabytes);
            int structs = Mathematics.BytesToStructCount<SByte>(totalBytes);
            Structs = new SByte[structs];

            for (int x = 0; x < structs; x++)
                Structs[x] = (SByte)NextRandom(SByte.MinValue, SByte.MaxValue);

            Bytes = StructArray.GetBytes(Structs);
        }

        public System.IO.FileStream GetFileStream()
        {
			WriteFileToDisk();
            return new System.IO.FileStream(TestFileName, FileMode.Open);
        }

        public System.IO.FileStream GetFileStreamWithBufferSize(int bufferSize)
        {
			WriteFileToDisk();
            return new System.IO.FileStream(TestFileName, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize);
        }

        public System.IO.MemoryStream GetMemoryStream()
        {
            return new System.IO.MemoryStream(Bytes);
        }

        private void WriteFileToDisk() 
        {
			if (! _fileInDisk) 
            {
				File.WriteAllBytes(TestFileName, Bytes);
				_fileInDisk = true;
            }
        }

		private int NextRandom(int minimum, int maximum)
        {
			return _random.Next(minimum, maximum);
        }

		private long NextRandom(long minimum, long maximum)
        {
			_random.NextBytes(_randomBuffer);
            Struct.FromArray(_randomBuffer, out long longRand, 0);
			return (Math.Abs(longRand % (maximum - minimum)) + minimum);
        }

		private ulong NextRandom(ulong minimum, ulong maximum)
        {
			_random.NextBytes(_randomBuffer);
            Struct.FromArray(_randomBuffer, out ulong longRand, 0);
			return (longRand % (maximum - minimum)) + minimum;
        }

        private double NextRandom(double minimum, double maximum)
        {
			_random.NextBytes(_randomBuffer);
            Struct.FromArray(_randomBuffer, out long longRand, 0);
			return (double)(Math.Abs(longRand % (maximum - minimum)) + minimum);
        }
    }

  
	public class RandomInt16Generator
    {
        /* Amount of randomized data in Megabytes. */
        public const string TestFileName = "Int16s.bin";
        private static Random _random = new Random();
        private static byte[] _randomBuffer = new byte[8];

        /* Target size of buffers for testing. */
        public Int16[] Structs { get; set; }
        public byte[] Bytes { get; set; }
        private bool _fileInDisk = false;

        /* Construction/Destruction */
        public RandomInt16Generator(int megabytes)
        {
            int totalBytes = Mathematics.MegaBytesToBytes(megabytes);
            int structs = Mathematics.BytesToStructCount<Int16>(totalBytes);
            Structs = new Int16[structs];

            for (int x = 0; x < structs; x++)
                Structs[x] = (Int16)NextRandom(Int16.MinValue, Int16.MaxValue);

            Bytes = StructArray.GetBytes(Structs);
        }

        public System.IO.FileStream GetFileStream()
        {
			WriteFileToDisk();
            return new System.IO.FileStream(TestFileName, FileMode.Open);
        }

        public System.IO.FileStream GetFileStreamWithBufferSize(int bufferSize)
        {
			WriteFileToDisk();
            return new System.IO.FileStream(TestFileName, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize);
        }

        public System.IO.MemoryStream GetMemoryStream()
        {
            return new System.IO.MemoryStream(Bytes);
        }

        private void WriteFileToDisk() 
        {
			if (! _fileInDisk) 
            {
				File.WriteAllBytes(TestFileName, Bytes);
				_fileInDisk = true;
            }
        }

		private int NextRandom(int minimum, int maximum)
        {
			return _random.Next(minimum, maximum);
        }

		private long NextRandom(long minimum, long maximum)
        {
			_random.NextBytes(_randomBuffer);
            Struct.FromArray(_randomBuffer, out long longRand, 0);
			return (Math.Abs(longRand % (maximum - minimum)) + minimum);
        }

		private ulong NextRandom(ulong minimum, ulong maximum)
        {
			_random.NextBytes(_randomBuffer);
            Struct.FromArray(_randomBuffer, out ulong longRand, 0);
			return (longRand % (maximum - minimum)) + minimum;
        }

        private double NextRandom(double minimum, double maximum)
        {
			_random.NextBytes(_randomBuffer);
            Struct.FromArray(_randomBuffer, out long longRand, 0);
			return (double)(Math.Abs(longRand % (maximum - minimum)) + minimum);
        }
    }

  
	public class RandomUInt16Generator
    {
        /* Amount of randomized data in Megabytes. */
        public const string TestFileName = "UInt16s.bin";
        private static Random _random = new Random();
        private static byte[] _randomBuffer = new byte[8];

        /* Target size of buffers for testing. */
        public UInt16[] Structs { get; set; }
        public byte[] Bytes { get; set; }
        private bool _fileInDisk = false;

        /* Construction/Destruction */
        public RandomUInt16Generator(int megabytes)
        {
            int totalBytes = Mathematics.MegaBytesToBytes(megabytes);
            int structs = Mathematics.BytesToStructCount<UInt16>(totalBytes);
            Structs = new UInt16[structs];

            for (int x = 0; x < structs; x++)
                Structs[x] = (UInt16)NextRandom(UInt16.MinValue, UInt16.MaxValue);

            Bytes = StructArray.GetBytes(Structs);
        }

        public System.IO.FileStream GetFileStream()
        {
			WriteFileToDisk();
            return new System.IO.FileStream(TestFileName, FileMode.Open);
        }

        public System.IO.FileStream GetFileStreamWithBufferSize(int bufferSize)
        {
			WriteFileToDisk();
            return new System.IO.FileStream(TestFileName, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize);
        }

        public System.IO.MemoryStream GetMemoryStream()
        {
            return new System.IO.MemoryStream(Bytes);
        }

        private void WriteFileToDisk() 
        {
			if (! _fileInDisk) 
            {
				File.WriteAllBytes(TestFileName, Bytes);
				_fileInDisk = true;
            }
        }

		private int NextRandom(int minimum, int maximum)
        {
			return _random.Next(minimum, maximum);
        }

		private long NextRandom(long minimum, long maximum)
        {
			_random.NextBytes(_randomBuffer);
            Struct.FromArray(_randomBuffer, out long longRand, 0);
			return (Math.Abs(longRand % (maximum - minimum)) + minimum);
        }

		private ulong NextRandom(ulong minimum, ulong maximum)
        {
			_random.NextBytes(_randomBuffer);
            Struct.FromArray(_randomBuffer, out ulong longRand, 0);
			return (longRand % (maximum - minimum)) + minimum;
        }

        private double NextRandom(double minimum, double maximum)
        {
			_random.NextBytes(_randomBuffer);
            Struct.FromArray(_randomBuffer, out long longRand, 0);
			return (double)(Math.Abs(longRand % (maximum - minimum)) + minimum);
        }
    }

  
	public class RandomInt32Generator
    {
        /* Amount of randomized data in Megabytes. */
        public const string TestFileName = "Int32s.bin";
        private static Random _random = new Random();
        private static byte[] _randomBuffer = new byte[8];

        /* Target size of buffers for testing. */
        public Int32[] Structs { get; set; }
        public byte[] Bytes { get; set; }
        private bool _fileInDisk = false;

        /* Construction/Destruction */
        public RandomInt32Generator(int megabytes)
        {
            int totalBytes = Mathematics.MegaBytesToBytes(megabytes);
            int structs = Mathematics.BytesToStructCount<Int32>(totalBytes);
            Structs = new Int32[structs];

            for (int x = 0; x < structs; x++)
                Structs[x] = (Int32)NextRandom(Int32.MinValue, Int32.MaxValue);

            Bytes = StructArray.GetBytes(Structs);
        }

        public System.IO.FileStream GetFileStream()
        {
			WriteFileToDisk();
            return new System.IO.FileStream(TestFileName, FileMode.Open);
        }

        public System.IO.FileStream GetFileStreamWithBufferSize(int bufferSize)
        {
			WriteFileToDisk();
            return new System.IO.FileStream(TestFileName, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize);
        }

        public System.IO.MemoryStream GetMemoryStream()
        {
            return new System.IO.MemoryStream(Bytes);
        }

        private void WriteFileToDisk() 
        {
			if (! _fileInDisk) 
            {
				File.WriteAllBytes(TestFileName, Bytes);
				_fileInDisk = true;
            }
        }

		private int NextRandom(int minimum, int maximum)
        {
			return _random.Next(minimum, maximum);
        }

		private long NextRandom(long minimum, long maximum)
        {
			_random.NextBytes(_randomBuffer);
            Struct.FromArray(_randomBuffer, out long longRand, 0);
			return (Math.Abs(longRand % (maximum - minimum)) + minimum);
        }

		private ulong NextRandom(ulong minimum, ulong maximum)
        {
			_random.NextBytes(_randomBuffer);
            Struct.FromArray(_randomBuffer, out ulong longRand, 0);
			return (longRand % (maximum - minimum)) + minimum;
        }

        private double NextRandom(double minimum, double maximum)
        {
			_random.NextBytes(_randomBuffer);
            Struct.FromArray(_randomBuffer, out long longRand, 0);
			return (double)(Math.Abs(longRand % (maximum - minimum)) + minimum);
        }
    }

  
	public class RandomUInt32Generator
    {
        /* Amount of randomized data in Megabytes. */
        public const string TestFileName = "UInt32s.bin";
        private static Random _random = new Random();
        private static byte[] _randomBuffer = new byte[8];

        /* Target size of buffers for testing. */
        public UInt32[] Structs { get; set; }
        public byte[] Bytes { get; set; }
        private bool _fileInDisk = false;

        /* Construction/Destruction */
        public RandomUInt32Generator(int megabytes)
        {
            int totalBytes = Mathematics.MegaBytesToBytes(megabytes);
            int structs = Mathematics.BytesToStructCount<UInt32>(totalBytes);
            Structs = new UInt32[structs];

            for (int x = 0; x < structs; x++)
                Structs[x] = (UInt32)NextRandom(UInt32.MinValue, UInt32.MaxValue);

            Bytes = StructArray.GetBytes(Structs);
        }

        public System.IO.FileStream GetFileStream()
        {
			WriteFileToDisk();
            return new System.IO.FileStream(TestFileName, FileMode.Open);
        }

        public System.IO.FileStream GetFileStreamWithBufferSize(int bufferSize)
        {
			WriteFileToDisk();
            return new System.IO.FileStream(TestFileName, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize);
        }

        public System.IO.MemoryStream GetMemoryStream()
        {
            return new System.IO.MemoryStream(Bytes);
        }

        private void WriteFileToDisk() 
        {
			if (! _fileInDisk) 
            {
				File.WriteAllBytes(TestFileName, Bytes);
				_fileInDisk = true;
            }
        }

		private int NextRandom(int minimum, int maximum)
        {
			return _random.Next(minimum, maximum);
        }

		private long NextRandom(long minimum, long maximum)
        {
			_random.NextBytes(_randomBuffer);
            Struct.FromArray(_randomBuffer, out long longRand, 0);
			return (Math.Abs(longRand % (maximum - minimum)) + minimum);
        }

		private ulong NextRandom(ulong minimum, ulong maximum)
        {
			_random.NextBytes(_randomBuffer);
            Struct.FromArray(_randomBuffer, out ulong longRand, 0);
			return (longRand % (maximum - minimum)) + minimum;
        }

        private double NextRandom(double minimum, double maximum)
        {
			_random.NextBytes(_randomBuffer);
            Struct.FromArray(_randomBuffer, out long longRand, 0);
			return (double)(Math.Abs(longRand % (maximum - minimum)) + minimum);
        }
    }

  
	public class RandomInt64Generator
    {
        /* Amount of randomized data in Megabytes. */
        public const string TestFileName = "Int64s.bin";
        private static Random _random = new Random();
        private static byte[] _randomBuffer = new byte[8];

        /* Target size of buffers for testing. */
        public Int64[] Structs { get; set; }
        public byte[] Bytes { get; set; }
        private bool _fileInDisk = false;

        /* Construction/Destruction */
        public RandomInt64Generator(int megabytes)
        {
            int totalBytes = Mathematics.MegaBytesToBytes(megabytes);
            int structs = Mathematics.BytesToStructCount<Int64>(totalBytes);
            Structs = new Int64[structs];

            for (int x = 0; x < structs; x++)
                Structs[x] = (Int64)NextRandom(Int64.MinValue, Int64.MaxValue);

            Bytes = StructArray.GetBytes(Structs);
        }

        public System.IO.FileStream GetFileStream()
        {
			WriteFileToDisk();
            return new System.IO.FileStream(TestFileName, FileMode.Open);
        }

        public System.IO.FileStream GetFileStreamWithBufferSize(int bufferSize)
        {
			WriteFileToDisk();
            return new System.IO.FileStream(TestFileName, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize);
        }

        public System.IO.MemoryStream GetMemoryStream()
        {
            return new System.IO.MemoryStream(Bytes);
        }

        private void WriteFileToDisk() 
        {
			if (! _fileInDisk) 
            {
				File.WriteAllBytes(TestFileName, Bytes);
				_fileInDisk = true;
            }
        }

		private int NextRandom(int minimum, int maximum)
        {
			return _random.Next(minimum, maximum);
        }

		private long NextRandom(long minimum, long maximum)
        {
			_random.NextBytes(_randomBuffer);
            Struct.FromArray(_randomBuffer, out long longRand, 0);
			return (Math.Abs(longRand % (maximum - minimum)) + minimum);
        }

		private ulong NextRandom(ulong minimum, ulong maximum)
        {
			_random.NextBytes(_randomBuffer);
            Struct.FromArray(_randomBuffer, out ulong longRand, 0);
			return (longRand % (maximum - minimum)) + minimum;
        }

        private double NextRandom(double minimum, double maximum)
        {
			_random.NextBytes(_randomBuffer);
            Struct.FromArray(_randomBuffer, out long longRand, 0);
			return (double)(Math.Abs(longRand % (maximum - minimum)) + minimum);
        }
    }

  
	public class RandomUInt64Generator
    {
        /* Amount of randomized data in Megabytes. */
        public const string TestFileName = "UInt64s.bin";
        private static Random _random = new Random();
        private static byte[] _randomBuffer = new byte[8];

        /* Target size of buffers for testing. */
        public UInt64[] Structs { get; set; }
        public byte[] Bytes { get; set; }
        private bool _fileInDisk = false;

        /* Construction/Destruction */
        public RandomUInt64Generator(int megabytes)
        {
            int totalBytes = Mathematics.MegaBytesToBytes(megabytes);
            int structs = Mathematics.BytesToStructCount<UInt64>(totalBytes);
            Structs = new UInt64[structs];

            for (int x = 0; x < structs; x++)
                Structs[x] = (UInt64)NextRandom(UInt64.MinValue, UInt64.MaxValue);

            Bytes = StructArray.GetBytes(Structs);
        }

        public System.IO.FileStream GetFileStream()
        {
			WriteFileToDisk();
            return new System.IO.FileStream(TestFileName, FileMode.Open);
        }

        public System.IO.FileStream GetFileStreamWithBufferSize(int bufferSize)
        {
			WriteFileToDisk();
            return new System.IO.FileStream(TestFileName, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize);
        }

        public System.IO.MemoryStream GetMemoryStream()
        {
            return new System.IO.MemoryStream(Bytes);
        }

        private void WriteFileToDisk() 
        {
			if (! _fileInDisk) 
            {
				File.WriteAllBytes(TestFileName, Bytes);
				_fileInDisk = true;
            }
        }

		private int NextRandom(int minimum, int maximum)
        {
			return _random.Next(minimum, maximum);
        }

		private long NextRandom(long minimum, long maximum)
        {
			_random.NextBytes(_randomBuffer);
            Struct.FromArray(_randomBuffer, out long longRand, 0);
			return (Math.Abs(longRand % (maximum - minimum)) + minimum);
        }

		private ulong NextRandom(ulong minimum, ulong maximum)
        {
			_random.NextBytes(_randomBuffer);
            Struct.FromArray(_randomBuffer, out ulong longRand, 0);
			return (longRand % (maximum - minimum)) + minimum;
        }

        private double NextRandom(double minimum, double maximum)
        {
			_random.NextBytes(_randomBuffer);
            Struct.FromArray(_randomBuffer, out long longRand, 0);
			return (double)(Math.Abs(longRand % (maximum - minimum)) + minimum);
        }
    }

  
	public class RandomSingleGenerator
    {
        /* Amount of randomized data in Megabytes. */
        public const string TestFileName = "Singles.bin";
        private static Random _random = new Random();
        private static byte[] _randomBuffer = new byte[8];

        /* Target size of buffers for testing. */
        public Single[] Structs { get; set; }
        public byte[] Bytes { get; set; }
        private bool _fileInDisk = false;

        /* Construction/Destruction */
        public RandomSingleGenerator(int megabytes)
        {
            int totalBytes = Mathematics.MegaBytesToBytes(megabytes);
            int structs = Mathematics.BytesToStructCount<Single>(totalBytes);
            Structs = new Single[structs];

            for (int x = 0; x < structs; x++)
                Structs[x] = (Single)NextRandom(Single.MinValue, Single.MaxValue);

            Bytes = StructArray.GetBytes(Structs);
        }

        public System.IO.FileStream GetFileStream()
        {
			WriteFileToDisk();
            return new System.IO.FileStream(TestFileName, FileMode.Open);
        }

        public System.IO.FileStream GetFileStreamWithBufferSize(int bufferSize)
        {
			WriteFileToDisk();
            return new System.IO.FileStream(TestFileName, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize);
        }

        public System.IO.MemoryStream GetMemoryStream()
        {
            return new System.IO.MemoryStream(Bytes);
        }

        private void WriteFileToDisk() 
        {
			if (! _fileInDisk) 
            {
				File.WriteAllBytes(TestFileName, Bytes);
				_fileInDisk = true;
            }
        }

		private int NextRandom(int minimum, int maximum)
        {
			return _random.Next(minimum, maximum);
        }

		private long NextRandom(long minimum, long maximum)
        {
			_random.NextBytes(_randomBuffer);
            Struct.FromArray(_randomBuffer, out long longRand, 0);
			return (Math.Abs(longRand % (maximum - minimum)) + minimum);
        }

		private ulong NextRandom(ulong minimum, ulong maximum)
        {
			_random.NextBytes(_randomBuffer);
            Struct.FromArray(_randomBuffer, out ulong longRand, 0);
			return (longRand % (maximum - minimum)) + minimum;
        }

        private double NextRandom(double minimum, double maximum)
        {
			_random.NextBytes(_randomBuffer);
            Struct.FromArray(_randomBuffer, out long longRand, 0);
			return (double)(Math.Abs(longRand % (maximum - minimum)) + minimum);
        }
    }

  
	public class RandomDoubleGenerator
    {
        /* Amount of randomized data in Megabytes. */
        public const string TestFileName = "Doubles.bin";
        private static Random _random = new Random();
        private static byte[] _randomBuffer = new byte[8];

        /* Target size of buffers for testing. */
        public Double[] Structs { get; set; }
        public byte[] Bytes { get; set; }
        private bool _fileInDisk = false;

        /* Construction/Destruction */
        public RandomDoubleGenerator(int megabytes)
        {
            int totalBytes = Mathematics.MegaBytesToBytes(megabytes);
            int structs = Mathematics.BytesToStructCount<Double>(totalBytes);
            Structs = new Double[structs];

            for (int x = 0; x < structs; x++)
                Structs[x] = (Double)NextRandom(Double.MinValue, Double.MaxValue);

            Bytes = StructArray.GetBytes(Structs);
        }

        public System.IO.FileStream GetFileStream()
        {
			WriteFileToDisk();
            return new System.IO.FileStream(TestFileName, FileMode.Open);
        }

        public System.IO.FileStream GetFileStreamWithBufferSize(int bufferSize)
        {
			WriteFileToDisk();
            return new System.IO.FileStream(TestFileName, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize);
        }

        public System.IO.MemoryStream GetMemoryStream()
        {
            return new System.IO.MemoryStream(Bytes);
        }

        private void WriteFileToDisk() 
        {
			if (! _fileInDisk) 
            {
				File.WriteAllBytes(TestFileName, Bytes);
				_fileInDisk = true;
            }
        }

		private int NextRandom(int minimum, int maximum)
        {
			return _random.Next(minimum, maximum);
        }

		private long NextRandom(long minimum, long maximum)
        {
			_random.NextBytes(_randomBuffer);
            Struct.FromArray(_randomBuffer, out long longRand, 0);
			return (Math.Abs(longRand % (maximum - minimum)) + minimum);
        }

		private ulong NextRandom(ulong minimum, ulong maximum)
        {
			_random.NextBytes(_randomBuffer);
            Struct.FromArray(_randomBuffer, out ulong longRand, 0);
			return (longRand % (maximum - minimum)) + minimum;
        }

        private double NextRandom(double minimum, double maximum)
        {
			_random.NextBytes(_randomBuffer);
            Struct.FromArray(_randomBuffer, out long longRand, 0);
			return (double)(Math.Abs(longRand % (maximum - minimum)) + minimum);
        }
    }

}
