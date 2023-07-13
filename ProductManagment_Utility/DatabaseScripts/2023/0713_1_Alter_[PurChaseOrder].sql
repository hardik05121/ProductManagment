/*
   Thursday, July 13, 202311:58:57 AM
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
ALTER TABLE dbo.PurChaseOrder ADD
	GrandTotal float(53) NULL
GO
ALTER TABLE dbo.PurChaseOrder SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.PurChaseOrder', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.PurChaseOrder', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.PurChaseOrder', 'Object', 'CONTROL') as Contr_Per 