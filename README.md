# RosFuscator
YouTube/Livestream project for obfuscating C# source code using Roslyn


# Todo /Fixes
Some of MANY that needs to be fixed before automated usage
* Make sure i don't obfuscate a method that is demanded by an interface i cannot modify.
* Make sure i don't obfuscate strings that needs to be constant
* Make sure strings that are obfuscated can reach the xorEncDec Function.


Issues:

WARN : Msbuild failed when processing the file 'C:\Users\epiro\Documents\proj\switch_knife\SwitchKnifeApp.csproj' with message: The imported project "C:\Users\epiro\Documents\proj\RoslynObfuscatorLinux\bin\Debug\net6.0\Sdks\Microsoft.NET.Sdk\Sdk\Sdk.props" was not found. Confirm that the path in the <Import> declaration is correct, and that the file exists on disk.  C:\Users\epiro\Documents\proj\switch_knife\SwitchKnifeApp.csproj

https://github.com/Microsoft/msbuild/issues/1697 talks about the issue, current workaround is to set env.MSBuildSDKsPath to the right place, e.g. C:\Program Files\dotnet\sdk\1.0.4\Sdks. If it is net core 2.0, also try #1752 (comment)


