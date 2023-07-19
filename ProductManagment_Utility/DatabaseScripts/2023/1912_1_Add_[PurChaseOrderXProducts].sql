

/****** Object:  Table [dbo].[PurChaseOrderXProducts]    Script Date: 7/19/2023 12:52:31 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PurChaseOrderXProducts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PurChaseOrderId] [int] NOT NULL,
	[WarehouseId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[UnitId] [int] NOT NULL,
	[TaxId] [int] NOT NULL,
	[Price] [float] NULL,
	[Quantity] [int] NULL,
	[Subtotal] [float] NULL,
	[Discount] [float] NULL,
 CONSTRAINT [PK_PurChaseOrderXProducts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PurChaseOrderXProducts]  WITH CHECK ADD  CONSTRAINT [FK_PurChaseOrderXProducts_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
GO

ALTER TABLE [dbo].[PurChaseOrderXProducts] CHECK CONSTRAINT [FK_PurChaseOrderXProducts_Products]
GO

ALTER TABLE [dbo].[PurChaseOrderXProducts]  WITH CHECK ADD  CONSTRAINT [FK_PurChaseOrderXProducts_PurChaseOrder] FOREIGN KEY([PurChaseOrderId])
REFERENCES [dbo].[PurChaseOrder] ([Id])
GO

ALTER TABLE [dbo].[PurChaseOrderXProducts] CHECK CONSTRAINT [FK_PurChaseOrderXProducts_PurChaseOrder]
GO

ALTER TABLE [dbo].[PurChaseOrderXProducts]  WITH CHECK ADD  CONSTRAINT [FK_PurChaseOrderXProducts_Taxs] FOREIGN KEY([TaxId])
REFERENCES [dbo].[Taxs] ([Id])
GO

ALTER TABLE [dbo].[PurChaseOrderXProducts] CHECK CONSTRAINT [FK_PurChaseOrderXProducts_Taxs]
GO

ALTER TABLE [dbo].[PurChaseOrderXProducts]  WITH CHECK ADD  CONSTRAINT [FK_PurChaseOrderXProducts_Units] FOREIGN KEY([UnitId])
REFERENCES [dbo].[Units] ([Id])
GO

ALTER TABLE [dbo].[PurChaseOrderXProducts] CHECK CONSTRAINT [FK_PurChaseOrderXProducts_Units]
GO

ALTER TABLE [dbo].[PurChaseOrderXProducts]  WITH CHECK ADD  CONSTRAINT [FK_PurChaseOrderXProducts_Warehouses] FOREIGN KEY([WarehouseId])
REFERENCES [dbo].[Warehouses] ([Id])
GO

ALTER TABLE [dbo].[PurChaseOrderXProducts] CHECK CONSTRAINT [FK_PurChaseOrderXProducts_Warehouses]
GO


