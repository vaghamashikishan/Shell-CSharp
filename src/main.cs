bool run = true;
while (run)
{
    Console.Write("$ ");
    var command = Console.ReadLine();
    Console.WriteLine($"{command}: command not found");
}