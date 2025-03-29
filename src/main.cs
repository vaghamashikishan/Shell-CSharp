bool run = true;
while (run)
{
    Console.Write("$ ");
    var command = Console.ReadLine();
    if (command != null && command.Split(" ")[0] == "exit")
        break;
    Console.WriteLine($"{command}: command not found");
}