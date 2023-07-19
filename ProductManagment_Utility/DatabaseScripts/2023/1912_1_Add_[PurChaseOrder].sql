
/****** Object:  Table [dbo].[PurChaseOrder]    Script Date: 7/19/2023 12:51:57 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PurChaseOrder](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[QuotationId] [int] NOT NULL,
	[PONumber] [nvarchar](50) NOT NULL,
	[PaymentStatus] [nvarchar](50) NULL,
	[IsReturn] [nvarchar](50) NULL,
	[OrderDate] [datetime] NOT NULL,
	[DeliveryDate] [datetime] NOT NULL,
	[SupplierId] [int] NOT NULL,
	[UserId] [nvarchar](450) NULL,
	[TermCondition] [nvarchar](50) NULL,
	[Notes] [nvarchar](450) NULL,
	[ScanBarcode] [nvarchar](450) NULL,
	[GrandTotal] [float] NULL,
 CONSTRAINT [PK_PurChaseOrder] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PurChaseOrder]  WITH CHECK ADD  CONSTRAINT [FK_PurChaseOrder_Quotations] FOREIGN KEY([QuotationId])
REFERENCES [dbo].[Quotations] ([Id])
GO

ALTER TABLE [dbo].[PurChaseOrder] CHECK CONSTRAINT [FK_PurChaseOrder_Quotations]
GO

ALTER TABLE [dbo].[PurChaseOrder]  WITH CHECK ADD  CONSTRAINT [FK_PurChaseOrder_Suppliers] FOREIGN KEY([SupplierId])
REFERENCES [dbo].[Suppliers] ([Id])
GO

ALTER TABLE [dbo].[PurChaseOrder] CHECK CONSTRAINT [FK_PurChaseOrder_Suppliers]
GO


