SET IDENTITY_INSERT [dbo].[Products] ON
INSERT INTO [dbo].[Products] ([Id], [Name], [Description], [Price], [Colour], [IsReturnable], [Stock], [BrandId]) VALUES (5, N'Chocolate Protein', NULL, CAST(35.00 AS Decimal(5, 2)), N'Brown', 1, 100, 1)
INSERT INTO [dbo].[Products] ([Id], [Name], [Description], [Price], [Colour], [IsReturnable], [Stock], [BrandId]) VALUES (8, N'White Chocolate Protein', NULL, CAST(35.00 AS Decimal(5, 2)), N'White', 1, 100, 1)
SET IDENTITY_INSERT [dbo].[Products] OFF
