public class EchoCommand : IBuiltinCommand
{
    public string Name { get; } = "echo";

    public int Execute(string[] args, bool isRedirectionExists, int redirectionIndex, bool isOutputRedirection, bool isErrorRedirection)
    {
        var output = isRedirectionExists
        ? string.Join(" ", args.Skip(1).Take(redirectionIndex - 2))
        : string.Join(" ", args.Skip(1));

        if (isOutputRedirection)
        {
            var redirectOutput = new RedirectOutput();
            redirectOutput.Execute(output.Split(" "), redirectionIndex, args);
        }
        else
        {
            System.Console.WriteLine(output);
        }
        return 0;
    }
}
