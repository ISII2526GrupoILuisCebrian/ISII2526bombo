INSERT INTO [dbo].[PurchaseDeliveries] 
    ([DeliveryAssignmentId], [PurchaseOrderId], [Date], [Priority]) 
VALUES 
    -- Link 1: Order 1001 assigned to Delivery 7001 (High Priority: 0)
    (1, 1, '2025-11-13 19:45:00', 0),
    
    -- Link 2: Order 1002 assigned to Delivery 7002 (Medium Priority: 1)
    (2, 2, '2025-11-13 19:47:00', 1),
    
    -- Link 3: Order 1003 assigned to Delivery 7003 (Low Priority: 2)
    (3, 3, '2025-11-13 19:49:00', 2);