using System;
using Microsoft.Extensions.Logging;
using Scalesoft.Localization.Core.Logging;

namespace Scalesoft.Localization.Core.Pluralization
{
    public class PluralizationInterval
    {
        public int Start { get; set; }
        public int End { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="start">Beginning of interval (inclusive)</param>
        /// <param name="end">End of interval (inclusive)</param>
        /// <param name="logger">Logger instance</param>
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
        }

        /// <summary>
        /// Determines whether tested value is in the interval.
        /// </summary>
        /// /// <param name="value">Value to be tested</param>
        /// <returns>true if value is in the interval.</returns>
        public bool IsInInterval(int value)
        {
            return Start <= value && value <= End;
        }
    }
}