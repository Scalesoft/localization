using System;

namespace Localization.CoreLibrary.Pluralization
{
    public class PluralizationInterval
    {
        private readonly Int32 m_x;
        private readonly Int32 m_y;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">Begining of interval (inclusive)</param>
        /// <param name="y">End of interval (inclusive)</param>
        public PluralizationInterval(Int32 x, Int32 y)
        {
            if (x > y)
            {
                throw new ArgumentException("The x value should be less or equal than y.");
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
            if (obj == null)
            {
                throw new NullReferenceException("Comparing Pluralization interval cannot be null.");
            }
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