public class EchoCommand : IBuiltinCommand
{
    public string Name { get; } = "echo";

    public int Execute(string[] args)
    {
        System.Console.WriteLine(string.Join(" ", args.Skip(1)));
        return 0;
    }
}
