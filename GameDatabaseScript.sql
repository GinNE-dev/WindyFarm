-- Define the database name
DECLARE @DatabaseName NVARCHAR(128) = 'WindyFarmDatabase';

-- Check if the database exists
IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = @DatabaseName)
BEGIN
    -- Create the database
    EXEC('CREATE DATABASE ' + @DatabaseName);
    
    -- Optional: Include any additional database creation options here
    -- Example: specify the location of the database files
    -- EXEC('CREATE DATABASE ' + @DatabaseName + ' ON (NAME = ''YourDatabaseName'', FILENAME = ''C:\Path\To\YourDatabaseName.mdf'')');
    
    PRINT 'Database created successfully.';
END

USE WindyFarmDatabase
--USE master

CREATE TABLE Account (
    Email NVARCHAR(255) PRIMARY KEY,
    HashedPassword NVARCHAR(255) NOT NULL,
);
--DROP TABLE Account
--DELETE FROM [DBO].Account WHERE Email = 'gin2002fmt@gmail.com'
--SELECT * FROM Account

CREATE TABLE PlayerDat (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    DisplayName NVARCHAR(255) NOT NULL,
    Diamond INT NOT NULL CHECK (Diamond >= 0) DEFAULT(0),
    Gold INT NOT NULL DEFAULT(0) CHECK (Gold >= 0),
    Level INT NOT NULL  DEFAULT(1) CHECK (Level > 0),
    Exp INT NOT NULL DEFAULT(0) CHECK (Exp >= 0),
	Gender NVARCHAR(50) NOT NULL,
    CONSTRAINT CHK_Gender CHECK (Gender IN ('Male', 'Female')),
	MaxInventory INT NOT NULL DEFAULT(25) CHECK (MaxInventory >= 25),
	PositionX FLOAT NOT NULL DEFAULT(0),
    PositionY FLOAT NOT NULL DEFAULT(0),
    PositionZ FLOAT NOT NULL DEFAULT(0),
    MapId INT NOT NULL DEFAULT(0),
	AccountId NVARCHAR(255) UNIQUE NOT NULL,
	CONSTRAINT FK_PlayerDat_Account FOREIGN KEY (AccountId) REFERENCES Account(Email)
);
--DROP TABLE PlayerDat
--SELECT * FROM [DBO].PlayerDat WHRERE Id = '54B35ACF-E588-47AC-B404-01A85D053C2F'
--DELETE FROM [DBO].PlayerDat
INSERT INTO Account (Email, HashedPassword) VALUES('gin2002fsh@gmail.com', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918')
INSERT INTO PlayerDat (Id, DisplayName, Diamond, Gold, Level, Exp, Gender, PositionX, PositionY, PositionZ, MapId, AccountId) 
VALUES('beb5642a-cfd3-462f-9633-24862e97a692', 'Gin', 99, 123000, 1, 0, 'Male', 57, 0, 30, 0, 'gin2002fsh@gmail.com')

CREATE TABLE ItemDat
(
	Id UNIQUEIDENTIFIER PRIMARY KEY,
	ItemType INT NOT NULL,
	Quality INT NOT NULL CHECK (Quality >= 0 AND Quality < 5)
);

INSERT INTO ItemDat(Id, ItemType, Quality) 
VALUES
--('00000000-0000-0000-0000-000000000000', 0, 1),
('beb5642a-cfd3-462f-9633-24862e97a691', 1, 2),
('beb5642a-cfd3-462f-9633-24862e97a692', 2, 2),
('beb5642a-cfd3-462f-9633-24862e97a693', 3, 4),
('beb5642a-cfd3-462f-9633-24862e97a694', 4, 3),
('beb5642a-cfd3-462f-9633-24862e97a695', 5, 2),
('beb5642a-cfd3-462f-9633-24862e97a696', 6, 2),
('beb5642a-cfd3-462f-9633-24862e97a697', 7, 4),
('beb5642a-cfd3-462f-9633-24862e97a698', 8, 3),
('beb5642a-cfd3-462f-9633-24862e97a699', 9, 2),
('beb5642a-cfd3-462f-9633-24862e97a69a', 10, 2),
('beb5642a-cfd3-462f-9633-24862e97a69b', 11, 0),
('beb5642a-cfd3-462f-9633-24862e97a69c', 12, 3),
('beb5642a-cfd3-462f-9633-24862e97a69d', 13, 2),
('beb5642a-cfd3-462f-9633-24862e97a69e', 14, 0),
('beb5642a-cfd3-462f-9633-24862e97a69f', 15, 4);
--SELECT * FROM ItemDat WHERE Id = '304913B9-BC83-41AF-8754-4CD1E018F3BE'
--DELETE FROM ItemDat
CREATE TABLE InventorySlotDat (
    PlayerId UNIQUEIDENTIFIER NOT NULL,
    Slot INT NOT NULL CHECK (Slot >= 0),
    ItemDatId UNIQUEIDENTIFIER,
    StackCount INT NOT NULL CHECK (StackCount >= 0 AND StackCount < 1000),
    PRIMARY KEY (PlayerId, Slot),
    CONSTRAINT FK_Inventory_Player FOREIGN KEY (PlayerId) REFERENCES PlayerDat(Id),
    CONSTRAINT FK_Inventory_Item FOREIGN KEY (ItemDatId) REFERENCES ItemDat(Id)
);

INSERT INTO InventorySlotDat(PlayerId, Slot, ItemDatId, StackCount)
VALUES
('beb5642a-cfd3-462f-9633-24862e97a692', 1, 'beb5642a-cfd3-462f-9633-24862e97a691', 111),
('beb5642a-cfd3-462f-9633-24862e97a692', 2, 'beb5642a-cfd3-462f-9633-24862e97a692', 555),
('beb5642a-cfd3-462f-9633-24862e97a692', 3, 'beb5642a-cfd3-462f-9633-24862e97a693', 999),
('beb5642a-cfd3-462f-9633-24862e97a692', 4, 'beb5642a-cfd3-462f-9633-24862e97a694', 976),
('beb5642a-cfd3-462f-9633-24862e97a692', 5, 'beb5642a-cfd3-462f-9633-24862e97a695', 245),
('beb5642a-cfd3-462f-9633-24862e97a692', 6, 'beb5642a-cfd3-462f-9633-24862e97a696', 111),
('beb5642a-cfd3-462f-9633-24862e97a692', 7, 'beb5642a-cfd3-462f-9633-24862e97a697', 999),
('beb5642a-cfd3-462f-9633-24862e97a692', 8, 'beb5642a-cfd3-462f-9633-24862e97a698', 976),
('beb5642a-cfd3-462f-9633-24862e97a692', 9, 'beb5642a-cfd3-462f-9633-24862e97a699', 245),
('beb5642a-cfd3-462f-9633-24862e97a692', 10, 'beb5642a-cfd3-462f-9633-24862e97a69a', 111),
('beb5642a-cfd3-462f-9633-24862e97a692', 11, 'beb5642a-cfd3-462f-9633-24862e97a69b', 999),
('beb5642a-cfd3-462f-9633-24862e97a692', 12, 'beb5642a-cfd3-462f-9633-24862e97a69c', 976),
('beb5642a-cfd3-462f-9633-24862e97a692', 13, 'beb5642a-cfd3-462f-9633-24862e97a69d', 245),
('beb5642a-cfd3-462f-9633-24862e97a692', 14, 'beb5642a-cfd3-462f-9633-24862e97a69e', 976),
('beb5642a-cfd3-462f-9633-24862e97a692', 15, 'beb5642a-cfd3-462f-9633-24862e97a69f', 245);
--SELECT * FROM InventorySlotDat
--DELETE FROM InventorySlotDat

CREATE TABLE FarmlandDat (
    OwnerId UNIQUEIDENTIFIER,
    PlotIndex INT CHECK (PlotIndex >= 0),
    PlotState VARCHAR(10) CHECK (PlotState IN ('Wild', 'Buyable','Messed', 'Tilled', 'Planted')) DEFAULT('Wild') NOT NULL,
	Fertilized BIT DEFAULT(0)  NOT NULL,
    Seed INT CHECK (Seed >= 0) DEFAULT(0) NOT NULL,
    CropQuality INT CHECK (CropQuality >= 0 AND CropQuality <= 5) DEFAULT(0) NOT NULL,
    PlantedAt DATETIME DEFAULT DATEADD(YEAR, 100, GETDATE()) NOT NULL,
    PRIMARY KEY (OwnerId, PlotIndex),
    FOREIGN KEY (OwnerId) REFERENCES PlayerDat(Id)
);

--INSERT INTO FarmlandDat(OwnerId, PlotIndex) VALUES
--('BEB5642A-CFD3-462F-9633-24862E97A692', 0);
--DROP TABLE FarmlandDat
--SELECT * FROM FarmlandDat
--DELETE FROM FarmlandDat


CREATE TABLE ItemSellPrices
(
	ItemId INT PRIMARY KEY,
	BasePrice INT NOT NULL CHECK(BasePrice >= 0) DEFAULT(0)
)

INSERT INTO ItemSellPrices (ItemId, BasePrice) VALUES
(1, 800), (2, 1600), (3, 2400), (4, 3200), (5, 4000), 
(6, 4800), (7, 5600), (8, 6400), (9, 7200), (10, 8000),
(11, 8800), (12, 9600), (13, 10400), (14, 11200), (15, 12000),
(101, 1200), (102, 2300), (103, 3400), (104, 4500), (105, 5600), 
(106, 6700), (107, 7800), (108, 8900), (109, 10000), (110, 110000),
(111, 12000), (112, 13000), (113, 14000), (114, 15000), (115, 16000);

--SELECT * FROM ItemSellPrices
--DELETE FROM ItemSellPrices

CREATE TABLE FarmShop
(
	SlotIndex INT PRIMARY KEY CHECK(SlotIndex>=0),
	ItemId INT CHECK(ItemId>0) NOT NULL,
	BuyPrice INT CHECK(BuyPrice>=0) NOT NULL
)
--SELECT * FROM FarmShop

INSERT INTO FarmShop (SlotIndex, ItemId, BuyPrice) VALUES
(0, 1, 1000), (1, 2, 2000), (2, 3, 3000), (3, 4, 4000), (4, 5, 5000), 
(5, 6, 6000), (6, 7, 7000), (7, 8, 8000), (8, 9, 9000), (9, 10, 10000),
(10, 11, 11000), (11, 12, 12000), (12, 13, 13000), (13, 14, 14000), (14, 15, 15000);