namespace Scalesoft.Localization.Core.Exception
{
    public class DictionaryLoadException : LocalizationLibraryException
    {
        public DictionaryLoadException()
        {
        }

        public DictionaryLoadException(string message) : base(message)
        {
        }

        public DictionaryLoadException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}