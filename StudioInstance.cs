// Decompiled with JetBrains decompiler
// Type: Microsoft.Build.Locator.VisualStudioInstance
// Assembly: Microsoft.Build.Locator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9dff12846e04bfbd
// MVID: 18349E3C-7AB3-4DEC-9B7B-051CF13408B8
// Assembly location: C:\Users\epiro\.nuget\packages\microsoft.build.locator\1.0.13\lib\net46\Microsoft.Build.Locator.dll

using System;
using System.IO;

namespace Microsoft.Build.Locator
{
    public class VisualStudioInstance
    {
        internal VisualStudioInstance(
          string name,
          string path,
          Version version,
          DiscoveryType discoveryType)
        {
            this.Name = name;
            this.VisualStudioRootPath = path;
            this.Version = version;
            this.DiscoveryType = discoveryType;
            this.MSBuildPath = Path.Combine(this.VisualStudioRootPath, "MSBuild", "15.0", "Bin");
        }

        public Version Version { get; }

        public string VisualStudioRootPath { get; }

        public string Name { get; }

        public string MSBuildPath { get; }

        public DiscoveryType DiscoveryType { get; }
    }
}
