namespace Scalesoft.Localization.Core.Exception
{
    public class DictionaryFormatException : LocalizationLibraryException
    {
        public DictionaryFormatException()
        {
        }

        public DictionaryFormatException(string message) : base(message)
        {
        }

        public DictionaryFormatException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}