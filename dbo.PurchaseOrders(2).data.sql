SET IDENTITY_INSERT [dbo].[PurchaseOrders] ON;

INSERT INTO [dbo].[PurchaseOrders] 
    ([Id], [City], [Date], [Description], [NameSurname], [PostalCode], [Rating], [Street], [TotalPrice], [PaymentMethodId], [State], [CustomerId]) 
VALUES 
    -- Order 1: Customer 1, Payment 1, State: Request (0)
    (1001, 'Madrid', '2025-11-13 19:40:00', 'Order for electronic goods', 'Ana García', '28001', 3, 'Calle Mayor, 15', 89.99, 1, 0, 1),
    
    -- Order 2: Customer 2, Payment 2, State: Request (0)
    (1002, 'Barcelona', '2025-11-12 19:40:00', 'Groceries delivery', 'Marco Pérez', '08005', 5, 'Avenida Diagonal, 220', 45.50, 2, 0, 2),
    
    -- Order 3: Customer 3, Payment 3, State: Request (0)
    (1003, 'Sevilla', '2025-11-13 14:40:00', 'Book and stationery order', 'Laura Martínez', '41002', 4, 'Plaza Nueva, 5', 22.75, 3, 0, 3);

SET IDENTITY_INSERT [dbo].[PurchaseOrders] OFF;