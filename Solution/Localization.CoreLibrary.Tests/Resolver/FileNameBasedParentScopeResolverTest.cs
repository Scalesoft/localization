using Localization.CoreLibrary.Resolver;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Localization.CoreLibrary.Tests.Resolver
{
    [TestClass]
    public class FileNameBasedParentScopeResolverTest
    {
        [TestMethod]
        public void TranslateFormatTest()
        {
            var fileNameBasedParentScopeResolver = new FileNameBasedParentScopeResolver();

            Assert.AreSame(
                null,
                fileNameBasedParentScopeResolver.ResolveParentScope("Localization/apiresource/apiresource.cs.json")
            );
            Assert.AreEqual(
                "dynamicModule",
                fileNameBasedParentScopeResolver.ResolveParentScope("Localization/dynamicModule/generic/dynamicModule.generic.cs.json")
            );
            Assert.AreEqual(
                "dynamicModule",
                fileNameBasedParentScopeResolver.ResolveParentScope("Localization/dynamicModule/generic/dynamicModule.generic.en.json")
            );

            Assert.AreSame(
                null,
                fileNameBasedParentScopeResolver.ResolveParentScope("Localization/apiresource/apiresource_cs.json")
            );
            Assert.AreEqual(
                "dynamicModule",
                fileNameBasedParentScopeResolver.ResolveParentScope("Localization/dynamicModule/generic/dynamicModule_generic_cs.json")
            );
            Assert.AreEqual(
                "dynamicModule",
                fileNameBasedParentScopeResolver.ResolveParentScope("Localization/dynamicModule/generic/dynamicModule_generic_en.json")
            );
        }
    }
}
