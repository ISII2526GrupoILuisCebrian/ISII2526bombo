SET IDENTITY_INSERT [dbo].[BanReports] ON
INSERT INTO [dbo].[BanReports] ([Id], [DetailedDesciption], [EndDate], [Reason], [StartDate]) VALUES (1, N'Didn''t like the product', N'2010-10-10 00:00:00', N'bad manners', N'2010-10-01 00:00:00')
INSERT INTO [dbo].[BanReports] ([Id], [DetailedDesciption], [EndDate], [Reason], [StartDate]) VALUES (6, N'The shop assistant was ugly', N'2022-01-30 00:00:00', N'Customer demanded the shop assistant to get a low tapper fade', N'2018-08-12 00:00:00')
INSERT INTO [dbo].[BanReports] ([Id], [DetailedDesciption], [EndDate], [Reason], [StartDate]) VALUES (7, N'Shared personal info of another user.', N'2025-12-24 00:00:00', N'Privacy violation', N'2024-10-25 00:00:00')
SET IDENTITY_INSERT [dbo].[BanReports] OFF
