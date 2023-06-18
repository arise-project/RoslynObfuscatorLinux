
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RoslynObfuscatorLinux
{
    public class Rewriter : CSharpSyntaxRewriter
    {
        public readonly List<(string before, string after)> StringDict = new List<(string before, string after)>();

        private static string[] BadStrings = new string[] { "DllImport", "const" };

        private static SyntaxKind[] BadTypes = new SyntaxKind[] {
                SyntaxKind.Parameter,
                SyntaxKind.ParameterList,
                SyntaxKind.CaseKeyword,
                SyntaxKind.CasePatternSwitchLabel,
                SyntaxKind.CaseSwitchLabel,
                SyntaxKind.ConstructorConstraint
            };

        public override SyntaxNode Visit(SyntaxNode rawSyntaxNode)
        {
            if (rawSyntaxNode.IsKind(SyntaxKind.StringLiteralExpression))
            {
                var castedSyntaxNode = (LiteralExpressionSyntax)rawSyntaxNode;

                if (castedSyntaxNode != null)
                {
                    //If this is a string node
                    if (castedSyntaxNode.Kind().Equals(SyntaxKind.StringLiteralExpression))
                    {
                        //Turn the node into it's raw string form 
                        var actualStringValue = ((LiteralExpressionSyntax)castedSyntaxNode).Token.ValueText;

                        var actualStringValueOuter = ((LiteralExpressionSyntax)castedSyntaxNode).Token.Text;

                        var declartionOfvalue = ((LiteralExpressionSyntax)castedSyntaxNode).Parent.Parent.ToFullString();

                        //Process the node
                        var randomkey = Guid.NewGuid().ToString();

                        var xorEncString = EncDec.Xor(Encoding.UTF8.GetBytes(actualStringValue), randomkey);

                        var xorEncStringB64 = Convert.ToBase64String(xorEncString);

                        var decrypted = EncDec.Xor(Convert.FromBase64String(xorEncStringB64), randomkey);

                        var decryptedLogic = $"System.Text.Encoding.UTF8.GetString(Program.xorEncDec(System.Convert.FromBase64String(\"{xorEncStringB64}\"), \"{randomkey}\"))";

                        var result = declartionOfvalue.Replace(actualStringValueOuter, decryptedLogic);
                        // Console.WriteLine($"[+] {actualStringValue} => {xorEncStringB64}");

                        try
                        {
                            if (BadStrings.Where(x => castedSyntaxNode.Parent.Parent.Parent.Parent.ToFullString().Contains(x)).Count() > 0)
                                return base.Visit(castedSyntaxNode);
                        }
                        catch (Exception)
                        {

                        }

                        if (castedSyntaxNode.Token.Value.ToString().Contains("\\Users\\"))
                        {
                            var value3 = (castedSyntaxNode.Parent.Parent.Parent.ToFullString());
                            var value2 = (castedSyntaxNode.Parent.Parent.ToFullString());
                            var value1 = (castedSyntaxNode.Parent.ToFullString());

                        }

                        if (!BadTypes.Contains(castedSyntaxNode.Parent.Parent.Parent.Kind()) && !BadTypes.Contains(castedSyntaxNode.Parent.Parent.Kind()) && !BadTypes.Contains(castedSyntaxNode.Parent.Kind()))
                            StringDict.Add((declartionOfvalue, result));

                    }
                    return base.Visit(castedSyntaxNode);
                }
                return base.Visit(castedSyntaxNode);
            }
            return base.Visit(rawSyntaxNode);
        }
    }
}