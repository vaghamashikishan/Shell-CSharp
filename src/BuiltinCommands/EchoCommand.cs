public class EchoCommand : IBuiltinCommand
{
    public string Name { get; } = "echo";

    public int Execute(string[] args, bool isRedirectionExists, int redirectionIndex)
    {
        if (isRedirectionExists)
        {
            var redirectOutput = new RedirectOutput();
            var output = args.Skip(1).Take(redirectionIndex - 2).ToArray();
            redirectOutput.Execute(output, redirectionIndex, args);
        }
        else
        {
            System.Console.WriteLine(string.Join(" ", args.Skip(1)));
        }
        return 0;
    }
}
