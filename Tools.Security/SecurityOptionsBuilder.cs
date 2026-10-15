using System.Text.RegularExpressions;

namespace Tools.Security
{
    public class SecurityOptionsBuilder
    {
        internal const string HeaderRegexPattern = "^[!#$%&'*+\\-.^_`|~0-9A-Za-z]+$";

        public string PolicyName { get; set; } = "Corsican Police Department";
        internal IList<string> Origins { get; } = new List<string>();
        internal IList<string> Methods { get; } = new List<string>();
        internal IList<HeaderInfo> Headers { get; } = new List<HeaderInfo>();
        public bool UseCors { get; set; } = false;

        public void AddOrigins(params string[] origins)
        {
            if (!UseCors)
                throw new InvalidOperationException("The cors aren't enabled");

            foreach (string origin in origins)
            {
                string currentOrigin = origin;

                if (currentOrigin.EndsWith("/"))
                    currentOrigin = currentOrigin.TrimEnd('/');

                Uri uri = new Uri(currentOrigin); //<-- il n'est qu'à des fins de validations et récupération du Scheme

                if(uri.Scheme is "http" or "https")
                {
                    Origins.Add(currentOrigin);
                }
            }
        }

        public void SetMethods(Verbs verbs)
        {
            if (!UseCors)
                throw new InvalidOperationException("The cors aren't enabled");

            foreach (Verbs verb in Enum.GetValues<Verbs>())
            {
                if (verb is not Verbs.ALL && verbs.HasFlag(verb))
                    Methods.Add(verb.ToString());
            }
        }

        public void AddHeaders(params HeaderInfo[] headers)
        {
            foreach(HeaderInfo header in headers)
            {
                if(!Regex.IsMatch(header.HeaderName, HeaderRegexPattern))
                {
                    throw new InvalidOperationException($"The header '{header}' is invalid!");
                }

                Headers.Add(header);
            }
        }
    }
}
