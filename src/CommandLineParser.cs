class CommandLineParser
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
    private Action<char> parseMode;
    private IList<string> tokens;
    private IList<char> currentToken;
    private int index = 0;

    public IEnumerable<string> Parse(string userInput)
    {
        currentToken = [];
        tokens = [];
        parseMode = StartParsing;
        index = 0;

        while (index < userInput.Length)
        {
            parseMode(userInput[index]);
        }
        AddCurrentToken();
        return tokens;
    }

    private void StartParsing(char ch)
    {
        parseMode = char.IsWhiteSpace(ch) ? BreakBetweenTokens : SimpleToken;
    }

    private void SimpleToken(char ch)
    {
        if (char.IsWhiteSpace(ch))
        {
            parseMode = BreakBetweenTokens;
            return;
        }

        if (ch == '\'')
        {
            parseMode = SingleQuoteToken;
        }
        else if (ch == '"')
        {
            parseMode = DoubleQuoteToken;
        }
        else if (ch == '\\')
        {
            parseMode = NonQuotedBackSlash;
        }
        else
        {
            currentToken.Add(ch);
        }
        index++;
    }

    private void SingleQuoteToken(char ch)
    {
        if (ch == '\'')
        {
            parseMode = SimpleToken;
        }
        else
        {
            currentToken.Add(ch);
        }
        index++;
    }

    private void DoubleQuoteToken(char ch)
    {
        if (ch == '"')
        {
            parseMode = SimpleToken;
        }
        else if (ch == '\\')
        {
            parseMode = DoubleQuotedBackSlash;
        }
        else
        {
            currentToken.Add(ch);
        }
        index++;
    }

    private void DoubleQuotedBackSlash(char ch)
    {
        if (ch != '"' && ch != '\\' && ch != '$' && ch != '\n')
        {
            currentToken.Add('\\');
        }
        currentToken.Add(ch);
        index++;
        parseMode = DoubleQuoteToken;
    }

    private void NonQuotedBackSlash(char ch)
    {
        currentToken.Add(ch);
        index++;
        parseMode = SimpleToken;
    }

    private void BreakBetweenTokens(char ch)
    {
        AddCurrentToken();
        if (char.IsWhiteSpace(ch))
        {
            index++;
            return;
        }
        parseMode = SimpleToken;
    }

    private void AddCurrentToken()
    {
        if (currentToken.Count == 0)
        {
            return;
        }
        tokens.Add(currentToken.ToString() ?? "");
        currentToken = [];
    }
}