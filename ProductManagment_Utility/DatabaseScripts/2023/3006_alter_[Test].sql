/*
   Friday, June 30, 20235:42:26 PM
   User: 
   Server: HARDIK\SQLEXPRESS
   Database: ProductManagment
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Test
	(
	Id int NULL,
	Name nchar(10) NOT NULL,
	IsActive nchar(10) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Test SET (LOCK_ESCALATION = TABLE)
GO
IF EXISTS(SELECT * FROM dbo.Test)
	 EXEC('INSERT INTO dbo.Tmp_Test (Id, Name, IsActive)
		SELECT Id, Name, IsActive FROM dbo.Test WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.Test
GO
EXECUTE sp_rename N'dbo.Tmp_Test', N'Test', 'OBJECT' 
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Test', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Test', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Test', 'Object', 'CONTROL') as Contr_Per 