using System.IO;
using System.Linq;

namespace Scalesoft.Localization.Core.Resolver
{
    public class FileNameBasedParentScopeResolver : IParentScopeResolver
    {
        private static readonly char[] Delimiters =
        {
            // order matters
            '.',
            '_',
        };

        public string ResolveParentScope(string filePath)
        {
            var fileName = Path.GetFileName(
                Path.ChangeExtension(filePath, "")
            );

            fileName = fileName?.TrimEnd('.');

            if (fileName == null)
            {
                return null;
            }

            foreach (var delimiter in Delimiters)
            {
                if (fileName.Contains(delimiter.ToString()))
                {
                    var fileNameArr = fileName.Split(delimiter);

                    if (fileNameArr.Length > 2)
                    {
                        return string.Join(delimiter.ToString(), fileNameArr.Take(fileNameArr.Length - 2));
                    }

                    // Try to resolve only by first known delimiter
                    break;
                }
            }

            return null;
        }
    }
}
