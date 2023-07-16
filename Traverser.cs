extern alias bb;
using System.Text;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace RoslynObfuscatorLinux
{
    public class Traverser
    {
        public static string xorSource = @"
       public static byte[] xorEncDec(byte[] input, string theKeystring)
       {
            byte[] theKey = System.Text.Encoding.UTF8.GetBytes(theKeystring);
            byte[] mixed = new byte[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                mixed[i] = (byte)(input[i] ^ theKey[i % theKey.Length]);
            }
            return mixed;
        }
";
        private readonly string solutionPath;

        public Traverser(string sln)
        {
            solutionPath = sln;
        }

        public async Task Walk()
        {
            //https://stackoverflow.com/questions/69297342/can-not-get-roslyn-to-work-with-net-5-project
            MSBuildLocator.RegisterDefaults();

            Console.WriteLine($"Loading solution '{solutionPath}'");

            using (bb::Microsoft.CodeAnalysis.MSBuild.MSBuildWorkspace workspace = bb::Microsoft.CodeAnalysis.MSBuild.MSBuildWorkspace.Create())
            {
                Solution solution = await workspace.OpenSolutionAsync(solutionPath, new ConsoleProgressReporter());
                // Print message for WorkspaceFailed event to help diagnosing project load failures.    
                //Uncomment to debug
                //workspace.WorkspaceFailed += (o, e) => Console.WriteLine(e.Diagnostic.Message);
                
                Console.WriteLine($"Finished loading solution '{solutionPath}'");

                var mainTrigger = false;
                Console.WriteLine();
                foreach (var diag in (solution.Workspace as bb::Microsoft.CodeAnalysis.MSBuild.MSBuildWorkspace)?.Diagnostics )
                {
                    Console.WriteLine($"WARN : {diag.Message}");
                    Console.WriteLine();
                }

                foreach (var project in solution.Projects)
                {
                    foreach (var documentId in project.DocumentIds)
                    {
                        Document orgDocument = project.GetDocument(documentId);

                        SyntaxNode orgRoot = await orgDocument.GetSyntaxRootAsync();


                        if (orgRoot.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault() != null && Program.obfuStrings)
                        {
                            if (!mainTrigger)
                            {
                                var rawDocText = (await orgDocument.GetTextAsync()).ToString().Split('\n');

                                var mainLine = rawDocText.Where(x => x.Contains("Main(")).FirstOrDefault();

                                if (mainLine != null)
                                {

                                    var NewmainLine = xorSource + "\n\n\n" + mainLine;

                                    var compiledraw = string.Join("\n", rawDocText);

                                    compiledraw = compiledraw.Replace(mainLine, NewmainLine);

                                    var xorNode = CSharpSyntaxTree.ParseText(compiledraw).GetRoot();

                                    var xorEditor = await DocumentEditor.CreateAsync(orgDocument);

                                    xorEditor.ReplaceNode(orgRoot, xorNode);

                                    var xorDocument = xorEditor.GetChangedDocument();

                                    var project1 = xorDocument.Project;

                                    orgDocument = project1.GetDocument(documentId);

                                    orgRoot = await orgDocument.GetSyntaxRootAsync();

                                    mainTrigger = true;
                                }
                            }

                            #region stringObfuscation 
                            var stringObfu = new Rewriter();

                            stringObfu.Visit(orgRoot);

                            var rootText = orgRoot.GetText().ToString();

                            foreach (var dictItem in stringObfu.StringDict)
                            {
                                rootText = rootText.Replace(dictItem.before, dictItem.after);
                            }

                            var newRoot = CSharpSyntaxTree.ParseText(rootText).GetRoot();

                            var editor = await DocumentEditor.CreateAsync(orgDocument);

                            editor.ReplaceNode(orgRoot, newRoot);

                            var newDocument = editor.GetChangedDocument();

                            var project2 = newDocument.Project;

                            #endregion
                        }
                    }

                    //Persist the project changes to the current solution
                    var solution1 = project.Solution;
                }


                foreach (var project in workspace.CurrentSolution.Projects)
                {
                    foreach (var documentId in project.DocumentIds)
                    {


                        if (Program.obfuVars)
                            await CSharpObfuscator.ObfuscateSyntaxNodes<VariableDeclaratorSyntax>(project, documentId);

                        if (Program.obfuMethods)
                            await CSharpObfuscator.ObfuscateSyntaxNodes<MethodDeclarationSyntax>(project, documentId);

                        if (Program.obfuClasses)
                            await CSharpObfuscator.ObfuscateSyntaxNodes<ClassDeclarationSyntax>(project, documentId);
                    }


                    //Persist the project changes to the current solution
                    var solution1 = project.Solution;
                }


                //Finally, apply all your changes to the workspace at once.
                var didItWork = workspace.TryApplyChanges(workspace.CurrentSolution.Projects.First().Solution);
            }
        }
    }
}