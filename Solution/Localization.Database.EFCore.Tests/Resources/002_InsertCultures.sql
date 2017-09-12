BEGIN TRAN

SET IDENTITY_INSERT [Culture] ON 

INSERT INTO [Culture] ([Id], [Name]) 
	VALUES (1, 'cs-CZ')	

INSERT INTO [Culture] ([Id], [Name]) 
	VALUES (2, 'en')	

INSERT INTO [Culture] ([Id], [Name]) 
	VALUES (3, 'en-US')

INSERT INTO [DatabaseVersion]
	(DatabaseVersion)
VALUES
	('002' )

--ROLLBACK
COMMIT