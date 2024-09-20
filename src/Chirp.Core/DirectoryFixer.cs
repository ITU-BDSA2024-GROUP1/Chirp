namespace Chirp.Core;

public static class DirectoryFixer
{
    // With the help of ChatGPT since I could not find a solution through searching google.
    public static void SetWorkingDirectoryToProjectRoot()
    {
        /*           // Locate the project directory by navigating up from the binary directory
                   string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.Parent.Parent.FullName;

                   // Set the working directory to the project directory
                   Directory.SetCurrentDirectory(projectDirectory);
       */
        //Console.WriteLine("Current Working Directory Set To: " + Directory.GetCurrentDirectory());

        while (Path.GetFileName(Directory.GetCurrentDirectory()) != "Chirp")
        {
            Directory.SetCurrentDirectory(Path.GetFullPath(".."));
        }
    }
}