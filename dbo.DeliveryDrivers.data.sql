SET IDENTITY_INSERT [dbo].[DeliveryDrivers] ON;

INSERT INTO [dbo].[DeliveryDrivers] ([Id], [Available], [Name]) 
VALUES 
    (1, 'TRUE', 'Marco Rossi');

INSERT INTO [dbo].[DeliveryDrivers] ([Id], [Available], [Name]) 
VALUES 
    (2, 'TRUE', 'Sofia Bianchi');

INSERT INTO [dbo].[DeliveryDrivers] ([Id], [Available], [Name]) 
VALUES 
    (3, 'FALSE', 'Luca Verdi');

SET IDENTITY_INSERT [dbo].[DeliveryDrivers] OFF;