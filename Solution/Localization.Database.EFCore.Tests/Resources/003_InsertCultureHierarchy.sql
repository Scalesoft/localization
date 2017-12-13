BEGIN TRAN

SET IDENTITY_INSERT [CultureHierarchy] ON 

--SELF
--INSERT INTO [CultureHierarchy] ([Id], [Culture], [ParentCulture], [LevelProperty]) 
	--VALUES (1, 3, 3, 0)	

INSERT INTO [CultureHierarchy] ([Id], [Culture], [ParentCulture], [LevelProperty]) 
	VALUES (1, 3, 2, 1)	

INSERT INTO [CultureHierarchy] ([Id], [Culture], [ParentCulture], [LevelProperty])
	VALUES (2, 3, 1, 2)	

--SELF
--INSERT INTO [CultureHierarchy] ([Id], [Culture], [ParentCulture], [LevelProperty])
	--VALUES (4, 1, 1, 0)

INSERT INTO [CultureHierarchy] ([Id], [Culture], [ParentCulture], [LevelProperty])
	VALUES (3, 2, 1, 1)

	INSERT INTO [CultureHierarchy] ([Id], [Culture], [ParentCulture], [LevelProperty])
    VALUES (4, 1, 1, 0);

INSERT INTO [DatabaseVersion]
	(DatabaseVersion)
VALUES
	('003' )

--ROLLBACK
COMMIT