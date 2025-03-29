internal class CdCommand : IBuiltinCommand
{
    public string Name { get; } = "cd";

    public int Execute(string[] args)
    {
        var path = args[1];

        if (Directory.Exists(path))
        {
            Directory.SetCurrentDirectory(path);
            return 0;
        }

        System.Console.WriteLine($"cd: {path}: No such file or directory");
        return 0;
    }
}