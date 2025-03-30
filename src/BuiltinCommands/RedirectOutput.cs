class RedirectOutput
{
    public int Execute(string[] input, int redirectionIndex, string[] args)
    {
        var output = string.Join(" ", input);
        var fileLocation = args[redirectionIndex];
        File.WriteAllText(fileLocation, output);
        return 0;
    }
}