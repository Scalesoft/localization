using System;
using System.Globalization;
using Microsoft.Extensions.Logging;
using Scalesoft.Localization.Database.EFCore.Entity;
using Scalesoft.Localization.Database.EFCore.EntityBuilder.Common;
using Scalesoft.Localization.Database.EFCore.Exception;
using Scalesoft.Localization.Database.EFCore.Logging;

namespace Scalesoft.Localization.Database.EFCore.EntityBuilder
{
    public class CultureBuilder
    {
        private static readonly ILogger Logger = LogProvider.GetCurrentClassLogger();

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
            BuilderGuard.ArgumentAlreadySet(nameof(name), m_culture.Name, Logger);

            CheckCultureValidity(name);
            m_culture.Name = name;

            return this;
        }

        public Culture Build()
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
            catch (CultureNotFoundException)
            {
                throw new BuilderException("Culture is not supported.");
            }
            
        }

    }
}