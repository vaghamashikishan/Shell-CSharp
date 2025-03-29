internal class ExecutableDirectories(string pathVariableContent)
{
    private readonly string[] sourcePaths = pathVariableContent
                                        .Split(':', ';', StringSplitOptions.RemoveEmptyEntries)
                                        .Where(Directory.Exists)
                                        .ToArray();

    public string? GetProgramPath(string programName)
    {
        foreach (var sourcePaths in sourcePaths)
        {
            var combinePath = Path.Combine(sourcePaths, programName);
            if (File.Exists(combinePath))
            {
                return combinePath;
            }
        }
        return null;
    }
}
