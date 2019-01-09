namespace Scalesoft.Localization.Core.Util
{
    /// <summary>
    /// Types of translation sources
    /// </summary>
    public enum LocTranslationSource
    {
        //File type e.g. json, xml, csv formats
        File = 0,
        Database = 1,

        //Automatic - finds first match from File and Database. The first to check is specified in localizationsettings.json settings file.
        Auto = 2,
    }
}