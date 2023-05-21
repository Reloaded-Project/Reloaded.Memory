using System.Diagnostics;

namespace Reloaded.Memory.Utilities.License;

/// <summary>
/// Indicates the code was forked from an MIT project; and is still subject to the MIT license.
/// You can copy this code out of Reloaded.Memory and use it in your own projects under the original license.
/// </summary>
[Conditional("INCLUDE_LICENSE_METADATA")]
internal class MITLicense : Attribute { }
