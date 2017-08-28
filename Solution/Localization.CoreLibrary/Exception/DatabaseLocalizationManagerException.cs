namespace Localization.CoreLibrary.Exception
{
    public class DatabaseLocalizationManagerException : LocalizationLibraryException
    {
        public DatabaseLocalizationManagerException()
        {
        }

        public DatabaseLocalizationManagerException(string message) : base(message)
        {
        }

        public DatabaseLocalizationManagerException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}