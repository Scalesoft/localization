namespace Localization.CoreLibrary.Util
{
    /// <summary>
    /// Types of translation sources
    /// </summary>
    public enum LocTranslationSource
    {
        File, //File type e.g. json, xml, csv formats
        Database, //Database type
        Auto //Automatic - finds first match from File and Database. The first to check is specified in localizationsettings.json settings file.
    }
}