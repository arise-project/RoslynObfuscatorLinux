using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.MSBuild;

namespace RoslynObfuscatorLinux
{
    class Program
    {
        private static string xorSource = @"
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
            using (MSBuildWorkspace workspace = MSBuildWorkspace.Create())
            {
                // Print message for WorkspaceFailed event to help diagnosing project load failures.    
                //Uncomment to debug
                //workspace.WorkspaceFailed += (o, e) => Console.WriteLine(e.Diagnostic.Message);

                Console.WriteLine($"Loading solution '{solutionPath}'");

                // Attach progress reporter so we print projects as they are loaded.
                Solution solution = await workspace.OpenSolutionAsync(solutionPath, new ConsoleProgressReporter());
                Console.WriteLine($"Finished loading solution '{solutionPath}'");

                var mainTrigger = false;

                foreach (var projectId in solution.ProjectIds)
                {
                    Project project = solution.GetProject(projectId);

                    foreach (var documentId in project.DocumentIds)
                    {
                        Document orgDocument = project.GetDocument(documentId);

                        SyntaxNode orgRoot = await orgDocument.GetSyntaxRootAsync();


                        if (orgRoot.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault() != null && obfuStrings)
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

                                    project = xorDocument.Project;

                                    orgDocument = project.GetDocument(documentId);

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

                            project = newDocument.Project;

                            #endregion
                        }
                    }

                    //Persist the project changes to the current solution
                    solution = project.Solution;
                }


                foreach (var projectId in solution.ProjectIds)
                {
                    Project project = solution.GetProject(projectId);

                    foreach (var documentId in project.DocumentIds)
                    {


                        if (obfuVars)
                            project = await CSharpObfuscator.ObfuscateSyntaxNodes<VariableDeclaratorSyntax>(project, documentId);

                        if (obfuMethods)
                            project = await CSharpObfuscator.ObfuscateSyntaxNodes<MethodDeclarationSyntax>(project, documentId);

                        if (obfuClasses)
                            project = await CSharpObfuscator.ObfuscateSyntaxNodes<ClassDeclarationSyntax>(project, documentId);
                    }


                    //Persist the project changes to the current solution
                    solution = project.Solution;
                }


                //Finally, apply all your changes to the workspace at once.
                var didItWork = workspace.TryApplyChanges(solution);
            }
        }
    }
}
