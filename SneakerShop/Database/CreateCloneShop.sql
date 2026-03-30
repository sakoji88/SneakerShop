-- 1) Создание БД (выполнить в SSMS)
IF DB_ID('CloneShop') IS NULL
BEGIN
    CREATE DATABASE CloneShop;
END
GO

USE CloneShop;
GO

-- 2) Таблица пользователей
IF OBJECT_ID('dbo.Users', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Users
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Login NVARCHAR(50) NOT NULL UNIQUE,
        [Password] NVARCHAR(128) NOT NULL,
        Email NVARCHAR(120) NOT NULL
    );
END
GO

-- 3) Таблица клонов
IF OBJECT_ID('dbo.Clones', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Clones
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Price DECIMAL(10,2) NOT NULL,
        [Description] NVARCHAR(1000) NULL,
        Intelligence INT NOT NULL,
        Toxicity INT NOT NULL,
        Charisma INT NOT NULL,
        ImagePath NVARCHAR(255) NULL
    );
END
GO

-- 4) Таблица заказов
IF OBJECT_ID('dbo.Orders', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Orders
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        UserId INT NOT NULL,
        [Date] DATETIME NOT NULL DEFAULT(GETDATE()),
        TotalPrice DECIMAL(10,2) NOT NULL,
        CONSTRAINT FK_Orders_Users FOREIGN KEY(UserId) REFERENCES dbo.Users(Id)
    );
END
GO

-- 5) Таблица позиций заказа
IF OBJECT_ID('dbo.OrderItems', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.OrderItems
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        OrderId INT NOT NULL,
        CloneId INT NOT NULL,
        Quantity INT NOT NULL,
        CONSTRAINT FK_OrderItems_Orders FOREIGN KEY(OrderId) REFERENCES dbo.Orders(Id),
        CONSTRAINT FK_OrderItems_Clones FOREIGN KEY(CloneId) REFERENCES dbo.Clones(Id)
    );
END
GO

-- 6) Стартовые данные каталога
IF NOT EXISTS (SELECT 1 FROM dbo.Clones)
BEGIN
    INSERT INTO dbo.Clones (Name, Price, [Description], Intelligence, Toxicity, Charisma, ImagePath)
    VALUES
    (N'Неоновый Стратег', 24999.00, N'Холодный тактик для сложных переговоров.', 95, 24, 68, N'Resources/Images/clone1.png'),
    (N'Саркастичный Аналитик', 19990.00, N'Разбирает любую проблему, но любит язвить.', 91, 63, 52, N'Resources/Images/clone2.png'),
    (N'Оратор Прайм', 27990.00, N'Публичные выступления уровня топ-спикера.', 84, 37, 96, N'Resources/Images/clone3.png');
END
GO
