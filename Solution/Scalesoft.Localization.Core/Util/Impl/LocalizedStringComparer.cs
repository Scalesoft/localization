﻿using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Localization;

[assembly: InternalsVisibleTo("Scalesoft.Localization.Core.Tests")]

namespace Scalesoft.Localization.Core.Util.Impl
{
    internal class LocalizedStringComparer : IEqualityComparer<LocalizedString>
    {
        /// <summary>
        /// Determines whether the property "name" of specified objects are equal.
        /// </summary>
        /// <param name="objA">The first object of type LocalizedString to compare.</param>
        /// <param name="objB">The second object of type LocalizedString to compare.</param>
        /// <returns>true if the objects property "name" are considered equal; otherwise, false. If both objA and objB are null, the method returns true.</returns>
        public bool Equals(LocalizedString objA, LocalizedString objB)
        {
            if (objA == null || objB == null)
            {
                if (objA == null && objB == null)
                {
                    return true;
                }

                return false;
            }

            if (objA.Name == objB.Name)
            {
                return true;
            }

            return false;
        }

        public int GetHashCode(LocalizedString obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}