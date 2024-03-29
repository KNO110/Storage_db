USE [TEST]
GO
/****** Object:  Table [dbo].[Товары]    Script Date: 12.03.2024 19:50:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Товары](
	[Наименование] [nvarchar](100) NULL,
	[Тип] [nvarchar](50) NULL,
	[Поставщик] [nvarchar](100) NULL,
	[Количество] [int] NULL,
	[id] [int] IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Товары] ON 

INSERT [dbo].[Товары] ([Наименование], [Тип], [Поставщик], [Количество], [id]) VALUES (N'Lenovo legion 3', N'Ноутбук', N'Lenovo', 10, 1)
INSERT [dbo].[Товары] ([Наименование], [Тип], [Поставщик], [Количество], [id]) VALUES (N'Монитор Dell', N'Монитор', N'Dell', 15, 2)
INSERT [dbo].[Товары] ([Наименование], [Тип], [Поставщик], [Количество], [id]) VALUES (N'Принтер HP', N'Принтер', N'HP', 8, 3)
INSERT [dbo].[Товары] ([Наименование], [Тип], [Поставщик], [Количество], [id]) VALUES (N'Мышь Logitech G305', N'Мышь', N'Logitech', 20, 4)
SET IDENTITY_INSERT [dbo].[Товары] OFF
GO
