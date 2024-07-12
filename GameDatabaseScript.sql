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
--DROP DATABASE WindyFarmDatabase

CREATE TABLE Account (
    Email NVARCHAR(255) PRIMARY KEY,
    HashedPassword NVARCHAR(255) NOT NULL,
);
INSERT INTO Account (Email, HashedPassword) VALUES
('gin2002fsh@gmail.com', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918'),
('gin2002fmt@gmail.com', '3c73d1c3df3a787fe626dc17cd4066e792ffa7152f6000242f0ebda89765638c')
--DROP TABLE Account
--DELETE FROM [DBO].Account WHERE Email = 'gin2002fmt@gmail.com'
--SELECT * FROM Account

CREATE TABLE PlayerDat (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    DisplayName NVARCHAR(255) NOT NULL,
    Diamond INT CHECK (Diamond >= 0) DEFAULT(0)  NOT NULL,
    Gold INT DEFAULT(0) CHECK (Gold >= 0) NOT NULL,
    Level INT  DEFAULT(1) CHECK (Level > 0) NOT NULL,
	Energy INT CHECK(Energy>=0) DEFAULT(50) NOT NULL,
    Exp INT DEFAULT(0) CHECK (Exp >= 0) NOT NULL,
	Gender NVARCHAR(50) NOT NULL,
    CONSTRAINT CHK_Gender CHECK (Gender IN ('Male', 'Female')),
	MaxInventory INT DEFAULT(25) CHECK (MaxInventory >= 25) NOT NULL,
	PositionX FLOAT DEFAULT(0) NOT NULL,
    PositionY FLOAT DEFAULT(0) NOT NULL,
    PositionZ FLOAT DEFAULT(0) NOT NULL,
    MapId INT DEFAULT(0) NOT NULL,
	AccountId NVARCHAR(255) UNIQUE NOT NULL,
	CONSTRAINT FK_PlayerDat_Account FOREIGN KEY (AccountId) REFERENCES Account(Email)
);
--DROP TABLE PlayerDat
--SELECT * FROM [DBO].PlayerDat WHRERE Id = '54B35ACF-E588-47AC-B404-01A85D053C2F'
--DELETE FROM [DBO].PlayerDat WHERE NOT DisplayName = 'Gin'
--UPDATE PlayerDat  SET Gold = 100000
--UPDATE PlayerDat SET Gender = 'Male'
INSERT INTO PlayerDat (Id, DisplayName, Diamond, Gold, Level, Exp, Gender, PositionX, PositionY, PositionZ, MapId, AccountId) 
VALUES('beb5642a-cfd3-462f-9633-24862e97a692', 'Gin', 99, 50000, 1, 0, 'Male', 57, 0, 30, 0, 'gin2002fsh@gmail.com')

CREATE TABLE ItemDat
(
	Id UNIQUEIDENTIFIER PRIMARY KEY,
	ItemType INT NOT NULL,
	Quality INT CHECK (Quality > 0 AND Quality <= 5) DEFAULT(1) NOT NULL,
);

INSERT INTO ItemDat(Id, ItemType, Quality) 
VALUES
--('00000000-0000-0000-0000-000000000000', 0, 1),
('beb5642a-cfd3-462f-9633-24862e97a691', 1, 1),
('beb5642a-cfd3-462f-9633-24862e97a692', 2, 2),
('beb5642a-cfd3-462f-9633-24862e97a693', 3, 4),
('beb5642a-cfd3-462f-9633-24862e97a694', 4, 3),
('beb5642a-cfd3-462f-9633-24862e97a695', 5, 2),
('beb5642a-cfd3-462f-9633-24862e97a696', 6, 2),
('beb5642a-cfd3-462f-9633-24862e97a697', 7, 4),
('beb5642a-cfd3-462f-9633-24862e97a698', 8, 3),
('beb5642a-cfd3-462f-9633-24862e97a699', 9, 2),
('beb5642a-cfd3-462f-9633-24862e97a69a', 10, 2),
('beb5642a-cfd3-462f-9633-24862e97a69b', 11, 1),
('beb5642a-cfd3-462f-9633-24862e97a69c', 12, 3),
('beb5642a-cfd3-462f-9633-24862e97a69d', 13, 2),
('beb5642a-cfd3-462f-9633-24862e97a69e', 14, 1),
('beb5642a-cfd3-462f-9633-24862e97a69f', 15, 4),
('beb5642a-cfd3-462f-9633-24862e97a201', 201, 4);
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
('beb5642a-cfd3-462f-9633-24862e97a692', 1, 'beb5642a-cfd3-462f-9633-24862e97a691', 10);
--('beb5642a-cfd3-462f-9633-24862e97a692', 2, 'beb5642a-cfd3-462f-9633-24862e97a692', 555),
--('beb5642a-cfd3-462f-9633-24862e97a692', 3, 'beb5642a-cfd3-462f-9633-24862e97a693', 999),
---('beb5642a-cfd3-462f-9633-24862e97a692', 4, 'beb5642a-cfd3-462f-9633-24862e97a694', 976),
----('beb5642a-cfd3-462f-9633-24862e97a692', 5, 'beb5642a-cfd3-462f-9633-24862e97a695', 245),
--('beb5642a-cfd3-462f-9633-24862e97a692', 6, 'beb5642a-cfd3-462f-9633-24862e97a696', 111),
--('beb5642a-cfd3-462f-9633-24862e97a692', 7, 'beb5642a-cfd3-462f-9633-24862e97a697', 999),
--('beb5642a-cfd3-462f-9633-24862e97a692', 8, 'beb5642a-cfd3-462f-9633-24862e97a698', 976),
--('beb5642a-cfd3-462f-9633-24862e97a692', 9, 'beb5642a-cfd3-462f-9633-24862e97a699', 245),
--('beb5642a-cfd3-462f-9633-24862e97a692', 10, 'beb5642a-cfd3-462f-9633-24862e97a69a', 111),
--('beb5642a-cfd3-462f-9633-24862e97a692', 11, 'beb5642a-cfd3-462f-9633-24862e97a69b', 999),
--('beb5642a-cfd3-462f-9633-24862e97a692', 12, 'beb5642a-cfd3-462f-9633-24862e97a69c', 976),
--('beb5642a-cfd3-462f-9633-24862e97a692', 13, 'beb5642a-cfd3-462f-9633-24862e97a69d', 245),
--('beb5642a-cfd3-462f-9633-24862e97a692', 14, 'beb5642a-cfd3-462f-9633-24862e97a69e', 976),
--('beb5642a-cfd3-462f-9633-24862e97a692', 15, 'beb5642a-cfd3-462f-9633-24862e97a69f', 245),
--('beb5642a-cfd3-462f-9633-24862e97a692', 16, 'beb5642a-cfd3-462f-9633-24862e97a201', 201);
--SELECT * FROM InventorySlotDat
--DELETE FROM InventorySlotDat

CREATE TABLE FarmlandDat (
    OwnerId UNIQUEIDENTIFIER,
    PlotIndex INT CHECK (PlotIndex >= 0),
    PlotState VARCHAR(10) CHECK (PlotState IN ('Wild', 'Buyable','Messed', 'Tilled', 'Planted')) DEFAULT('Wild') NOT NULL,
	Fertilized BIT DEFAULT(0)  NOT NULL,
	CropQualityRiseChange INT CHECK(CropQualityRiseChange>=0) DEFAULT(0) NOT NULL,
    Seed INT CHECK (Seed >= 0) DEFAULT(0) NOT NULL,
    CropQuality INT CHECK (CropQuality > 0 AND CropQuality <= 5) DEFAULT(1) NOT NULL,
    PlantedAt DATETIME DEFAULT DATEADD(YEAR, 100, GETDATE()) NOT NULL,
    PRIMARY KEY (OwnerId, PlotIndex),
    FOREIGN KEY (OwnerId) REFERENCES PlayerDat(Id)
);

--INSERT INTO FarmlandDat(OwnerId, PlotIndex) VALUES
--('BEB5642A-CFD3-462F-9633-24862E97A692', 0);
--DROP TABLE FarmlandDat
--SELECT * FROM FarmlandDat
--DELETE FROM FarmlandDat

CREATE TABLE BarnDat
(
	OwnerId UNIQUEIDENTIFIER,
    SlotIndex INT CHECK (SlotIndex >= 0),
	SpawnerId INT CHECK(SpawnerId >= 0) DEFAULT(0) NOT NULL,
	--SlotState VARCHAR(10) CHECK (SlotState IN ('Empty', 'Growing', 'Grown')) DEFAULT('Empty') NOT NULL,
	AnimalHealth INT CHECK(AnimalHealth >=0 AND AnimalHealth <= 10) DEFAULT(0) NOT NULL,
	GrowAt DATETIME DEFAULT DATEADD(YEAR, 100, GETDATE()) NOT NULL,
	LastFeedAt DATETIME DEFAULT DATEADD(YEAR, 100, GETDATE()) NOT NULL,
	GiveProductAt DATETIME DEFAULT DATEADD(YEAR, 100, GETDATE()) NOT NULL,
	LastTimeMarkerUpdate DATETIME DEFAULT(GETDATE()) NOT NULL,
	--LastFeedDelay FLOAT CHECK(LastFeedDelay>=0) DEFAULT(0) NOT NULL,
	PRIMARY KEY (OwnerId, SlotIndex),
	FOREIGN KEY (OwnerId) REFERENCES PlayerDat(Id)
)

--DROP TABLE BarnDat
--SELECT * FROM BarnDat
--DELETE FROM BarnDat

CREATE TABLE ItemSellPrices
(
	ItemId INT PRIMARY KEY,
	BasePrice INT NOT NULL CHECK(BasePrice >= 0) DEFAULT(0)
)

INSERT INTO ItemSellPrices (ItemId, BasePrice) VALUES
(1, 100), (2, 160), (3, 240), (4, 320), (5, 400), 
(6, 480), (7, 560), (8, 640), (9, 720), (10, 800),
(11, 880), (12, 960), (13, 1040), (14, 1120), (15, 1200),
(101, 180), (102, 300), (103, 450), (104, 600), (105, 700), 
(106, 900), (107, 1100), (108, 1500), (109, 2000), (110, 2100),
(111, 2200), (112, 2300), (113, 2600), (114, 2800), (115, 3000),
(201, 80), (301, 800), (202, 80), (401, 50), (402, 80), (403, 400),
(404, 500), (405, 1000);

--SELECT * FROM ItemSellPrices
--DELETE FROM ItemSellPrices

CREATE TABLE FarmShop
(
	SlotIndex INT PRIMARY KEY CHECK(SlotIndex>=0),
	ItemId INT CHECK(ItemId>0) NOT NULL,
	BuyPrice INT CHECK(BuyPrice>=0) NOT NULL
)
--SELECT * FROM FarmShop
--DELETE FROM FarmShop
INSERT INTO FarmShop (SlotIndex, ItemId, BuyPrice) VALUES
(0, 1, 150), (1, 2, 200), (2, 3, 300), (3, 4, 400), (4, 5, 500), 
(5, 6, 600), (6, 7, 700), (7, 8, 800), (8, 9, 900), (9, 10, 1000),
(10, 11, 1100), (11, 12, 1200), (12, 13, 1300), (13, 14, 1400), (14, 15, 1500),
(15, 201, 100), (16, 301, 1000), (17, 202, 100), (18, 302, 200), (19, 303, 1000),
(20, 304, 2000), (21, 305, 5000);