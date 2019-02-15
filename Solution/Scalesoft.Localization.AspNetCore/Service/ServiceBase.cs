﻿using System.Globalization;
using Scalesoft.Localization.AspNetCore.Manager;

namespace Scalesoft.Localization.AspNetCore.Service
{
    public abstract class ServiceBase
    {
        protected readonly IRequestCultureManager m_requestCultureManager;

        protected ServiceBase(
            IRequestCultureManager requestCultureManager
        )
        {
            m_requestCultureManager = requestCultureManager;
        }

        protected CultureInfo GetRequestCulture(CultureInfo defaultCulture)
        {
            return m_requestCultureManager.ResolveRequestCulture(defaultCulture);
        }
    }
}
