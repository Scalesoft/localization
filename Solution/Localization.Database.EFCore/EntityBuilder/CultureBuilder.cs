using System;
using System.Globalization;
using Localization.Database.Abstractions.Entity;
using Localization.Database.EFCore.Entity;
using Localization.Database.EFCore.Exception;

namespace Localization.Database.EFCore.EntityBuilder
{
    public class CultureBuilder
    {
        private readonly Culture m_culture;

        public CultureBuilder()
        {
            m_culture = new Culture();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Culture name. E.g. en-US or cs</param>
        /// <returns>this</returns>
        /// <exception cref="BuilderException">Thrown if Name is already set.</exception>
        public CultureBuilder Name(string name)
        {
            if (m_culture.Name != null)
            {
                throw new BuilderException("Name is already set");
            }

            CheckCultureValidity(name);
            m_culture.Name = name;

            return this;
        }

        public ICulture Build()
        {
            if (m_culture.Name == null)
            {
                throw new NullReferenceException("Name is not set");
            }

            return m_culture;
        }

        private void CheckCultureValidity(string cultureName)
        {
            try
            {
                new CultureInfo(cultureName);
            }
            catch (CultureNotFoundException e)
            {
                throw new BuilderException("Culture is not supported.");
            }
            
        }

    }
}