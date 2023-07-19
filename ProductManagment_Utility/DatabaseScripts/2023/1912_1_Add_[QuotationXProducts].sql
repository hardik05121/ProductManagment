
/****** Object:  Table [dbo].[QuotationXProducts]    Script Date: 7/19/2023 12:51:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[QuotationXProducts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UnitId] [int] NOT NULL,
	[QuotationId] [int] NOT NULL,
	[WarehouseId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[TaxId] [int] NOT NULL,
	[Price] [float] NULL,
	[Quantity] [int] NULL,
	[Subtotal] [float] NULL,
	[Discount] [float] NULL,
 CONSTRAINT [PK_QuotationXProducts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[QuotationXProducts]  WITH CHECK ADD  CONSTRAINT [FK_QuotationXProducts_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
GO

ALTER TABLE [dbo].[QuotationXProducts] CHECK CONSTRAINT [FK_QuotationXProducts_Products]
GO

ALTER TABLE [dbo].[QuotationXProducts]  WITH CHECK ADD  CONSTRAINT [FK_QuotationXProducts_Quotations] FOREIGN KEY([QuotationId])
REFERENCES [dbo].[Quotations] ([Id])
GO

ALTER TABLE [dbo].[QuotationXProducts] CHECK CONSTRAINT [FK_QuotationXProducts_Quotations]
GO

ALTER TABLE [dbo].[QuotationXProducts]  WITH CHECK ADD  CONSTRAINT [FK_QuotationXProducts_Taxs] FOREIGN KEY([TaxId])
REFERENCES [dbo].[Taxs] ([Id])
GO

ALTER TABLE [dbo].[QuotationXProducts] CHECK CONSTRAINT [FK_QuotationXProducts_Taxs]
GO

ALTER TABLE [dbo].[QuotationXProducts]  WITH CHECK ADD  CONSTRAINT [FK_QuotationXProducts_Units] FOREIGN KEY([UnitId])
REFERENCES [dbo].[Units] ([Id])
GO

ALTER TABLE [dbo].[QuotationXProducts] CHECK CONSTRAINT [FK_QuotationXProducts_Units]
GO

ALTER TABLE [dbo].[QuotationXProducts]  WITH CHECK ADD  CONSTRAINT [FK_QuotationXProducts_Warehouses] FOREIGN KEY([WarehouseId])
REFERENCES [dbo].[Warehouses] ([Id])
GO

ALTER TABLE [dbo].[QuotationXProducts] CHECK CONSTRAINT [FK_QuotationXProducts_Warehouses]
GO


