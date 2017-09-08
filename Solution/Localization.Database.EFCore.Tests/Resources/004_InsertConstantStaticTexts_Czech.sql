BEGIN TRAN

SET IDENTITY_INSERT [dbo].[BaseText] ON

--DictionaryScope = bibliographies
INSERT INTO [dbo].[BaseText] ([Id], [Culture], [DictionaryScope], [Name], [Format], [Discriminator], [Text], [ModificationUser]) 
	VALUES (24, 1, 2, 'prvni-den-tydnu', 1, N'ConstantStaticText', N'Pondělí'
, N'Admin')

INSERT INTO [dbo].[DatabaseVersion]
	(DatabaseVersion)
VALUES
	('004' )

--ROLLBACK
COMMIT