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
INSERT INTO PlayerDat (Id, DisplayName, Diamond, Gold, Level, Exp, Gender, PositionX, PositionY, PositionZ, MapId, AccountId) VALUES('beb5642a-cfd3-462f-9633-24862e97a692', 'Gin', 0, 0, 1, 0, 'Male', 0, 0, 0, 0, 'gin2002fsh@gmail.com')

CREATE TABLE ItemDat
(
	Id UNIQUEIDENTIFIER PRIMARY KEY,
	ItemType INT NOT NULL,
	Quality INT NOT NULL CHECK (Quality >= 0 AND Quality < 5)
);

INSERT INTO ItemDat(Id, ItemType, Quality) 
VALUES
--('00000000-0000-0000-0000-000000000000', 0, 1),
('beb5642a-cfd3-462f-9633-24862e97a691', 1, 1),
('beb5642a-cfd3-462f-9633-24862e97a692', 2, 2)
--SELECT * FROM ItemDat WHERE Id = '304913B9-BC83-41AF-8754-4CD1E018F3BE'
--DELETE FROM ItemDat
CREATE TABLE InventorySlotDat (
    PlayerId UNIQUEIDENTIFIER NOT NULL,
    Slot INT NOT NULL CHECK (Slot >= 0),
    ItemDatId UNIQUEIDENTIFIER,
    StackCount INT NOT NULL CHECK (StackCount >= 0),
    PRIMARY KEY (PlayerId, Slot),
    CONSTRAINT FK_Inventory_Player FOREIGN KEY (PlayerId) REFERENCES PlayerDat(Id),
    CONSTRAINT FK_Inventory_Item FOREIGN KEY (ItemDatId) REFERENCES ItemDat(Id)
);

INSERT INTO InventorySlotDat(PlayerId, Slot, ItemDatId, StackCount)
VALUES
('beb5642a-cfd3-462f-9633-24862e97a692', 0, 'beb5642a-cfd3-462f-9633-24862e97a691', 111),
('beb5642a-cfd3-462f-9633-24862e97a692', 2, 'beb5642a-cfd3-462f-9633-24862e97a691', 123),
('beb5642a-cfd3-462f-9633-24862e97a692', 4, 'beb5642a-cfd3-462f-9633-24862e97a692', 976),
('beb5642a-cfd3-462f-9633-24862e97a692', 5, 'beb5642a-cfd3-462f-9633-24862e97a692', 245);
--SELECT * FROM InventorySlotDat
--DELETE FROM InventorySlotDat