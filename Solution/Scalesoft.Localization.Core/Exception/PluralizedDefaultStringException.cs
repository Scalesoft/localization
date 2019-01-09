namespace Scalesoft.Localization.Core.Exception
{
    public class PluralizedDefaultStringException : System.Exception
    {
        public PluralizedDefaultStringException()
        {
        }

        public PluralizedDefaultStringException(string message) : base(message)
        {
        }

        public PluralizedDefaultStringException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}