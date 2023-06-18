
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RoslynObfuscatorLinux
{
    public class Traverser
    {
        private readonly string folder;

        private IEnumerable<KeyValuePair<string, string[]>> projects;

        public Traverser(string folder)
        {
            this.folder = folder;
        }

        public void Walk()
        {
            projects  = Directory.GetFiles(folder, "*.csproj", SearchOption.AllDirectories)
            .Select(f => new KeyValuePair<string, string>(Path.GetFileNameWithoutExtension(f),  Path.GetDirectoryName(f)))
            .OrderByDescending(g => g.Value)
            .Select(g => new KeyValuePair<string, string[]>(g.Key, Directory.GetFiles(g.Value, "*.cs", SearchOption.AllDirectories)));
        }




    }
}