using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;

namespace Reloaded.Memory.Utilities
{
    /// <summary>
    /// A group of useful utility methods for determining if a type is blittable.
    /// </summary>
    public static class Blittable
    {
        /// <summary>
        /// Returns true if a type is blittable, else false.
        /// </summary>
        public static bool IsBlittable<T>()
        {
            return IsBlittableCache<T>.Value;
        }

        /// <summary>
        /// Checks if a type is blittable.
        /// </summary>
        public static bool IsBlittable(Type type)
        {
            if (type.IsArray)
            {
                var elem = type.GetElementType();
                return elem.IsValueType && IsBlittable(elem);
            }
            try
            {
                object instance = FormatterServices.GetUninitializedObject(type);
                GCHandle.Alloc(instance, GCHandleType.Pinned).Free();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static class IsBlittableCache<T>
        {
            public static readonly bool Value = IsBlittable(typeof(T));
        }
    }
}
