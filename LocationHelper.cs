// Decompiled with JetBrains decompiler
// Type: Microsoft.Build.Locator.VisualStudioLocationHelper
// Assembly: Microsoft.Build.Locator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9dff12846e04bfbd
// MVID: 18349E3C-7AB3-4DEC-9B7B-051CF13408B8
// Assembly location: C:\Users\epiro\.nuget\packages\microsoft.build.locator\1.0.13\lib\net46\Microsoft.Build.Locator.dll

using Microsoft.VisualStudio.Setup.Configuration;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.Build.Locator
{
    internal class VisualStudioLocationHelper
    {
        private const int REGDB_E_CLASSNOTREG = -2147221164;

        internal static IList<VisualStudioInstance> GetInstances()
        {
            List<VisualStudioInstance> instances = new List<VisualStudioInstance>();
            try
            {
                // ISSUE: reference to a compiler-generated method
                // ISSUE: variable of a compiler-generated type
                IEnumSetupInstances enumSetupInstances = ((ISetupConfiguration2)VisualStudioLocationHelper.GetQuery()).EnumAllInstances();
                ISetupInstance[] rgelt = new ISetupInstance[1];
                int pceltFetched;
                do
                {
                    // ISSUE: reference to a compiler-generated method
                    enumSetupInstances.Next(1, rgelt, out pceltFetched);
                    if (pceltFetched > 0)
                    {
                        // ISSUE: variable of a compiler-generated type
                        ISetupInstance setupInstance = rgelt[0];
                        // ISSUE: reference to a compiler-generated method
                        // ISSUE: variable of a compiler-generated type
                        InstanceState state = ((ISetupInstance2)setupInstance).GetState();
                        Version result;
                        // ISSUE: reference to a compiler-generated method
                        if (Version.TryParse(setupInstance.GetInstallationVersion(), out result) && (state == InstanceState.Complete || state.HasFlag((Enum)InstanceState.Registered) && state.HasFlag((Enum)InstanceState.NoRebootRequired)))
                        {
                            // ISSUE: reference to a compiler-generated method
                            // ISSUE: reference to a compiler-generated method
                            instances.Add(new VisualStudioInstance(setupInstance.GetDisplayName(), setupInstance.GetInstallationPath(), result, DiscoveryType.VisualStudioSetup));
                        }
                    }
                }
                while (pceltFetched > 0);
            }
            catch (COMException ex)
            {
            }
            catch (DllNotFoundException ex)
            {
            }
            return (IList<VisualStudioInstance>)instances;
        }

        private static ISetupConfiguration GetQuery()
        {
            try
            {
                // ISSUE: variable of a compiler-generated type
                ISetupConfiguration instance = (ISetupConfiguration)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("177F0C4A-1CD3-4DE7-A32C-71DBBB9FA36D")));
                return instance;
            }
            catch (COMException ex) when (ex.ErrorCode == -2147221164)
            {
                // ISSUE: variable of a compiler-generated type
                ISetupConfiguration configuration;
                int setupConfiguration = VisualStudioLocationHelper.GetSetupConfiguration(out configuration, IntPtr.Zero);
                if (setupConfiguration < 0)
                    throw new COMException(string.Format("Failed to get {0}", (object)"query"), setupConfiguration);
                // ISSUE: variable of a compiler-generated type
                ISetupConfiguration query = configuration;
                return query;
            }
        }

        [DllImport("Microsoft.VisualStudio.Setup.Configuration.Native.dll")]
        private static extern int GetSetupConfiguration(
          [MarshalAs(UnmanagedType.Interface)] out ISetupConfiguration configuration,
          IntPtr reserved);
    }
}
