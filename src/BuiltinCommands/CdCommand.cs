internal class CdCommand : IBuiltinCommand
{
    public string Name { get; } = "cd";

    public int Execute(string[] args, bool isRedirectionExists, int redirectionIndex, bool isOutputRedirection, bool isErrorRedirection, bool isRedirectionAppend)
    {
        var path = args[1];
        if (path == "~")
        {
            var homeDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            Directory.SetCurrentDirectory(homeDir);
            return 0;
        }

        if (Directory.Exists(path))
        {
            Directory.SetCurrentDirectory(path);
            return 0;
        }

        System.Console.WriteLine($"cd: {path}: No such file or directory");
        return 0;
    }
}