
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RoslynObfuscatorLinux
{
    public class FileModel
    {
        public string Project { get;set; }
        public string FilePath {get;set;}

        public async Task Obfuscate()
        {
            if (Program.obfuVars)
                await CSharpObfuscator.ObfuscateSyntaxNodes<VariableDeclaratorSyntax>(project, documentId);

            if (Program.obfuMethods)
                await CSharpObfuscator.ObfuscateSyntaxNodes<MethodDeclarationSyntax>(project, documentId);

            if (Program.obfuClasses)
                await CSharpObfuscator.ObfuscateSyntaxNodes<ClassDeclarationSyntax>(project, documentId);
        }
    }
}