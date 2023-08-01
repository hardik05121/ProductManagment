/*
   Thursday, July 27, 20236:59:16 PM
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
ALTER TABLE dbo.ErrorLogs ADD CONSTRAINT
	PK_ErrorLogs PRIMARY KEY CLUSTERED 
	(
	ErrorId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ErrorLogs SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.ErrorLogs', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.ErrorLogs', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.ErrorLogs', 'Object', 'CONTROL') as Contr_Per 