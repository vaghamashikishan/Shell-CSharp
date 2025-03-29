var builtinCommandsMap = new Dictionary<string, IBuiltinCommand>();
var builtinCommands = new List<IBuiltinCommand>()
{
    new EchoCommand()
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
    var parameters = userInput;
    // derive command from parsed input, till then do work on
    var command = userInput.Split(" ")[0];

    if (userInput != null)
    {
        if (command == "exit")
            break;

        if (builtinCommandsMap.TryGetValue(command, out var builtinCommand) == true)
        {
            builtinCommand.Execute(parameters.Split(" "));
            continue;
        }
    }
    Console.WriteLine($"{userInput}: command not found");
}