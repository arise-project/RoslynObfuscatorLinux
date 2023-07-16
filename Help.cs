
namespace RoslynObfuscatorLinux
{
    public static class Help
    {
        public static void Print()
        {
                string asciArt = @"
    ██▀███   ▒█████    ██████   █████▒█    ██   ██████  ▄████▄   ▄▄▄     ▄▄▄█████▓ ▒█████   ██▀███  
    ▓██ ▒ ██▒▒██▒  ██▒▒██    ▒ ▓██   ▒ ██  ▓██▒▒██    ▒ ▒██▀ ▀█  ▒████▄   ▓  ██▒ ▓▒▒██▒  ██▒▓██ ▒ ██▒
    ▓██ ░▄█ ▒▒██░  ██▒░ ▓██▄   ▒████ ░▓██  ▒██░░ ▓██▄   ▒▓█    ▄ ▒██  ▀█▄ ▒ ▓██░ ▒░▒██░  ██▒▓██ ░▄█ ▒
    ▒██▀▀█▄  ▒██   ██░  ▒   ██▒░▓█▒  ░▓▓█  ░██░  ▒   ██▒▒▓▓▄ ▄██▒░██▄▄▄▄██░ ▓██▓ ░ ▒██   ██░▒██▀▀█▄  
    ░██▓ ▒██▒░ ████▓▒░▒██████▒▒░▒█░   ▒▒█████▓ ▒██████▒▒▒ ▓███▀ ░ ▓█   ▓██▒ ▒██▒ ░ ░ ████▓▒░░██▓ ▒██▒
    ░ ▒▓ ░▒▓░░ ▒░▒░▒░ ▒ ▒▓▒ ▒ ░ ▒ ░   ░▒▓▒ ▒ ▒ ▒ ▒▓▒ ▒ ░░ ░▒ ▒  ░ ▒▒   ▓▒█░ ▒ ░░   ░ ▒░▒░▒░ ░ ▒▓ ░▒▓░
    ░▒ ░ ▒░  ░ ▒ ▒░ ░ ░▒  ░ ░ ░     ░░▒░ ░ ░ ░ ░▒  ░ ░  ░  ▒     ▒   ▒▒ ░   ░      ░ ▒ ▒░   ░▒ ░ ▒░
    ░░   ░ ░ ░ ░ ▒  ░  ░  ░   ░ ░    ░░░ ░ ░ ░  ░  ░  ░          ░   ▒    ░      ░ ░ ░ ▒    ░░   ░ 
    ░         ░ ░        ░            ░           ░  ░ ░            ░  ░            ░ ░     ░     
                                                        ░                                           
    Origin: @Flangvik

    #Obfuscate only strings and methods
    Example: ./RosFuscator.exe /path/to/target/solution/SeatBelt.sln --strings --methods

    #Obfuscate all the things!
    Example: ./RosFuscator.exe /path/to/target/solution/SeatBelt.sln 

    Major credits to https://stackoverflow.com/questions/59069677/roslyn-save-edited-document-to-physical-solution
    ";
                Console.WriteLine(asciArt);
        }
    }
}