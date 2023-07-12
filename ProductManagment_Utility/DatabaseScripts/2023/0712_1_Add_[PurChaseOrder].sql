/*
   Wednesday, July 12, 202312:36:35 PM
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
CREATE TABLE dbo.PurChaseOrders
	(
	Id int NOT NULL IDENTITY (1, 1),
	QuotationId int NOT NULL,
	PONumber nvarchar(50) NOT NULL,
	PaymentStatus nvarchar(50) NULL,
	IsReturn nvarchar(50) NULL,
	OrderDate datetime NOT NULL,
	DeliveryDate datetime NOT NULL,
	SupplierId int NOT NULL,
	UserId nvarchar(450) NOT NULL,
	TermCondition nvarchar(50) NULL,
	Notes nvarchar(450) NULL,
	ScanBarcode nvarchar(450) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.PurChaseOrders ADD CONSTRAINT
	PK_PurChaseOrder PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
