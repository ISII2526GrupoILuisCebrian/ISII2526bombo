SET IDENTITY_INSERT [dbo].[DeliveryAssignments] ON;

INSERT INTO [dbo].[DeliveryAssignments] 
    ([Id], [DeliveryDriverId], [ExtraReward], [PersonalMessage], [DeliveryAssignmentDone]) 
VALUES 
    -- Assignment 1: Assigned to Driver 1, using current time as placeholder for Done date
    (1, 1, 5.00, 'Handle with care.', '2025-11-13 19:30:00'),
    
    -- Assignment 2: Assigned to Driver 2, NULL ExtraReward and Message is allowed
    (2, 2, 3.00, 'Not in a rush', '2025-11-13 19:32:00'),
    
    -- Assignment 3: Assigned to Driver 3
    (3, 3, 10.00, 'Rush delivery required.', '2025-11-13 19:35:00');

SET IDENTITY_INSERT [dbo].[DeliveryAssignments] OFF;