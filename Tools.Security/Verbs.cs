namespace Tools.Security
{
    [Flags]
    public enum Verbs
    {
        GET = 1,
        POST = 2,
        PUT = 4,
        PATCH = 8,
        DELETE = 16,
        ALL = 31
    }
}