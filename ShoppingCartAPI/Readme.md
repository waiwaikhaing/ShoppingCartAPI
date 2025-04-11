# 🛒 Shopping Cart API (.NET Core)

This project is a simple Shopping Cart API built using ASP.NET Core, Entity Framework Core, and JWT-based authentication.

---

## 📦 Database Schema

### 🧑 User Table

```sql
CREATE TABLE [User] (
    UserId NVARCHAR(100) NOT NULL,
    Username NVARCHAR(100) NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    CreatedBy NVARCHAR(100),
    CreatedOn DATETIME,
    ModifiedBy NVARCHAR(100),
    ModifiedOn DATETIME,
    Active BIT
);

CREATE TABLE [Product] (
    ProductId NVARCHAR(100) NOT NULL,
    ProductName NVARCHAR(100) NOT NULL,
    Price DECIMAL(15, 2) NOT NULL,
    Qty DECIMAL(15,2),
    CreatedBy NVARCHAR(100),
    CreatedOn DATETIME,
    ModifiedBy NVARCHAR(100),
    ModifiedOn DATETIME,
    Active BIT
);

CREATE TABLE [CardItem] (
    CardItemId NVARCHAR(100) NOT NULL,
    UserID NVARCHAR(100) NOT NULL,
    ProductID NVARCHAR(100) NOT NULL,
    Price DECIMAL(15, 2) NOT NULL,
    Qty DECIMAL(15,2),
    Status NVARCHAR(100) NOT NULL,
    CreatedBy NVARCHAR(100),
    CreatedOn DATETIME,
    ModifiedBy NVARCHAR(100),
    ModifiedOn DATETIME,
    Active BIT
);


📚 Packages Used
1.BCrypt.Net-Next – for password hashing

2.System.IdentityModel.Tokens.Jwt – for JWT token generation

3.Microsoft.EntityFrameworkCore – for ORM and data manipulation with MSSQL

4.Serilog – for logging

🚀 How to Run the Project
1.Configure the Database:

-Update your connection string in appsettings.json.

-Run the SQL queries above to create tables.

2.Create a User:

-Use the RegisterUser API to register a new user.

3.Add Products:

-Use the AddProducts API to insert product data.

-Use GetProducts API to fetch product list.

-AddProducts and GetProducts do not require authentication.

4.Generate JWT Token:

-Use the login endpoint with your registered username and password to get a JWT token.

5.Shopping Cart Operations:

-Use the token in the Authorization header (Bearer <your-token>) for these operations:

-AddItemToCart

-GetCartItems

-RemoveItemFromCart

-Checkout

✅ Tips
6.All protected APIs require a valid JWT token.

7.Log files are written using Serilog for debugging and auditing.

🔐 Swagger Usage
8.To use protected APIs in Swagger:

-Click "Authorize" in the top right.

-Enter your token in the format: Bearer YOUR_TOKEN_HERE.



Create Database and Tables

Run the following SQL script in SQL Server to create the database and necessary tables:

```sql
-- Create Database
CREATE DATABASE [ShoppingDB];
GO

USE [ShoppingDB];
GO


--user table
CREATE TABLE [dbo].[UserData](
	[UserId] [nvarchar](100) NOT NULL,
	[Username] [nvarchar](100) NOT NULL,
	[PasswordHash] [nvarchar](255) NOT NULL,
	[CreatedBy] [nvarchar](100) NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedOn] [datetime] NULL,
	[Active] [bit] NULL
) ON [PRIMARY]

--product table
CREATE TABLE [dbo].[Product](
	[ProductId] [nvarchar](100) NOT NULL,
	[ProductName] [nvarchar](100) NOT NULL,
	[Price] [decimal](15, 2) NOT NULL,
	[Qty] [decimal](15, 2) NULL,
	[CreatedBy] [nvarchar](100) NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedOn] [datetime] NULL,
	[Active] [bit] NULL
) ON [PRIMARY]

--product table

CREATE TABLE [dbo].[CartItem](
	[CartItemId] [nvarchar](100) NOT NULL,
	[UserID] [nvarchar](100) NOT NULL,
	[ProductID] [nvarchar](100) NOT NULL,
	[Price] [decimal](15, 2) NOT NULL,
	[Qty] [decimal](15, 2) NULL,
	[Status] [nvarchar](100) NOT NULL,
	[CreatedBy] [nvarchar](100) NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedOn] [datetime] NULL,
	[Active] [bit] NULL
) ON [PRIMARY]


GO

///create view
Create VIEW [dbo].[vw_CartItemDetails] AS

SELECT 
    ci.CartItemId,
    ci.UserId,
    us.Username,
    ci.ProductId,
    p.ProductName AS ProductName,
    ci.Qty,
    ci.Price,
    ci.Status
FROM 
    CartItem ci
JOIN 
    UserData  us ON ci.UserId = us.UserId
JOIN 
    Product p ON ci.ProductId = p.ProductId
WHERE 
    ci.Active = 1;


