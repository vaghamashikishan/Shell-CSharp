class RedirectOutput
{
    public int Execute(string[] input, int redirectionIndex, string[] args, bool isRedirectionAppend)
    {
        var output = string.Join(" ", input) + "\n";
        var fileLocation = args[redirectionIndex];
        if (isRedirectionAppend)
        {
            File.AppendAllText(fileLocation, output);
        }
        else
        {
            File.WriteAllText(fileLocation, output);
        }
        return 0;
    }
}