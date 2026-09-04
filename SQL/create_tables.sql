DROP TABLE IF EXISTS Assets;
CREATE TABLE Assets (
  FileName varchar(255) PRIMARY KEY,
  DisplayName varchar(255) NOT NULL,
  Scale FLOAT
);

DROP TABLE IF EXISTS Grid;
CREATE TABLE Grid (
  Grid varchar(8000)
);

DROP TABLE IF EXISTS Users;
CREATE TABLE Users (
  UserName VARCHAR(128) PRIMARY KEY,
  IsAdmin BIT,
  Gold FLOAT,
  BastionTurns int,
  HasBastion BIT
);

DROP TABLE IF EXISTS RandomItem;
DROP TABLE IF EXISTS Attribute;
DROP TABLE IF EXISTS ItemType;
CREATE TABLE ItemType (
  ItemTypeID int PRIMARY KEY,
  ItemTypeValue VARCHAR(255)
)

DROP TABLE IF EXISTS Attribute;
CREATE TABLE Attribute (
  AttributeID int IDENTITY(1,1) PRIMARY KEY,
  AttributeValue VARCHAR(255),
  ItemTypeID int FOREIGN KEY REFERENCES ItemType,
  AttributeDescription VARCHAR(255),
  PriceModifier float
);

DROP TABLE IF EXISTS RandomItem;
CREATE TABLE RandomItem (
  ItemID int IDENTITY(1,1) PRIMARY KEY,
  ItemName VARCHAR(255),
  ItemTypeID int FOREIGN KEY REFERENCES ItemType,
  ItemDescription VARCHAR(255),
  Price float
);

DROP TABLE IF EXISTS NanaStock;
CREATE TABLE NanaStock (
  StockID int IDENTITY(1,1) PRIMARY KEY,
  ItemID int, -- will reference in log form RandomItem
  ItemDescription VARCHAR(255), -- Composite field, made of RandomItem's descriptions. log form so no database connection. 
  Price float -- Log form, RandomItem's Price * Attribute Modifier
);