namespace Localization.CoreLibrary.Models
{
    public enum IfDefaultNotExistAction
    {
        /// <summary>
        /// Do nothing
        /// </summary>
        DoNothing,

        /// <summary>
        /// Create the default empty
        /// </summary>
        CreateEmpty,

        /// <summary>
        /// Save the default text same as the current text
        /// </summary>
        CreateTextCopy,
    }
}