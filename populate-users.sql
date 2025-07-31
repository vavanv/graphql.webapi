-- Populate Users table with demo users
-- Password hashes are SHA256 hashes of the passwords

INSERT INTO Users (Username, Email, PasswordHash, FirstName, LastName, IsActive, CreatedAt) 
VALUES 
('admin', 'admin@example.com', 'jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=', 'Admin', 'User', 1, GETUTCDATE()),
('user', 'user@example.com', 'vQnqONVpW51bJnyPcC3jyiTKVty8XVmc+GLrFBaWus=', 'Regular', 'User', 1, GETUTCDATE());

-- Verify the users were inserted
SELECT Id, Username, Email, FirstName, LastName, IsActive, CreatedAt FROM Users; 