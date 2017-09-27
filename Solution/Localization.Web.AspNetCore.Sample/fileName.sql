IF OBJECT_ID(N'__EFMigrationsHistory') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Culture] (
    [Id] int NOT NULL IDENTITY,
    [Name] varchar(5) NOT NULL,
    CONSTRAINT [PK_Culture(Id)] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [DictionaryScope] (
    [Id] int NOT NULL IDENTITY,
    [Name] varchar(255) NOT NULL,
    CONSTRAINT [PK_DictionaryScope(Id)] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [CultureHierarchy] (
    [Id] int NOT NULL IDENTITY,
    [Culture] int NOT NULL,
    [Level] tinyint NOT NULL,
    [ParentCulture] int NOT NULL,
    CONSTRAINT [PK_CultureHierarchy(Id)] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CultureHierarchy(Culture)_Culture(Id)] FOREIGN KEY ([Culture]) REFERENCES [Culture] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_CultureHierarchy(ParentCulture)_Culture(Id)] FOREIGN KEY ([ParentCulture]) REFERENCES [Culture] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [BaseText] (
    [Id] int NOT NULL IDENTITY,
    [Culture] int NOT NULL,
    [DictionaryScope] int NOT NULL,
    [Discriminator] nvarchar(max) NOT NULL,
    [Format] smallint NOT NULL,
    [ModificationTime] datetime NOT NULL,
    [ModificationUser] varchar(255) NOT NULL,
    [Name] varchar(255) NOT NULL,
    [Text] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_BaseText(Id)] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_BaseText(Culture)_Culture(Id)] FOREIGN KEY ([Culture]) REFERENCES [Culture] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_BaseText_DictionaryScope_DictionaryScope] FOREIGN KEY ([DictionaryScope]) REFERENCES [DictionaryScope] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [IntervalText] (
    [Id] int NOT NULL IDENTITY,
    [IntervalEnd] int NOT NULL,
    [IntervalStart] int NOT NULL,
    [PluralizedStaticText] int NOT NULL,
    [Text] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_IntervalText(Id)] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_IntervalText(PluralizedStaticText)_PluralizedStaticText(Id)] FOREIGN KEY ([PluralizedStaticText]) REFERENCES [BaseText] ([Id]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_BaseText_Culture] ON [BaseText] ([Culture]);

GO

CREATE INDEX [IX_BaseText_DictionaryScope] ON [BaseText] ([DictionaryScope]);

GO

CREATE INDEX [IX_CultureHierarchy_ParentCulture] ON [CultureHierarchy] ([ParentCulture]);

GO

CREATE UNIQUE INDEX [IX_CultureHierarchy_Culture_ParentCulture] ON [CultureHierarchy] ([Culture], [ParentCulture]);

GO

CREATE INDEX [IX_IntervalText_PluralizedStaticText] ON [IntervalText] ([PluralizedStaticText]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20170829125213_Inheritance', N'1.1.2');

GO

