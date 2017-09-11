namespace Localization.CoreLibrary.Exception
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