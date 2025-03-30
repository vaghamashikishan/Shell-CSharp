internal class ExitCommand : IBuiltinCommand
{
    public string Name { get; } = "exit";
    public static string CommandName = "exit";
    public int Execute(string[] args, bool isRedirectionExists, int redirectionIndex, bool isOutputRedirection, bool isErrorRedirection, bool isRedirectionAppend)
    {
        if (args.Length > 1 && int.TryParse(args[1], out var exitCode))
        {
            return exitCode;
        }
        return 0;
    }
}