﻿using System.Diagnostics;
using Reloaded.Memory.Sources;

namespace Reloaded.Memory.Tests.Memory.Helpers
{
    public class IMemoryTools
    {
        /// <summary>
        /// If the memory source is of type ExternalMemory, give it a new instance of this process.
        /// </summary>
        public static void SwapExternalMemorySource(ref Reloaded.Memory.Sources.IMemory memorySource, Process newProcess = null)
        {
            // While running tests with xUnit, dotnet can seemingly restart itself causing for the old handle set in
            // the IMemoryGenerator class to be invalid.

            // This is a hack which replaces the ExternalMemory instance such that the tests run on the correct handle.
            if (memorySource.GetType() == typeof(ExternalMemory))
            {
                if (newProcess != null)
                    memorySource = new Reloaded.Memory.Sources.ExternalMemory(newProcess);
                else
                    memorySource = new Reloaded.Memory.Sources.ExternalMemory(Process.GetCurrentProcess());
            }
                
        }
    }
}
