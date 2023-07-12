/*
   Wednesday, July 12, 20235:22:08 PM
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
CREATE TABLE dbo.Tmp_AspNetUsers
	(
	Id nvarchar(450) NOT NULL,
	Discriminator nvarchar(MAX) NULL,
	FirstName nvarchar(50) NULL,
	LastName nvarchar(50) NULL,
	MobileNumber bigint NULL,
	Address nvarchar(450) NULL,
	UserImage nvarchar(450) NULL,
	CreatedDate datetime2(7) NULL,
	UserName nvarchar(256) NULL,
	NormalizedUserName nvarchar(256) NULL,
	Email nvarchar(256) NULL,
	NormalizedEmail nvarchar(256) NULL,
	EmailConfirmed bit NOT NULL,
	PasswordHash nvarchar(MAX) NULL,
	SecurityStamp nvarchar(MAX) NULL,
	ConcurrencyStamp nvarchar(MAX) NULL,
	PhoneNumber nvarchar(MAX) NULL,
	PhoneNumberConfirmed bit NOT NULL,
	TwoFactorEnabled bit NOT NULL,
	LockoutEnd datetimeoffset(7) NULL,
	LockoutEnabled bit NOT NULL,
	AccessFailedCount int NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_AspNetUsers SET (LOCK_ESCALATION = TABLE)
GO
IF EXISTS(SELECT * FROM dbo.AspNetUsers)
	 EXEC('INSERT INTO dbo.Tmp_AspNetUsers (Id, Discriminator, FirstName, LastName, MobileNumber, Address, UserImage, CreatedDate, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)
		SELECT Id, Discriminator, FirstName, LastName, MobileNumber, Address, UserImage, CreatedDate, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount FROM dbo.AspNetUsers WITH (HOLDLOCK TABLOCKX)')
GO
ALTER TABLE dbo.PurChaseOrder
	DROP CONSTRAINT FK_PurChaseOrder_AspNetUsers
GO
ALTER TABLE dbo.Quotations
	DROP CONSTRAINT FK_Quotation_AspNetUsers
GO
ALTER TABLE dbo.AspNetUserClaims
	DROP CONSTRAINT FK_AspNetUserClaims_AspNetUsers_UserId
GO
ALTER TABLE dbo.AspNetUserLogins
	DROP CONSTRAINT FK_AspNetUserLogins_AspNetUsers_UserId
GO
ALTER TABLE dbo.AspNetUserRoles
	DROP CONSTRAINT FK_AspNetUserRoles_AspNetUsers_UserId
GO
ALTER TABLE dbo.AspNetUserTokens
	DROP CONSTRAINT FK_AspNetUserTokens_AspNetUsers_UserId
GO
DROP TABLE dbo.AspNetUsers
GO
EXECUTE sp_rename N'dbo.Tmp_AspNetUsers', N'AspNetUsers', 'OBJECT' 
GO
ALTER TABLE dbo.AspNetUsers ADD CONSTRAINT
	PK_AspNetUsers PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
select Has_Perms_By_Name(N'dbo.AspNetUsers', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.AspNetUsers', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.AspNetUsers', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.AspNetUserTokens ADD CONSTRAINT
	FK_AspNetUserTokens_AspNetUsers_UserId FOREIGN KEY
	(
	UserId
	) REFERENCES dbo.AspNetUsers
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.AspNetUserTokens SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.AspNetUserTokens', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.AspNetUserTokens', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.AspNetUserTokens', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.AspNetUserRoles ADD CONSTRAINT
	FK_AspNetUserRoles_AspNetUsers_UserId FOREIGN KEY
	(
	UserId
	) REFERENCES dbo.AspNetUsers
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.AspNetUserRoles SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.AspNetUserRoles', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.AspNetUserRoles', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.AspNetUserRoles', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.AspNetUserLogins ADD CONSTRAINT
	FK_AspNetUserLogins_AspNetUsers_UserId FOREIGN KEY
	(
	UserId
	) REFERENCES dbo.AspNetUsers
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.AspNetUserLogins SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.AspNetUserLogins', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.AspNetUserLogins', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.AspNetUserLogins', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.AspNetUserClaims ADD CONSTRAINT
	FK_AspNetUserClaims_AspNetUsers_UserId FOREIGN KEY
	(
	UserId
	) REFERENCES dbo.AspNetUsers
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.AspNetUserClaims SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.AspNetUserClaims', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.AspNetUserClaims', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.AspNetUserClaims', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.Quotations ADD CONSTRAINT
	FK_Quotation_AspNetUsers FOREIGN KEY
	(
	UserId
	) REFERENCES dbo.AspNetUsers
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Quotations SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Quotations', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Quotations', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Quotations', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.PurChaseOrder ADD CONSTRAINT
	FK_PurChaseOrder_AspNetUsers FOREIGN KEY
	(
	UserId
	) REFERENCES dbo.AspNetUsers
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.PurChaseOrder SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.PurChaseOrder', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.PurChaseOrder', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.PurChaseOrder', 'Object', 'CONTROL') as Contr_Per 