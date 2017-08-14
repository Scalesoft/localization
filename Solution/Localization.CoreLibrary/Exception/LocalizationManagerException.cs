namespace Localization.CoreLibrary.Exception
{
    public class LocalizationManagerException : LocalizationLibraryException
    {
        public LocalizationManagerException()
        {
        }

        public LocalizationManagerException(string message) : base(message)
        {
        }

        public LocalizationManagerException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}