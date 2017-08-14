namespace Localization.CoreLibrary.Exception
{
    public class LibraryConfigurationException : System.Exception
    {
        public LibraryConfigurationException()
        {
        }

        public LibraryConfigurationException(string message) : base(message)
        {
        }

        public LibraryConfigurationException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}