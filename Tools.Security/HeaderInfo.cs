namespace Tools.Security
{
    public record HeaderInfo(string HeaderName, Func<string, bool>? Validator = null);
}