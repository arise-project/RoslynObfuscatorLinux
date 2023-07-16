// Decompiled with JetBrains decompiler
// Type: Microsoft.Build.Locator.MSBuildLocator
// Assembly: Microsoft.Build.Locator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9dff12846e04bfbd
// MVID: 18349E3C-7AB3-4DEC-9B7B-051CF13408B8
// Assembly location: C:\Users\epiro\.nuget\packages\microsoft.build.locator\1.0.13\lib\net46\Microsoft.Build.Locator.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Microsoft.Build.Locator
{
    [Flags]
    public enum DiscoveryType
    {
        DeveloperConsole = 1,
        VisualStudioSetup = 2,
    }

    public static class MSBuildLocator
    {
        private const string MSBuildPublicKeyToken = "b03f5f7f11d50a3a";
        private static readonly string[] s_msBuildAssemblies = new string[4]
        {
      "Microsoft.Build",
      "Microsoft.Build.Framework",
      "Microsoft.Build.Tasks.Core",
      "Microsoft.Build.Utilities.Core"
        };

        public static IEnumerable<VisualStudioInstance> QueryVisualStudioInstances() => MSBuildLocator.QueryVisualStudioInstances(VisualStudioInstanceQueryOptions.Default);

        public static IEnumerable<VisualStudioInstance> QueryVisualStudioInstances(
          VisualStudioInstanceQueryOptions options)
        {
            return MSBuildLocator.GetInstances().Where<VisualStudioInstance>((Func<VisualStudioInstance, bool>)(i => i.DiscoveryType.HasFlag((Enum)options.DiscoveryTypes)));
        }

        public static VisualStudioInstance RegisterDefaults()
        {
            VisualStudioInstance instance = MSBuildLocator.GetInstances().FirstOrDefault<VisualStudioInstance>();
            MSBuildLocator.RegisterInstance(instance);
            return instance;
        }

        public static void RegisterInstance(VisualStudioInstance instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            IEnumerable<Assembly> source = ((IEnumerable<Assembly>)AppDomain.CurrentDomain.GetAssemblies()).Where<Assembly>(new Func<Assembly, bool>(MSBuildLocator.IsMSBuildAssembly));
            if (source.Any<Assembly>())
            {
                string str = string.Join<AssemblyName>(Environment.NewLine, source.Select<Assembly, AssemblyName>((Func<Assembly, AssemblyName>)(a => a.GetName())));
                throw new InvalidOperationException(string.Format("{0}.{1} was called, but MSBuild assemblies were already loaded.", (object)typeof(MSBuildLocator), (object)nameof(RegisterInstance)) + Environment.NewLine + string.Format("Ensure that {0} is called before any method that directly references types in the Microsoft.Build namespace has been called.", (object)nameof(RegisterInstance)) + Environment.NewLine + "Loaded MSBuild assemblies: " + str);
            }
            AppDomain.CurrentDomain.AssemblyResolve += (ResolveEventHandler)((_, eventArgs) =>
            {
                AssemblyName assemblyName = new AssemblyName(eventArgs.Name);
                if (!MSBuildLocator.IsMSBuildAssembly(assemblyName))
                    return (Assembly)null;
                string str = Path.Combine(instance.MSBuildPath, assemblyName.Name + ".dll");
                return !File.Exists(str) ? (Assembly)null : Assembly.LoadFrom(str);
            });
        }

        private static bool IsMSBuildAssembly(Assembly assembly) => MSBuildLocator.IsMSBuildAssembly(assembly.GetName());

        private static bool IsMSBuildAssembly(AssemblyName assemblyName)
        {
            if (!((IEnumerable<string>)MSBuildLocator.s_msBuildAssemblies).Contains<string>(assemblyName.Name, (IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase))
                return false;
            byte[] publicKeyToken = assemblyName.GetPublicKeyToken();
            if (publicKeyToken == null || publicKeyToken.Length == 0)
                return false;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte num in publicKeyToken)
                stringBuilder.Append(string.Format("{0:x2}", (object)num));
            return stringBuilder.ToString().Equals("b03f5f7f11d50a3a", StringComparison.OrdinalIgnoreCase);
        }

        private static IEnumerable<VisualStudioInstance> GetInstances()
        {
            VisualStudioInstance devConsoleInstance = MSBuildLocator.GetDevConsoleInstance();
            if (devConsoleInstance != null)
                yield return devConsoleInstance;
            foreach (VisualStudioInstance instance in (IEnumerable<VisualStudioInstance>)VisualStudioLocationHelper.GetInstances())
                yield return instance;
        }

        private static VisualStudioInstance GetDevConsoleInstance()
        {
            string environmentVariable = Environment.GetEnvironmentVariable("VSINSTALLDIR");
            if (string.IsNullOrEmpty(environmentVariable))
                return (VisualStudioInstance)null;
            Version result;
            Version.TryParse(Environment.GetEnvironmentVariable("VSCMD_VER"), out result);
            if (result == (Version)null)
                Version.TryParse(Environment.GetEnvironmentVariable("VisualStudioVersion"), out result);
            return new VisualStudioInstance("DEVCONSOLE", environmentVariable, result, DiscoveryType.DeveloperConsole);
        }
    }
}
