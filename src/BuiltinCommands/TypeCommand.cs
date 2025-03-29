internal class TypeCommand(Dictionary<string, IBuiltinCommand> builtinCommandsMap, ExecutableDirectories executableDirectories) : IBuiltinCommand
{
    public string Name { get; } = "type";

    public int Execute(string[] args)
    {
        foreach (var programName in args.Skip(1))
        {
            if (builtinCommandsMap.ContainsKey(programName))
            {
                System.Console.WriteLine($"{programName} is a shell builtin");
                continue;
            }

            var programPath = executableDirectories.GetProgramPath(programName);
            if (programPath != null)
            {
                System.Console.WriteLine($"{programName} is {programPath} builtin");
            }
            else
            {
                System.Console.WriteLine($"{programName}: not found");
            }
        }

        return 0;
    }
}