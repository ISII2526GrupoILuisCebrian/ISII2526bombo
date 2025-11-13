SET IDENTITY_INSERT [dbo].[PaymentMethods] ON;

INSERT INTO [dbo].[PaymentMethods] 
    ([Id], [UserId], [PaymentMethodType], [TelephoneNumber], [CreditCardNumber], [ExpirationDate], [Email]) 
VALUES 
    (1, 1, 'PayPal', NULL, NULL, NULL, 'customer.one@example.com');

INSERT INTO [dbo].[PaymentMethods] 
    ([Id], [UserId], [PaymentMethodType], [TelephoneNumber], [CreditCardNumber], [ExpirationDate], [Email]) 
VALUES 
    (2, 1, 'Bizum', '601234567', NULL, NULL, NULL);

INSERT INTO [dbo].[PaymentMethods] 
    ([Id], [UserId], [PaymentMethodType], [TelephoneNumber], [CreditCardNumber], [ExpirationDate], [Email]) 
VALUES 
    (3, 1, 'CreditCard', NULL, '5555444433332222', '2029-07-01', NULL);

SET IDENTITY_INSERT [dbo].[PaymentMethods] OFF;