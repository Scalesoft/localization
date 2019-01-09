BEGIN TRAN

SET IDENTITY_INSERT [Culture] ON

INSERT INTO [Culture] ([Id], [Name])
	VALUES (1, 'cs-CZ');

SET IDENTITY_INSERT [Culture] OFF

SET IDENTITY_INSERT [CultureHierarchy] ON

INSERT INTO [CultureHierarchy] ([Id], [Culture], [ParentCulture], [LevelProperty])
    VALUES (1, 1, 1, 0);

SET IDENTITY_INSERT [CultureHierarchy] OFF

INSERT INTO [DatabaseVersion]
	(DatabaseVersion)
VALUES
	('002' )

--ROLLBACK
COMMIT
