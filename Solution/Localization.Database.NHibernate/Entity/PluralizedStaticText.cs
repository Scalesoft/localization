using System.Collections.Generic;
using Localization.Database.Abstractions.Entity;

namespace Localization.Database.NHibernate.Entity
{
    public class PluralizedStaticText : BaseText, IPluralizedStaticText
    {
        public virtual ICollection<IntervalText> IntervalTexts { get; set; }
    }
}