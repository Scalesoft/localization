using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scalesoft.Localization.Core.Resolver;

namespace Scalesoft.Localization.Core.Tests.Resolver
{
    [TestClass]
    public class FileNameBasedParentScopeResolverTest
    {
        [TestMethod]
        public void TranslateFormatTest()
        {
            var fileNameBasedParentScopeResolver = new FileNameBasedParentScopeResolver();

            Assert.IsNull(
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

            Assert.IsNull(
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
