BEGIN TRAN

SET IDENTITY_INSERT [Culture] ON 

INSERT INTO [Culture] ([Id], [Name]) 
	VALUES (1, 'cs-CZ');

INSERT INTO [CultureHierarchy] ([Id], [Culture], [ParentCulture], [LevelProperty])
    VALUES (1, 1, 1, 0);



INSERT INTO [DatabaseVersion]
	(DatabaseVersion)
VALUES
	('002' )

--ROLLBACK
COMMIT