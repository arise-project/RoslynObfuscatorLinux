using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.MSBuild;

namespace RoslynObfuscatorLinux
{
    class Program
    {
        public static bool obfuMethods { get; private set; }
        public static bool obfuStrings { get; private set; }
        public static bool obfuClasses { get; private set; }
        public static bool obfuVars { get; private set; }

        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Help.Print();
                return;
            }

            var solutionPath = "";

            obfuMethods = false;
            obfuStrings = false;
            obfuClasses = false;
            obfuVars = false;

            solutionPath = args[0];

            if (args.Select(x => x.ToLower()).Contains("--methods"))
                obfuMethods = true;

            if (args.Select(x => x.ToLower()).Contains("--strings"))
                obfuStrings = true;

            if (args.Select(x => x.ToLower()).Contains("--classes"))
                obfuClasses = true;

            if (args.Select(x => x.ToLower()).Contains("--vars"))
                obfuVars = true;

            if (!obfuVars && !obfuClasses && !obfuStrings && !obfuMethods)
            {
                obfuVars = true;
                obfuClasses = true;
                obfuStrings = true;
                obfuMethods = true;
            }


            // https://github.com/xoofx/Broslyn
            // https://github.com/RendleLabs/LegacyWorkspaceLoader
            // MSBuild 15.0 https://gist.github.com/DustinCampbell/32cd69d04ea1c08a16ae5c4cd21dd3a3
            
            await new Traverser(solutionPath).Walk();
        }
    }
}
