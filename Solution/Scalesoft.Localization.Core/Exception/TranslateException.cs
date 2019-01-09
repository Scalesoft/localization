namespace Scalesoft.Localization.Core.Exception
{
    public class TranslateException : System.Exception
    {
        public TranslateException()
        {
        }

        public TranslateException(string message) : base(message)
        {
        }

        public TranslateException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}