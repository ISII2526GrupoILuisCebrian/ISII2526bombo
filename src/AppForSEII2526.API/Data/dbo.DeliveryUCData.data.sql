
SET IDENTITY_INSERT dbo.DeliveryDrivers ON;

INSERT INTO dbo.DeliveryDrivers (Id, Available, Name) VALUES
(1, 1, 'Marco Rossi'),
(2, 1, 'Sofia Bianchi'),
(3, 0, 'Luca Verdi');

SET IDENTITY_INSERT dbo.DeliveryDrivers OFF;



INSERT INTO dbo.AspNetUsers
(Id, AccountCreationDate, Address, Name, Surname, UserName, NormalizedUserName,
 Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp,
 ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled,
 LockoutEnd, LockoutEnabled, AccessFailedCount)
VALUES
(N'1', '2003-04-12', 'Calle Tejares 40, Albacete', 'Antony', 'Matheus dos Santos',
 'antony7', 'ANTONY7', NULL, NULL, 1,
 'AQAAAAIAAYagAAAAEBU/4EQun1qc6iBRHJ7Z+7w+SpsG3EhOgcBgPY+gP/nqg/JSV2tC5aaeecmAv9Z1Sg==',
 'QTXB7L6725IPUIGLUJWMLUR5L3ZRVZYL',
 '35e9d49b-ddd6-4019-abca-414ca898fb46',
 NULL, 0, 0, NULL, 1, 0),

(N'2', '1999-06-24', 'Calle Rosario 22, Albacete', 'Luis', 'Milla',
 'luismi', 'LUISMI', NULL, NULL, 1,
 'AQAAAAIAAYagAAAAEEvxMBzj3cVTwFRl0DEyb/YEl22KzfuuCEHTuL2gXiAf1tfvEZ7pGet7nsDmnJ9Zdw==',
 'HWUYGUVW5NJQCYGT3Y7CMWHKGSUQOWQH',
 'e970f5ae-7148-472b-a8ab-5a1308ded31c',
 NULL, 0, 0, NULL, 1, 0),

(N'3', '2010-09-19', 'Calle Oro 15, Albacete', 'Isi', 'Palazon',
 'isi_palazon', 'ISI_PALAZON', NULL, NULL, 1,
 'AQAAAAIAAYagAAAAELFDnEv5Eq7TX4wFPtIcRMT6/ZeDuRZDx+M4DwJZADjj/hFvS45NxIZUky6q9T2SDw==',
 '5M5FTAAGJGBHF5YDVWXXVQ4IAJGNCSSO',
 'c22958c7-3a9a-42de-9c1c-535c92e3a43b',
 NULL, 0, 0, NULL, 1, 0);



SET IDENTITY_INSERT dbo.PaymentMethods ON;

INSERT INTO dbo.PaymentMethods
(Id, UserId, PaymentMethodType, TelephoneNumber, CreditCardNumber, ExpirationDate, Email)
VALUES
(1, 1, 'PayPal', NULL, NULL, NULL, 'customer.one@example.com'),
(2, 1, 'Bizum', '601234567', NULL, NULL, NULL),
(3, 1, 'CreditCard', NULL, '5555444433332222', '2029-07-01', NULL);

SET IDENTITY_INSERT dbo.PaymentMethods OFF;



SET IDENTITY_INSERT dbo.PurchaseOrders ON;

INSERT INTO dbo.PurchaseOrders
(Id, City, Date, Description, NameSurname, PostalCode, Rating,
 Street, TotalPrice, PaymentMethodId, State, CustomerId)
VALUES
(1, 'Madrid',    '2025-11-13 19:40:00', 'Order for electronic goods',
 'Ana García', '28001', 3, 'Calle Mayor, 15', 89.99, 1, 0, '1'),

(2, 'Barcelona', '2025-11-12 19:40:00', 'Groceries delivery',
 'Marco Pérez', '08005', 5, 'Avenida Diagonal, 220', 45.50, 2, 0, '2'),

(3, 'Sevilla',   '2025-11-13 14:40:00', 'Book and stationery order',
 'Laura Martínez', '41002', 4, 'Plaza Nueva, 5', 22.75, 3, 0, '3');

SET IDENTITY_INSERT dbo.PurchaseOrders OFF;



SET IDENTITY_INSERT dbo.DeliveryAssignments ON;

INSERT INTO dbo.DeliveryAssignments
(Id, DeliveryDriverId, ExtraReward, PersonalMessage, DeliveryAssignmentDone)
VALUES
(1, 1, 5.00,  'Handle with care.',        '2025-11-13 19:30:00'),
(2, 2, 3.00,  'Not in a rush',             '2025-11-13 19:32:00'),
(3, 3, 10.00, 'Rush delivery required.',   '2025-11-13 19:35:00');

SET IDENTITY_INSERT dbo.DeliveryAssignments OFF;

INSERT INTO dbo.PurchaseDeliveries
(DeliveryAssignmentId, PurchaseOrderId, Date, Priority)
VALUES
(1, 1, '2025-11-13 19:45:00', 0),
(2, 2, '2025-11-13 19:47:00', 1),
(3, 3, '2025-11-13 19:49:00', 2);
