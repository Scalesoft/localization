namespace Localization.Database.EFCore.Exception
{
    public class BuilderException : System.Exception
    {
        public BuilderException()
        {
        }

        public BuilderException(string message) : base(message)
        {
        }

        public BuilderException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}