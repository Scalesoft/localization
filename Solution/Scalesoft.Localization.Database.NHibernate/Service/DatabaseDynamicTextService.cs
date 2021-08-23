using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Scalesoft.Localization.Database.Abstractions.Entity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Scalesoft.Localization.Core.Configuration;
using Scalesoft.Localization.Core.Database;
using Scalesoft.Localization.Core.Model;
using Scalesoft.Localization.Database.NHibernate.UnitOfWork;

namespace Scalesoft.Localization.Database.NHibernate.Service
{
    public class DatabaseDynamicTextService : DatabaseServiceBase, IDatabaseDynamicTextService
    {
        private readonly StaticTextUoW m_staticTextUoW;

        public DatabaseDynamicTextService(
            StaticTextUoW staticTextUoW,
            LocalizationConfiguration configuration,
            CultureUoW cultureUoW,
            DictionaryScopeUoW dictionaryScopeUoW,
            ILogger<DatabaseDynamicTextService> logger,
            IMemoryCache memoryCache
        ) : base(configuration, cultureUoW, dictionaryScopeUoW, logger, memoryCache)
        {
            m_staticTextUoW = staticTextUoW;
        }

        public DynamicText GetDynamicText(string name, string scope, CultureInfo cultureInfo)
        {
            if (string.IsNullOrEmpty(scope))
            {
                scope = m_configuration.DefaultScope;
            }

            var culture = GetCachedCultureByNameOrGetDefault(cultureInfo.Name);
            var dictionaryScope = GetCachedDictionaryScope(scope);

            var staticText = m_staticTextUoW.GetByNameAndCultureAndScope(
                name, culture.Name, dictionaryScope.Name
            );

            if (staticText == null)
            {
                return null;
            }

            return new DynamicText
            {
                FallBack = staticText.Culture.Name != cultureInfo.Name,
                Culture = staticText.Culture.Name,
                DictionaryScope = staticText.DictionaryScope.Name,
                Format = staticText.Format,
                ModificationTime = staticText.ModificationTime,
                ModificationUser = staticText.ModificationUser,
                Name = staticText.Name,
                Text = staticText.Text,
            };
        }

        public IList<DynamicText> GetAllDynamicText(string name, string scope)
        {
            if (string.IsNullOrEmpty(scope))
            {
                scope = m_configuration.DefaultScope;
            }

            var dictionaryScope = GetCachedDictionaryScope(scope);

            var staticTexts = m_staticTextUoW.FindByNameAndScope(
                name, dictionaryScope.Name
            );

            return staticTexts
                .Select(staticText =>
                    new DynamicText
                    {
                        FallBack = false,
                        Culture = staticText.Culture.Name,
                        DictionaryScope = staticText.DictionaryScope.Name,
                        Format = staticText.Format,
                        ModificationTime = staticText.ModificationTime,
                        ModificationUser = staticText.ModificationUser,
                        Name = staticText.Name,
                        Text = staticText.Text,
                    }
                )
                .ToList();
        }

        public DynamicText SaveDynamicText(
            DynamicText dynamicText,
            IfDefaultNotExistAction actionForDefaultCulture = IfDefaultNotExistAction.DoNothing
        )
        {
            var dictionaryScope = GetDictionaryScope(dynamicText.DictionaryScope);
            if (dictionaryScope.Name != dynamicText.DictionaryScope)
            {
                m_dictionaryScopeUoW.AddScope(dynamicText.DictionaryScope);
                dictionaryScope = GetDictionaryScope(dynamicText.DictionaryScope);
            }

            var culture = GetCultureByNameOrGetDefault(dynamicText.Culture);

            if (culture.Name != dynamicText.Culture)
            {
                throw new ArgumentException($"Unknown culture {dynamicText.Culture}");
            }

            var staticText = m_staticTextUoW.GetByNameAndCultureAndScope(
                dynamicText.Name, culture.Name, dictionaryScope.Name
            );

            if (staticText == null)
            {
                m_staticTextUoW.AddStaticText(
                    dynamicText.Name,
                    dynamicText.Format,
                    dynamicText.Text,
                    culture.Name,
                    dictionaryScope.Name,
                    dynamicText.ModificationUser,
                    DateTime.UtcNow
                );
            }
            else
            {
                m_staticTextUoW.UpdateStaticText(
                    dynamicText.Name,
                    culture.Name,
                    dictionaryScope.Name,
                    dynamicText.Format,
                    dynamicText.Text,
                    dynamicText.ModificationUser,
                    DateTime.UtcNow
                );
            }

            ExecuteDefaultCultureAction(actionForDefaultCulture, dynamicText, culture, dictionaryScope);

            return dynamicText;
        }

        public void DeleteDynamicText(string name, string scope, CultureInfo cultureInfo)
        {
            if (string.IsNullOrEmpty(scope))
            {
                scope = m_configuration.DefaultScope;
            }

            var culture = GetCultureByNameOrGetDefault(cultureInfo.Name);

            if (culture.Name != cultureInfo.Name)
            {
                throw new ArgumentException($"Unknown culture {cultureInfo.Name}");
            }

            m_staticTextUoW.Delete(name, culture.Name, scope);
        }

        public void DeleteAllDynamicText(string name, string scope)
        {
            if (string.IsNullOrEmpty(scope))
            {
                scope = m_configuration.DefaultScope;
            }

            m_staticTextUoW.DeleteAll(name, scope);
        }

        /// <summary>
        /// Executes an action on default culture text if it does not exist. Actions include - nothing, create empty string, copy current culture text
        /// </summary>
        /// <param name="actionForDefaultCulture">Specific action</param>
        /// <param name="dynamicText">Current dynamic text entity</param>
        /// <param name="currentCulture">Current culture entity</param>
        /// <param name="dictionaryScope">Current dictionary scope entity</param>
        private void ExecuteDefaultCultureAction(
            IfDefaultNotExistAction actionForDefaultCulture,
            DynamicText dynamicText,
            ICulture currentCulture,
            IDictionaryScope dictionaryScope
        )
        {
            if (actionForDefaultCulture == IfDefaultNotExistAction.DoNothing)
            {
                return;
            }

            var defaultCulture = GetDefaultCulture();
            if (currentCulture.Equals(defaultCulture))
            {
                return;
            }

            var defaultText = m_staticTextUoW.GetByNameAndCultureAndScope(
                dynamicText.Name, defaultCulture.Name, dictionaryScope.Name
            );

            if (defaultText != null)
            {
                return;
            }

            string text;

            switch (actionForDefaultCulture)
            {
                case IfDefaultNotExistAction.DoNothing:
                    return;

                case IfDefaultNotExistAction.CreateEmpty:
                    text = string.Empty;
                    break;
                case IfDefaultNotExistAction.CreateTextCopy:
                    text = dynamicText.Text;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(actionForDefaultCulture),
                        actionForDefaultCulture,
                        "Invalid default language save action"
                    );
            }

            m_staticTextUoW.AddStaticText(
                dynamicText.Name,
                dynamicText.Format,
                text,
                defaultCulture.Name,
                dictionaryScope.Name,
                dynamicText.ModificationUser,
                DateTime.UtcNow
            );
        }
    }
}
