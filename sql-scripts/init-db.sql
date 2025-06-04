-- Create the CarRental database if it doesn't exist
IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'CarRental')
BEGIN
    CREATE DATABASE [CarRental];
END
GO

USE [CarRental];
GO

-- Create Partner table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Partner]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Partner](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [PartnerName] [nvarchar](255) NULL,
        PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END

-- Create Coupon table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Coupon]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Coupon](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [CouponName] [nvarchar](255) NULL,
        [Code] [nvarchar](50) NULL,
        [MinimumValue] [float] NULL,
        [Value] [float] NULL,
        [IsFixedDiscount] [bit] NULL,
        [Validity] [datetime] NULL,
        [PartnerId] [int] NULL,
        PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_Coupon_Partner] FOREIGN KEY([PartnerId]) REFERENCES [dbo].[Partner] ([Id])
    );
END

-- Create Customer table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Customer]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Customer](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Name] [nvarchar](255) NULL,
        [UniqueRegister] [nvarchar](50) NULL,
        [Address] [nvarchar](255) NULL,
        [Phone] [nvarchar](50) NULL,
        [Email] [nvarchar](255) NULL,
        [IsIndividual] [bit] NULL,
        [DriverLicense] [nvarchar](50) NULL,
        [DriverLicenseValidity] [datetime] NULL,
        PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END

-- Create Employee table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Employee]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Employee](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Name] [nvarchar](255) NULL,
        [UniqueRegister] [nvarchar](50) NULL,
        [Address] [nvarchar](255) NULL,
        [Phone] [nvarchar](50) NULL,
        [Email] [nvarchar](255) NULL,
        [IsIndividual] [bit] NULL,
        [InternalRegister] [int] NULL,
        [AccessUser] [nvarchar](50) NULL,
        [Password] [nvarchar](50) NULL,
        [Role] [nvarchar](100) NULL,
        [Salary] [float] NULL,
        [AdmissionDate] [datetime] NULL,
        PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END

-- Create Service table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Service]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Service](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Name] [nvarchar](255) NULL,
        [IsDailyCharged] [bit] NULL,
        [Value] [float] NULL,
        PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END

-- Create Vehicle_Group table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Vehicle_Group]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Vehicle_Group](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Name] [nvarchar](255) NULL,
        [DailyPlanRate] [float] NULL,
        [DailyKmRate] [float] NULL,
        [ControlledPlanRate] [float] NULL,
        [ControlledKmLimit] [int] NULL,
        [ControlledExceededKmRate] [float] NULL,
        [FreePlanRate] [float] NULL,
        PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END

-- Create Vehicle table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Vehicle]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Vehicle](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Model] [nvarchar](255) NULL,
        [VehicleGroupId] [int] NULL,
        [Plate] [nvarchar](50) NULL,
        [Chassis] [nvarchar](100) NULL,
        [Brand] [nvarchar](100) NULL,
        [Color] [nvarchar](50) NULL,
        [FuelType] [nvarchar](50) NULL,
        [TankCapacity] [float] NULL,
        [Year] [int] NULL,
        [Mileage] [float] NULL,
        [NumberOfDoors] [int] NULL,
        [PeopleCapacity] [int] NULL,
        [TrunkSize] [char](1) NULL,
        [HasAirConditioning] [bit] NULL,
        [HasPowerSteering] [bit] NULL,
        [HasAbsBrakes] [bit] NULL,
        [IsRented] [bit] NULL,
        PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_Vehicle_VehicleGroup] FOREIGN KEY([VehicleGroupId]) REFERENCES [dbo].[Vehicle_Group] ([Id])
    );
END

-- Create Vehicle_Image table
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

-- Create Rental table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rental]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Rental](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [VehicleId] [int] NULL,
        [EmployeeId] [int] NULL,
        [ContractingClientId] [int] NULL,
        [DriverClientId] [int] NULL,
        [CouponId] [int] NULL,
        [DepartureDate] [datetime] NULL,
        [ExpectedReturnDate] [datetime] NULL,
        [ReturnDate] [datetime] NULL,
        [PlanType] [nvarchar](50) NULL,
        [InsuranceType] [nvarchar](50) NULL,
        [RentalPrice] [float] NULL,
        [ReturnPrice] [float] NULL,
        [IsOpen] [bit] NULL,
        PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_Rental_Vehicle] FOREIGN KEY([VehicleId]) REFERENCES [dbo].[Vehicle] ([Id]),
        CONSTRAINT [FK_Rental_ContractingCustomer] FOREIGN KEY([ContractingClientId]) REFERENCES [dbo].[Customer] ([Id]),
        CONSTRAINT [FK_Rental_DriverCustomer] FOREIGN KEY([DriverClientId]) REFERENCES [dbo].[Customer] ([Id]),
        CONSTRAINT [FK_Rental_Employee] FOREIGN KEY([EmployeeId]) REFERENCES [dbo].[Employee] ([Id]),
        CONSTRAINT [FK_Rental_Coupon] FOREIGN KEY([CouponId]) REFERENCES [dbo].[Coupon] ([Id])
    );
END

-- Create Service_Rental table
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