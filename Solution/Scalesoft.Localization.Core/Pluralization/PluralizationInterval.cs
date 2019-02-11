using System;
using Microsoft.Extensions.Logging;
using Scalesoft.Localization.Core.Common;
using Scalesoft.Localization.Core.Logging;

namespace Scalesoft.Localization.Core.Pluralization
{
    public class PluralizationInterval // TODO should implement IEquatable?
    {
        private readonly ILogger m_logger;

        public readonly int Start;
        public readonly int End;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="start">Beginning of interval (inclusive)</param>
        /// <param name="end">End of interval (inclusive)</param>
        public PluralizationInterval(int start, int end, ILogger logger = null)
        {
            if (start > end)
            {
                var intervalErrorMsg = "The start value should be less or equal than end.";

                if (logger != null && logger.IsErrorEnabled())
                {
                    logger.LogError(intervalErrorMsg);
                }

                throw new ArgumentException(intervalErrorMsg);
            }

            Start = start;
            End = end;
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

            return Start <= obj.End && obj.Start <= End;
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
            return Start.GetHashCode() ^ End.GetHashCode();
        }
    }
}