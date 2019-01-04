namespace Localization.CoreLibrary.Resolver
{
    public interface IParentScopeResolver
    {
        string ResolveParentScope(string filePath);
    }
}
