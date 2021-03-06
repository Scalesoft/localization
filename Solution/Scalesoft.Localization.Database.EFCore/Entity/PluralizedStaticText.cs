﻿using System.Collections.Generic;
using Scalesoft.Localization.Database.Abstractions.Entity;

namespace Scalesoft.Localization.Database.EFCore.Entity
{
    public sealed class PluralizedStaticText : BaseText, IPluralizedStaticText
    {
        public ICollection<IntervalText> IntervalTexts { get; set; }
    }
}
