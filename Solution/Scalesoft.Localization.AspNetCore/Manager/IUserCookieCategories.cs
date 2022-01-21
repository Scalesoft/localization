namespace Scalesoft.Localization.AspNetCore.Manager
{
    public interface IUserCookieCategories
    {
        bool EssentialAllowed { get; set; }
        bool PreferentialAllowed { get; set; }
    }
}
