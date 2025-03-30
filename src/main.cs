using System.Diagnostics;

var parser = new CommandLineParser();
var executableDirectories = new ExecutableDirectories(Environment.GetEnvironmentVariable("PATH") ?? "");
var builtinCommandsMap = new Dictionary<string, IBuiltinCommand>();
var builtinCommands = new List<IBuiltinCommand>()
{
    new EchoCommand(),
    new TypeCommand(builtinCommandsMap, executableDirectories),
    new ExitCommand(),
    new PwdCommand(),
    new CdCommand()
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
    var (parameters, isRedirectionExists, redirectionIndex) = parser.Parse(userInput);
    var command = parameters.FirstOrDefault("");

    if (userInput != null)
    {
        // executing EXIT command
        if (command == ExitCommand.CommandName)
        {
            return builtinCommandsMap[command].Execute(parameters.ToArray());
        }

        // executing BUILT-IN command
        if (builtinCommandsMap.TryGetValue(command, out var builtinCommand) == true)
        {
            builtinCommand.Execute(parameters.ToArray(), isRedirectionExists, redirectionIndex);
            continue;
        }

        // executing external programs
        var programPath = executableDirectories.GetProgramPath(command);
        var newParameters = parameters.Skip(1).Take(redirectionIndex - 2).ToArray();
        if (programPath != null)
        {
            var processStartInfo = new ProcessStartInfo(command, newParameters)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var resultText = "";
            var errorText = "";
            using var process = new Process();
            process.StartInfo = processStartInfo;
            process.OutputDataReceived += (sender, args) =>
            {
                if (args.Data != null)
                {
                    if (resultText.Length > 0) resultText += "\n";
                    resultText += args.Data;
                }
            };

            process.ErrorDataReceived += (sender, args) =>
            {
                if (args.Data != null)
                {
                    errorText += args.Data;
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await process.WaitForExitAsync();

            if (isRedirectionExists)
            {
                var redirectOutput = new RedirectOutput();
                redirectOutput.Execute([resultText.Trim()], redirectionIndex, parameters.ToArray());
            }
            else
            {
                System.Console.WriteLine(resultText.Trim());
                System.Console.WriteLine();
            }
            continue;
        }

        Console.WriteLine($"{userInput}: command not found");
    }
}

return 0;