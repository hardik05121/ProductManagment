
/****** Object:  Table [dbo].[Quotations]    Script Date: 7/19/2023 12:49:43 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Quotations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SupplierId] [int] NOT NULL,
	[UserId] [nvarchar](450) NULL,
	[QuotationNumber] [nvarchar](450) NOT NULL,
	[OrderDate] [datetime] NOT NULL,
	[DeliveryDate] [datetime] NULL,
	[TermCondition] [nvarchar](450) NULL,
	[Notes] [nvarchar](450) NULL,
	[ScanBarCode] [nvarchar](450) NULL,
	[GrandTotal] [float] NULL,
 CONSTRAINT [PK_Quotation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Quotations]  WITH CHECK ADD  CONSTRAINT [FK_Quotation_Suppliers] FOREIGN KEY([SupplierId])
REFERENCES [dbo].[Suppliers] ([Id])
GO

ALTER TABLE [dbo].[Quotations] CHECK CONSTRAINT [FK_Quotation_Suppliers]
GO


