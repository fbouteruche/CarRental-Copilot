-- Create the CarRental database if it doesn't exist
IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'CarRental')
BEGIN
    CREATE DATABASE [CarRental];
END
GO

USE [CarRental];
GO

-- Create tables if they don't exist
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Partner]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Partner](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Name] [nvarchar](255) NULL,
        [Description] [nvarchar](max) NULL,
        PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Coupon]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Coupon](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Code] [nvarchar](50) NULL,
        [PartnerId] [int] NULL,
        [Discount] [float] NULL,
        [ExpireDate] [datetime] NULL,
        PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_Coupon_Partner] FOREIGN KEY([PartnerId]) REFERENCES [dbo].[Partner] ([Id])
    );
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Customer]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Customer](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Name] [nvarchar](255) NULL,
        [Address] [nvarchar](255) NULL,
        [Phone] [nvarchar](50) NULL,
        [Email] [nvarchar](255) NULL,
        [BirthDate] [datetime] NULL,
        PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Employee]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Employee](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Name] [nvarchar](255) NULL,
        [Phone] [nvarchar](50) NULL,
        [Email] [nvarchar](255) NULL,
        PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Service]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Service](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Name] [nvarchar](255) NULL,
        [Description] [nvarchar](max) NULL,
        [Price] [decimal](18, 2) NULL,
        PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Vehicle]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Vehicle](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Make] [nvarchar](255) NULL,
        [Model] [nvarchar](255) NULL,
        [Year] [int] NULL,
        [Seats] [int] NULL,
        [Doors] [int] NULL,
        [Transmission] [nvarchar](100) NULL,
        [DailyRate] [decimal](18, 2) NULL,
        [Available] [bit] NULL,
        PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Vehicle_Image]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Vehicle_Image](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [VehicleId] [int] NULL,
        [Image] [varbinary](max) NULL,
        PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_Vehicle_Image_Vehicle] FOREIGN KEY([VehicleId]) REFERENCES [dbo].[Vehicle] ([Id])
    );
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Vehicle_Group]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Vehicle_Group](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Name] [nvarchar](255) NULL,
        PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rental]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Rental](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [VehicleId] [int] NULL,
        [CustomerId] [int] NULL,
        [EmployeeId] [int] NULL,
        [StartDate] [datetime] NULL,
        [EndDate] [datetime] NULL,
        [CouponId] [int] NULL,
        [TotalPrice] [decimal](18, 2) NULL,
        PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_Rental_Vehicle] FOREIGN KEY([VehicleId]) REFERENCES [dbo].[Vehicle] ([Id]),
        CONSTRAINT [FK_Rental_Customer] FOREIGN KEY([CustomerId]) REFERENCES [dbo].[Customer] ([Id]),
        CONSTRAINT [FK_Rental_Employee] FOREIGN KEY([EmployeeId]) REFERENCES [dbo].[Employee] ([Id]),
        CONSTRAINT [FK_Rental_Coupon] FOREIGN KEY([CouponId]) REFERENCES [dbo].[Coupon] ([Id])
    );
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Service_Rental]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Service_Rental](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [ServiceId] [int] NULL,
        [RentalId] [int] NULL,
        PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_Service_Rental_Service] FOREIGN KEY([ServiceId]) REFERENCES [dbo].[Service] ([Id]),
        CONSTRAINT [FK_Service_Rental_Rental] FOREIGN KEY([RentalId]) REFERENCES [dbo].[Rental] ([Id])
    );
END
GO