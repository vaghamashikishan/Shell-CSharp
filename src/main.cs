var executableDirectories = new ExecutableDirectories(Environment.GetEnvironmentVariable("PATH") ?? "");
var builtinCommandsMap = new Dictionary<string, IBuiltinCommand>();
var builtinCommands = new List<IBuiltinCommand>()
{
    new EchoCommand(),
    new TypeCommand(builtinCommandsMap, executableDirectories),
    new ExitCommand()
};

foreach (var command in builtinCommands)
{
    builtinCommandsMap[command.Name] = command;
}

bool run = true;
while (run)
{
    Console.Write("$ ");
    var userInput = Console.ReadLine() ?? string.Empty;
    // parse the input here, till then
    var parameters = userInput.Split(" ");
    // derive command from parsed input, till then do work on
    var command = userInput.Split(" ")[0];

    if (userInput != null)
    {
        if (command == ExitCommand.CommandName)
        {
            return builtinCommandsMap[command].Execute(parameters);
        }

        if (builtinCommandsMap.TryGetValue(command, out var builtinCommand) == true)
        {
            builtinCommand.Execute(parameters);
            continue;
        }
    }
    Console.WriteLine($"{userInput}: command not found");
}

return 0;


// jo command 'type' hoi to
// -> to typeCommand class ne execute karvano
// --> --> tema check karvanu k inbuilt 6e k p6i environment variable 6e 