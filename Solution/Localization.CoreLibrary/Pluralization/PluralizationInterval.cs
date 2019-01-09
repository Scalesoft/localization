using System;
using Microsoft.Extensions.Logging;
using Scalesoft.Localization.Core.Common;
using Scalesoft.Localization.Core.Logging;

namespace Scalesoft.Localization.Core.Pluralization
{
    public class PluralizationInterval
    {
        private readonly ILogger m_logger;

        private readonly int m_x;
        private readonly int m_y;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">Begining of interval (inclusive)</param>
        /// <param name="y">End of interval (inclusive)</param>
        public PluralizationInterval(int x, int y, ILogger logger = null)
        {
            if (x > y)
            {
                var intervalErrorMsg = "The x value should be less or equal than y.";

                if (logger != null && logger.IsErrorEnabled())
                {
                    logger.LogError(intervalErrorMsg);
                }

                throw new ArgumentException(intervalErrorMsg);
            }

            m_x = x;
            m_y = y;
            m_logger = logger;
        }

        /// <summary>
        /// Determines whether the union of specified object with this is not empty.
        /// </summary>
        /// <param name="obj">Interval to test with this object.</param>
        /// <returns>true if union of intevals is not empty.</returns>
        /// <exception cref="NullReferenceException">Is thrown if obj is null.</exception>
        public bool IsOverlaping(PluralizationInterval obj)
        {
            Guard.ArgumentNotNull(nameof(obj), obj, m_logger);

            return m_x <= obj.m_y && obj.m_x <= m_y;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            var pluralizationInterval = (PluralizationInterval) obj;

            return IsOverlaping(pluralizationInterval);
        }

        public override int GetHashCode()
        {
            return m_x.GetHashCode() ^ m_y.GetHashCode();
        }
    }
}