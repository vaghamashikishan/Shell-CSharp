internal interface IBuiltinCommand
{
    string Name { get; }
    int Execute(string[] args, bool isRedirectionExists = false, int redirectionIndex = -1, bool isOutputRedirection = false, bool isErrorRedirection = false, bool isRedirectionAppend = false);
}
