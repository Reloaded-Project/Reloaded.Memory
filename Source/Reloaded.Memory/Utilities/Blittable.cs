using System;
#if NET5_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

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
        public static bool IsBlittable<
#if NET5_0_OR_GREATER
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
#endif
        T>()
        {
            return IsBlittableCache<T>.Value;
        }

        /// <summary>
        /// Checks if a type is blittable.
        /// </summary>
#if NET5_0_OR_GREATER
        [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2072", Justification = "Analyzer reflection skill issue in IsBlittable.")]
#endif
        public static bool IsBlittable(
#if NET5_0_OR_GREATER
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] 
#endif
            Type type)
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
        
        private static class IsBlittableCache<
#if NET5_0_OR_GREATER
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
#endif
        T>
        {
            public static readonly bool Value = IsBlittable(typeof(T));
        }
    }
}
