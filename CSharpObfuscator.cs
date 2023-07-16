
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;

namespace RoslynObfuscatorLinux
{
    public class CSharpObfuscator
    {
        public static async Task<Project> ObfuscateSyntaxNodes<T>(Project inputProject, DocumentId documentId) where T : SyntaxNode
        {

            Project project = inputProject;

            ProjectId projectId = project.Id;

            Document orgDoc = project.GetDocument(documentId);

            SyntaxNode orgRoot = await orgDoc.GetSyntaxRootAsync();

            int countOfTObjects = orgRoot.DescendantNodesAndSelf().OfType<T>().ToArray().Count();

            int maxSize = 0;
            for (int i = 0; i < countOfTObjects; i++)
            {
                Document currentDoc = project.GetDocument(documentId);

                //Get the correct root tree for doc we are modifing
                SyntaxNode currentRoot = await currentDoc.GetSyntaxRootAsync();

                T targetObject = currentRoot.DescendantNodesAndSelf().OfType<T>().ToArray()[i];

                Compilation targetComp = await project.GetCompilationAsync();

                SemanticModel targetModel = targetComp.GetSemanticModel(currentRoot.SyntaxTree);

                ISymbol targetSymbol = targetModel.GetDeclaredSymbol(targetObject);


                if (targetSymbol != null)
                {
                    if (!targetSymbol.Name.Equals("Main"))
                    {

                        //  if (!targetSymbol.IsOverride && !targetSymbol.IsImplicitlyDeclared && !targetSymbol.Name.ToLower().Contains("dispose"))
                        if (!targetSymbol.IsOverride && !targetSymbol.Name.ToLower().Contains("dispose"))
                        {
                            //Generate random nale
                            string newName = "_" + Guid.NewGuid().ToString().Replace("-", "");

                            //Generate a new solution this this modification
                            Solution newSolution = await Renamer.RenameSymbolAsync(project.Solution, targetSymbol, newName, project.Solution.Workspace.Options);

                            var printString = $"[+] {targetSymbol.Name} => {newName}";

                            if (maxSize <= printString.Length)
                                maxSize = printString.Length;

                            Console.Write("\r" + printString.PadRight(maxSize + 10, ' '));

                            project = newSolution.GetProject(projectId);
                        }
                    }
                }

            }
            return project;
        }
    }
}
