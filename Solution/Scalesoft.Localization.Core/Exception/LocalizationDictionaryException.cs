namespace Scalesoft.Localization.Core.Exception
{
    public class LocalizationDictionaryException : System.Exception
    {
        public LocalizationDictionaryException()
        {
        }

        public LocalizationDictionaryException(string message) : base(message)
        {
        }

        public LocalizationDictionaryException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}