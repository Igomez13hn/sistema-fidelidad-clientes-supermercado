-- Sistema de Fidelidad de Clientes - Base de Datos
-- Customer Loyalty System Database Schema

-- Crear base de datos
CREATE DATABASE LoyaltySystemDB;
GO

USE LoyaltySystemDB;
GO

-- Tabla de Clientes
CREATE TABLE Customers (
    CustomerID INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(150),
    Phone VARCHAR(50),
    Address VARCHAR(200),
    CreatedDate DATETIME DEFAULT GETDATE(),
    CurrentPoints INT DEFAULT 0
);

-- Tabla de Compras
CREATE TABLE Purchases (
    PurchaseID INT IDENTITY(1,1) PRIMARY KEY,
    CustomerID INT NOT NULL,
    PurchaseDate DATETIME DEFAULT GETDATE(),
    Amount DECIMAL(10,2) NOT NULL,
    EarnedPoints INT NOT NULL,
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);

-- Tabla de Canjes de Puntos
CREATE TABLE Redemptions (
    RedemptionID INT IDENTITY(1,1) PRIMARY KEY,
    CustomerID INT NOT NULL,
    RedemptionDate DATETIME DEFAULT GETDATE(),
    RedeemPoints INT NOT NULL,
    Comments VARCHAR(250),
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);

-- Índices para mejorar rendimiento
CREATE INDEX IX_Purchases_CustomerID ON Purchases(CustomerID);
CREATE INDEX IX_Redemptions_CustomerID ON Redemptions(CustomerID);
CREATE INDEX IX_Customers_Name ON Customers(Name);

-- Datos de ejemplo
INSERT INTO Customers (Name, Email, Phone, Address) VALUES
('Juan Pérez', 'juan.perez@email.com', '555-0001', 'Calle Principal 123'),
('María García', 'maria.garcia@email.com', '555-0002', 'Avenida Central 456'),
('Carlos López', 'carlos.lopez@email.com', '555-0003', 'Plaza Mayor 789');

-- Procedimientos almacenados para operaciones comunes

-- Procedimiento para registrar compra y actualizar puntos
CREATE PROCEDURE sp_RegisterPurchase
    @CustomerID INT,
    @Amount DECIMAL(10,2)
AS
BEGIN
    DECLARE @EarnedPoints INT = CAST(@Amount AS INT)
    
    BEGIN TRANSACTION
    
    INSERT INTO Purchases (CustomerID, Amount, EarnedPoints)
    VALUES (@CustomerID, @Amount, @EarnedPoints)
    
    UPDATE Customers 
    SET CurrentPoints = CurrentPoints + @EarnedPoints
    WHERE CustomerID = @CustomerID
    
    COMMIT TRANSACTION
END
GO

-- Procedimiento para canjear puntos
CREATE PROCEDURE sp_RedeemPoints
    @CustomerID INT,
    @RedeemPoints INT,
    @Comments VARCHAR(250) = NULL
AS
BEGIN
    DECLARE @CurrentPoints INT
    
    SELECT @CurrentPoints = CurrentPoints FROM Customers WHERE CustomerID = @CustomerID
    
    IF @CurrentPoints >= @RedeemPoints
    BEGIN
        BEGIN TRANSACTION
        
        INSERT INTO Redemptions (CustomerID, RedeemPoints, Comments)
        VALUES (@CustomerID, @RedeemPoints, @Comments)
        
        UPDATE Customers 
        SET CurrentPoints = CurrentPoints - @RedeemPoints
        WHERE CustomerID = @CustomerID
        
        COMMIT TRANSACTION
        
        SELECT 'SUCCESS' as Result
    END
    ELSE
    BEGIN
        SELECT 'INSUFFICIENT_POINTS' as Result
    END
END
GO

-- Vista para reporte de clientes
CREATE VIEW vw_CustomerReport AS
SELECT 
    c.CustomerID,
    c.Name,
    c.Email,
    c.Phone,
    c.CurrentPoints,
    ISNULL(SUM(p.Amount), 0) as TotalPurchases,
    ISNULL(SUM(p.EarnedPoints), 0) as TotalEarnedPoints,
    ISNULL(SUM(r.RedeemPoints), 0) as TotalRedeemedPoints,
    COUNT(p.PurchaseID) as TotalTransactions
FROM Customers c
LEFT JOIN Purchases p ON c.CustomerID = p.CustomerID
LEFT JOIN Redemptions r ON c.CustomerID = r.CustomerID
GROUP BY c.CustomerID, c.Name, c.Email, c.Phone, c.CurrentPoints;
GO
