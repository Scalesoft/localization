using System.Collections.Generic;
using Localization.Database.Abstractions.Entity;

namespace Localization.Database.NHibernate.Entity
{
    public class PluralizedStaticTextEntity : BaseTextEntity, IPluralizedStaticText
    {
        public virtual ICollection<IntervalTextEntity> IntervalTexts { get; set; }
    }
}
