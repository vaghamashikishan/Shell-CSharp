public class EchoCommand : IBuiltinCommand
{
    public string Name { get; } = "echo";

    public int Execute(string[] args, bool isRedirectionExists, int redirectionIndex, bool isOutputRedirection, bool isErrorRedirection, bool isRedirectionAppend)
    {
        var output = isRedirectionExists
        ? string.Join(" ", args.Skip(1).Take(redirectionIndex - 2))
        : string.Join(" ", args.Skip(1));

        if (isOutputRedirection)
        {
            var redirectOutput = new RedirectOutput();
            redirectOutput.Execute(output.Split(" "), redirectionIndex, args, isRedirectionAppend);
        }
        else
        {
            System.Console.WriteLine(output);

            if (isErrorRedirection)
            {
                var redirectOutput = new RedirectOutput();
                redirectOutput.Execute([""], redirectionIndex, args, isRedirectionAppend);
            }
        }
        return 0;
    }
}
