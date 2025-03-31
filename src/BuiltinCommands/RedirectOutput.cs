class RedirectOutput
{
    public int Execute(string[] input, int redirectionIndex, string[] args, bool isRedirectionAppend)
    {
        var output = string.Join(" ", input);
        var fileLocation = args[redirectionIndex];
        var fileInfo = new FileInfo(fileLocation);
        if (File.Exists(fileLocation) && fileInfo.Length > 0)
        {
            output = "\n" + output;
        }

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