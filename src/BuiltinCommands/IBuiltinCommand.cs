internal interface IBuiltinCommand
{
    string Name { get; }
    int Execute(string[] args, bool isRedirectionExists = false, int redirectionIndex = -1);
}
