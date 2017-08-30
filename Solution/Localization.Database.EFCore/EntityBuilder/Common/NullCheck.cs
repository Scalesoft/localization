using System;

namespace Localization.Database.EFCore.EntityBuilder.Common
{
    public static class NullCheck
    {
        public static void Check(string obj)
        {
            if (obj == null)
            {
                throw new NullReferenceException(nameof(obj));
            }
        }

        public static void Check(object obj)
        {
            if (obj == null)
            {
                throw new NullReferenceException(nameof(obj));
            }
        }
    }
}