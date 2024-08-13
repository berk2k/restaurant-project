# Db table init
CREATE TABLE Tables (
    TableID INT PRIMARY KEY IDENTITY(1,1),
    TableNumber INT NOT NULL,
    Capacity INT NOT NULL
);
</br>

CREATE TABLE MenuItems (
    MenuItemID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    Price DECIMAL(10, 2) NOT NULL,
    Category NVARCHAR(50) NOT NULL
);
</br>
CREATE TABLE Orders (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    TableID INT FOREIGN KEY REFERENCES Tables(TableID),
    OrderTime DATETIME NOT NULL DEFAULT GETDATE(),
    TotalPrice DECIMAL(10, 2),
    OrderStatus NVARCHAR(50) NOT NULL
);
</br>
CREATE TABLE OrderItems (
    OrderItemID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT FOREIGN KEY REFERENCES Orders(OrderID),
    MenuItemID INT FOREIGN KEY REFERENCES MenuItems(MenuItemID),
    Quantity INT NOT NULL,
    ItemPrice DECIMAL(10, 2) NOT NULL
);
</br>
CREATE TABLE Payments (
    PaymentID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT FOREIGN KEY REFERENCES Orders(OrderID),
    PaymentTime DATETIME NOT NULL DEFAULT GETDATE(),
    PaymentMethod NVARCHAR(50) NOT NULL,
    AmountPaid DECIMAL(10, 2) NOT NULL
);
