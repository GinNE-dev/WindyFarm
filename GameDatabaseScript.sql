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

CREATE TABLE PlayerDat (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    DisplayName NVARCHAR(255) NOT NULL,
    Diamond INT NOT NULL CHECK (Diamond >= 0),
    Gold INT NOT NULL CHECK (Gold >= 0),
    Level INT NOT NULL CHECK (Level > 0),
    Exp INT NOT NULL CHECK (Exp >= 0)
);
--DROP TABLE PlayerDat
--SELECT * FROM [DBO].PlayerDat WHRERE Id = '54B35ACF-E588-47AC-B404-01A85D053C2F'
--DELETE FROM [DBO].PlayerDat
CREATE TABLE Account (
    Email NVARCHAR(255) PRIMARY KEY,
    HashedPassword NVARCHAR(255) NOT NULL,
    PlayerId UNIQUEIDENTIFIER,
    CONSTRAINT FK_Account_Player FOREIGN KEY (PlayerId) REFERENCES PlayerDat(Id)
);
--DROP TABLE Account
--DELETE FROM [DBO].Account WHERE Email = 'gin2002fmt@gmail.com'
--SELECT * FROM Account
INSERT INTO PlayerDat (Id, DisplayName, Diamond, Gold, Level, Exp) VALUES('beb5642a-cfd3-462f-9633-24862e97a692', 'Gin', 0, 0, 1, 0)
INSERT INTO Account (Email, HashedPassword, PlayerId) VALUES('gin2002fsh@gmail.com', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a91', 'beb5642a-cfd3-462f-9633-24862e97a692')
