
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RoslynObfuscatorLinux
{
    public class Parser
    {
        private readonly string file;

        public Parser(string file)
        {
            this.file = file;
        }

        public void Parse()
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(File.ReadAllText(file));
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

        }
    }
}