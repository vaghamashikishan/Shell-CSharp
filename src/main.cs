using System.Diagnostics;

var parser = new CommandLineParser();
var executableDirectories = new ExecutableDirectories(Environment.GetEnvironmentVariable("PATH") ?? "");
var builtinCommandsMap = new Dictionary<string, IBuiltinCommand>();
var builtinCommands = new List<IBuiltinCommand>()
{
    new EchoCommand(),
    new TypeCommand(builtinCommandsMap, executableDirectories),
    new ExitCommand(),
    new PwdCommand()
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
    var parameters = parser.Parse(userInput).ToArray();
    var command = parameters.FirstOrDefault("");

    if (userInput != null)
    {
        // executing EXIT command
        if (command == ExitCommand.CommandName)
        {
            return builtinCommandsMap[command].Execute(parameters);
        }

        // executing BUILT-IN command
        if (builtinCommandsMap.TryGetValue(command, out var builtinCommand) == true)
        {
            builtinCommand.Execute(parameters);
            continue;
        }

        // executing external programs
        var programPath = executableDirectories.GetProgramPath(command);
        if (programPath != null)
        {
            var processStartInfo = new ProcessStartInfo(command, parameters.Skip(1))
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process();
            process.StartInfo = processStartInfo;
            process.OutputDataReceived += (sender, args) =>
            {
                if (args.Data != null)
                {
                    Console.WriteLine(args.Data);
                }
            };

            process.ErrorDataReceived += (sender, args) =>
            {
                if (args.Data != null)
                {
                    Console.Error.WriteLine(args.Data);
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await process.WaitForExitAsync();
            continue;
        }

        Console.WriteLine($"{userInput}: command not found");
    }
}

return 0;