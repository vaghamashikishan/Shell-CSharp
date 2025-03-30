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
    var (parameters, isRedirectionExists, redirectionIndex, isOutputRedirection, isErrorRedirection) = parser.Parse(userInput);
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
            builtinCommand.Execute(parameters.ToArray(), isRedirectionExists, redirectionIndex, isOutputRedirection, isErrorRedirection);
            continue;
        }

        // executing external programs
        var programPath = executableDirectories.GetProgramPath(command);
        var newParameters = redirectionIndex > 1
            ? parameters.Skip(1).Take(redirectionIndex - 2).ToArray()
            : parameters.Skip(1).ToArray();

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
                    if (isOutputRedirection)
                    {
                        if (resultText.Length > 0) resultText += "\n";
                        resultText += args.Data;
                    }
                    else
                    {
                        System.Console.WriteLine(args.Data);
                    }
                }
            };

            process.ErrorDataReceived += (sender, args) =>
            {
                if (args.Data != null)
                {
                    if (isErrorRedirection)
                    {
                        if (errorText.Length > 0) errorText += "\n";
                        errorText += args.Data;
                    }
                    else
                    {
                        System.Console.WriteLine(args.Data);
                    }
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await process.WaitForExitAsync();

            if (isRedirectionExists)
            {
                var str = isErrorRedirection ? errorText : resultText;
                System.Console.WriteLine($"it is -> {str}");
                var redirectOutput = new RedirectOutput();
                redirectOutput.Execute([str.Trim()], redirectionIndex, parameters.ToArray());
            }
            continue;
        }

        Console.WriteLine($"{userInput}: command not found");
    }
}

return 0;