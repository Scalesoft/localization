namespace Localization.CoreLibrary.Exception
{
    public class PluralizedStringIntervalOverlapException : System.Exception
    {
        public PluralizedStringIntervalOverlapException()
        {
        }

        public PluralizedStringIntervalOverlapException(string message) : base(message)
        {
        }

        public PluralizedStringIntervalOverlapException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}