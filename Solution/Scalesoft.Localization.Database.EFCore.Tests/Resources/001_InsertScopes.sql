BEGIN TRAN

SET IDENTITY_INSERT [DictionaryScope] ON 

INSERT INTO [DictionaryScope] ([Id], [Name]) 
	VALUES (1, 'global')

INSERT INTO [DictionaryScope] ([Id], [Name]) 
	VALUES (2, 'home')

INSERT INTO [DictionaryScope] ([Id], [Name]) 
	VALUES (3, 'dict')

INSERT INTO [DictionaryScope] ([Id], [Name]) 
	VALUES (4, 'edition')

INSERT INTO [DictionaryScope] ([Id], [Name]) 
	VALUES (5, 'textbank')

INSERT INTO [DictionaryScope] ([Id], [Name]) 
	VALUES (6, 'grammar')

INSERT INTO [DictionaryScope] ([Id], [Name]) 
	VALUES (7, 'professional')

INSERT INTO [DictionaryScope] ([Id], [Name]) 
	VALUES (8, 'bibliographies')

INSERT INTO [DictionaryScope] ([Id], [Name]) 
	VALUES (9, 'cardfiles')

INSERT INTO [DictionaryScope] ([Id], [Name]) 
	VALUES (10, 'audio')

	INSERT INTO [DictionaryScope] ([Id], [Name]) 
	VALUES (11, 'tools')

INSERT INTO [DatabaseVersion]
	(DatabaseVersion)
VALUES
	('001' )

--ROLLBACK
COMMIT