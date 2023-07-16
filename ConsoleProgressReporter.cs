
extern alias bb;
using bb::Microsoft.CodeAnalysis.MSBuild;

namespace RoslynObfuscatorLinux
{
    public class ConsoleProgressReporter : IProgress<bb::Microsoft.CodeAnalysis.MSBuild.ProjectLoadProgress>
            {
                public void Report(bb::Microsoft.CodeAnalysis.MSBuild.ProjectLoadProgress loadProgress)
                {
                    var projectDisplay = Path.GetFileName(loadProgress.FilePath);
                    if (loadProgress.TargetFramework != null)
                    {
                        projectDisplay += $" ({loadProgress.TargetFramework})";
                    }

                    Console.WriteLine($"{loadProgress.Operation,-15} {loadProgress.ElapsedTime,-15:m\\:ss\\.fffffff} {projectDisplay}");
                }
            }

}