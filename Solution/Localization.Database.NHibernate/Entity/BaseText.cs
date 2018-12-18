using System;
using Localization.Database.Abstractions.Entity;

namespace Localization.Database.NHibernate.Entity
{
    public abstract class BaseText : IBaseText
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual short Format { get; set; }

        public virtual DateTime ModificationTime { get; set; }

        public virtual string ModificationUser { get; set; }

        public virtual IDictionaryScope DictionaryScope { get; set; }

        public virtual int CultureId { get; set; }

        public virtual int DictionaryScopeId { get; set; }
        public virtual string Text { get; set; }

        public virtual ICulture Culture { get; set; }
    }
}