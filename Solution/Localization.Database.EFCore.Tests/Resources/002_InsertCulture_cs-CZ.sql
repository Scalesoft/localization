BEGIN TRAN

SET IDENTITY_INSERT [Culture] ON 

INSERT INTO [Culture] ([Id], [Name]) 
	VALUES (1, 'cs-CZ')	

INSERT INTO [DatabaseVersion]
	(DatabaseVersion)
VALUES
	('002' )

--ROLLBACK
COMMIT