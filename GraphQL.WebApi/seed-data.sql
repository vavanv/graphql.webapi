-- Seed data for jqueryDb database
-- Run this script in SQL Server Management Studio

USE [jqueryDb]
GO

-- Clear existing data (optional)
-- DELETE FROM [Customers]
-- GO

-- Insert sample customers
INSERT INTO [Customers] ([FirstName], [LastName], [Contact], [Email], [DateOfBirth])
VALUES
    ('John', 'Doe', '+1-555-0101', 'john.doe@email.com', '1985-03-15'),
    ('Jane', 'Smith', '+1-555-0102', 'jane.smith@email.com', '1990-07-22'),
    ('Michael', 'Johnson', '+1-555-0103', 'michael.johnson@email.com', '1982-11-08'),
    ('Sarah', 'Williams', '+1-555-0104', 'sarah.williams@email.com', '1988-04-12'),
    ('David', 'Brown', '+1-555-0105', 'david.brown@email.com', '1995-09-30'),
    ('Emily', 'Davis', '+1-555-0106', 'emily.davis@email.com', '1992-01-18'),
    ('Robert', 'Wilson', '+1-555-0107', 'robert.wilson@email.com', '1987-06-25'),
    ('Lisa', 'Anderson', '+1-555-0108', 'lisa.anderson@email.com', '1993-12-03'),
    ('James', 'Taylor', '+1-555-0109', 'james.taylor@email.com', '1980-08-14'),
    ('Amanda', 'Martinez', '+1-555-0110', 'amanda.martinez@email.com', '1991-02-28')
GO

-- Verify the data was inserted
SELECT * FROM [Customers]
GO