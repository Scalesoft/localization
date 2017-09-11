using System;
using Localization.CoreLibrary.Common;
using Localization.CoreLibrary.Logging;
using Localization.CoreLibrary.Util.Impl;
using Microsoft.Extensions.Logging;

namespace Localization.CoreLibrary.Pluralization
{
    public class PluralizationInterval
    {
        private static readonly ILogger Logger = LogProvider.GetCurrentClassLogger();

        private readonly int m_x;
        private readonly int m_y;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">Begining of interval (inclusive)</param>
        /// <param name="y">End of interval (inclusive)</param>
        public PluralizationInterval(int x, int y)
        {
            if (x > y)
            {
                string intervalErrorMsg = "The x value should be less or equal than y.";

                if (Logger.IsErrorEnabled())
                {
                    Logger.LogError(intervalErrorMsg);
                }

                throw new ArgumentException(intervalErrorMsg);
            }

            m_x = x;
            m_y = y;
        }

        /// <summary>
        /// Determines whether the union of specified object with this is not empty.
        /// </summary>
        /// <param name="obj">Interval to test with this object.</param>
        /// <returns>true if union of intevals is not empty.</returns>
        /// <exception cref="NullReferenceException">Is thrown if obj is null.</exception>
        public bool IsOverlaping(PluralizationInterval obj)
        {
            Guard.ArgumentNull(nameof(obj), obj, Logger);

            return this.m_x <= obj.m_y && obj.m_x <= this.m_y;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (this.GetType() != obj.GetType())
            {
                return false;
            }
            PluralizationInterval pluralizationInterval = (PluralizationInterval) obj;

            return IsOverlaping(pluralizationInterval);
        }

        public override int GetHashCode()
        {
            return m_x.GetHashCode() ^ m_y.GetHashCode();
        }
    }
}