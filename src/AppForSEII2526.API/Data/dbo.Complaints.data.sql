SET IDENTITY_INSERT [dbo].[ComplaintTypes] ON
INSERT INTO [dbo].[ComplaintTypes] ([Id], [Name]) VALUES (1, N'Low')
INSERT INTO [dbo].[ComplaintTypes] ([Id], [Name]) VALUES (2, N'Medium')
INSERT INTO [dbo].[ComplaintTypes] ([Id], [Name]) VALUES (3, N'High')
SET IDENTITY_INSERT [dbo].[ComplaintTypes] OFF

SET IDENTITY_INSERT [dbo].[Complaints] ON
INSERT INTO [dbo].[Complaints] ([Id], [ComplaintDate], [Description], [Processed], [CustomerId], [TypeId]) VALUES (6, N'2020-01-12 00:00:00', N'Wrong product', 1, N'1', 1)
INSERT INTO [dbo].[Complaints] ([Id], [ComplaintDate], [Description], [Processed], [CustomerId], [TypeId]) VALUES (8, N'2022-08-31 00:00:00', N'Late delivery', 0, N'2', 3)
INSERT INTO [dbo].[Complaints] ([Id], [ComplaintDate], [Description], [Processed], [CustomerId], [TypeId]) VALUES (9, N'2015-10-20 00:00:00', N'Defective article', 1, N'3', 2)
SET IDENTITY_INSERT [dbo].[Complaints] OFF


