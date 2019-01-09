using Microsoft.EntityFrameworkCore;
using Scalesoft.Localization.Database.EFCore.Entity;

namespace Scalesoft.Localization.Database.EFCore.Data.Impl
{
    public class StaticTextsContext : DbContext, IDatabaseStaticTextContext
    {
        public StaticTextsContext(DbContextOptions options) : base(options)
        {
            //Should be empty
        }

        public DbSet<Culture> Culture { get; set; }
        public DbSet<CultureHierarchy> CultureHierarchy { get; set; }
        public DbSet<DictionaryScope> DictionaryScope { get; set; }
        public DbSet<BaseText> BaseText { get; set; }
        public DbSet<ConstantStaticText> ConstantStaticText { get; set; }
        public DbSet<IntervalText> IntervalText { get; set; }
        public DbSet<PluralizedStaticText> PluralizedStaticText { get; set; }
        public DbSet<StaticText> StaticText { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Culture>()
                .ToTable("Culture")
                .HasKey(culture => culture.Id).HasName("PK_Culture(Id)");

            modelBuilder.Entity<CultureHierarchy>()
                .ToTable("CultureHierarchy")
                .HasKey(cultureHierarchy => cultureHierarchy.Id).HasName("PK_CultureHierarchy(Id)");

            modelBuilder.Entity<CultureHierarchy>()
                .HasIndex(cultureHierarchy => new {cultureHierarchy.CultureId, cultureHierarchy.ParentCultureId})
                .IsUnique(true);

            modelBuilder.Entity<CultureHierarchy>()
                .Property(p => p.CultureId).HasColumnName("Culture");

            modelBuilder.Entity<CultureHierarchy>()
                .Property(p => p.ParentCultureId).HasColumnName("ParentCulture");

            modelBuilder.Entity<DictionaryScope>()
                .ToTable("DictionaryScope")
                .HasKey(dictionaryScope => dictionaryScope.Id).HasName("PK_DictionaryScope(Id)");

            modelBuilder.Entity<BaseText>()
                .Property(p => p.CultureId).HasColumnName("Culture");

            modelBuilder.Entity<BaseText>()
                .Property(p => p.DictionaryScopeId).HasColumnName("DictionaryScope");

            modelBuilder.Entity<BaseText>()
                .ToTable("BaseText")
                .HasKey(baseText => baseText.Id).HasName("PK_BaseText(Id)");

            modelBuilder.Entity<ConstantStaticText>()
                .ToTable("ConstantStaticText");

            modelBuilder.Entity<IntervalText>()
                .ToTable("IntervalText")
                .HasKey(intervalText => intervalText.Id).HasName("PK_IntervalText(Id)");

            modelBuilder.Entity<IntervalText>()
                .Property(p => p.PluralizedStaticTextId).HasColumnName("PluralizedStaticText");

            modelBuilder.Entity<IntervalText>()
                .HasOne(t => t.PluralizedStaticText)
                .WithMany(t => t.IntervalTexts)
                .OnDelete(DeleteBehavior.Restrict)
                .HasForeignKey(f => f.PluralizedStaticTextId)
                .HasConstraintName("FK_IntervalText(PluralizedStaticText)_PluralizedStaticText(Id)");

            modelBuilder.Entity<PluralizedStaticText>()
                .ToTable("PluralizedStaticText");

            modelBuilder.Entity<StaticText>()
                .ToTable("StaticText");
        }
    }
}