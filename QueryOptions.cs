// Decompiled with JetBrains decompiler
// Type: Microsoft.Build.Locator.VisualStudioInstanceQueryOptions
// Assembly: Microsoft.Build.Locator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9dff12846e04bfbd
// MVID: 18349E3C-7AB3-4DEC-9B7B-051CF13408B8
// Assembly location: C:\Users\epiro\.nuget\packages\microsoft.build.locator\1.0.13\lib\net46\Microsoft.Build.Locator.dll

namespace Microsoft.Build.Locator
{
    public class VisualStudioInstanceQueryOptions
    {
        public static VisualStudioInstanceQueryOptions Default => new VisualStudioInstanceQueryOptions()
        {
            DiscoveryTypes = (DiscoveryType)0
        };

        public DiscoveryType DiscoveryTypes { get; set; }
    }
}
