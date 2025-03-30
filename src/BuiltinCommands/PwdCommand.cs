internal class PwdCommand : IBuiltinCommand
{
    public string Name { get; } = "pwd";

    public int Execute(string[] args, bool isRedirectionExists, int redirectionIndex, bool isOutputRedirection, bool isErrorRedirection, bool isRedirectionAppend)
    {
        if (args.Length == 1)
        {
            System.Console.WriteLine(Directory.GetCurrentDirectory());
            return 0;
        }
        return 0;
    }
}