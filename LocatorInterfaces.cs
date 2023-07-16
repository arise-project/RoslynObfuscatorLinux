using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.VisualStudio.Setup.Configuration
{
    [CompilerGenerated]
    [Guid("6380BCFF-41D3-4B2E-8B2E-BF8A6810C848")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [TypeIdentifier]
    [ComImport]
    public interface IEnumSetupInstances
    {
        void Next([MarshalAs(UnmanagedType.U4), In] int celt, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Interface), Out] ISetupInstance[] rgelt, [MarshalAs(UnmanagedType.U4)] out int pceltFetched);
    }

    [CompilerGenerated]
    [Flags]
    [TypeIdentifier("310100ba-5f84-4103-abe0-e8132ae862d9", "Microsoft.VisualStudio.Setup.Configuration.InstanceState")]
    public enum InstanceState : uint
    {
        None = 0,
        Local = 1,
        Registered = 2,
        NoRebootRequired = 4,
        NoErrors = 8,
        Complete = 4294967295, // 0xFFFFFFFF
    }

    [CompilerGenerated]
    [Guid("42843719-DB4C-46C2-8E7C-64F1816EFD5B")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [TypeIdentifier]
    [ComImport]
    public interface ISetupConfiguration
    {
    }


    [CompilerGenerated]
    [Guid("26AAB78C-4A60-49D6-AF3B-3C35BC93365D")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [TypeIdentifier]
    [ComImport]
    public interface ISetupConfiguration2 : ISetupConfiguration
    {
        [SpecialName]
        [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
        sealed extern void _VtblGap1_3();

        [return: MarshalAs(UnmanagedType.Interface)]
        IEnumSetupInstances EnumAllInstances();
    }

    [CompilerGenerated]
    [Guid("B41463C3-8866-43B5-BC33-2B0676F7F42E")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [TypeIdentifier]
    [ComImport]
    public interface ISetupInstance
    {
        [SpecialName]
        [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
        sealed extern void _VtblGap1_3();

        [return: MarshalAs(UnmanagedType.BStr)]
        string GetInstallationPath();

        [return: MarshalAs(UnmanagedType.BStr)]
        string GetInstallationVersion();

        [return: MarshalAs(UnmanagedType.BStr)]
        string GetDisplayName([MarshalAs(UnmanagedType.U4), In] int lcid = 0);
    }

    [CompilerGenerated]
    [Guid("89143C9A-05AF-49B0-B717-72E218A2185C")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [TypeIdentifier]
    [ComImport]
    public interface ISetupInstance2 : ISetupInstance
    {
        [SpecialName]
        [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
        sealed extern void _VtblGap1_8();

        [return: MarshalAs(UnmanagedType.U4)]
        InstanceState GetState();
    }

    [CompilerGenerated]
    [Guid("42843719-DB4C-46C2-8E7C-64F1816EFD5B")]
    [CoClass(typeof(object))]
    [TypeIdentifier]
    [ComImport]
    public interface SetupConfiguration : ISetupConfiguration2, ISetupConfiguration
    {
    }


}