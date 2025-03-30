class RedirectOutput
{
    public int Execute(string[] input, int redirectionIndex, string[] args, bool isRedirectionAppend)
    {
        System.Console.WriteLine("redirection ma", isRedirectionAppend);
        var output = string.Join(" ", input) + "\n";
        var fileLocation = args[redirectionIndex];
        if (isRedirectionAppend)
        {
            System.Console.WriteLine("Appending");
            File.AppendAllText(fileLocation, output);
        }
        else
        {
            File.WriteAllText(fileLocation, output);

        }
        return 0;
    }
}