using System;
using System.Collections.Generic;
using System.Text;

namespace Reloaded.Memory.Interop
{
    /// <summary>
    /// A <see cref="Pinnable{T}"/> that automatically disposes of the native object once it is either manually disposed
    /// or garbage collected.
    /// </summary>
    public class PinnableDisposable<T> : Pinnable<T> where T : unmanaged, IDisposable
    {
        /// <inheritdoc />
        public PinnableDisposable(T value) : base(value)         { }

        /// <inheritdoc />
        public PinnableDisposable(ref T value) : base(ref value) { }

        /// <summary>Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.</summary>
        ~PinnableDisposable() => Dispose();

        /// <summary/>
        public new void Dispose()
        {
            base.Dispose();
            Value.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
