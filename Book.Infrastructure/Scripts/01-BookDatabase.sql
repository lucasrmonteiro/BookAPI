CREATE DATABASE BookDatabse
GO
       
USE BookDatabse
GO

CREATE TABLE dbo.Category
(
    Id UNIQUEIDENTIFIER default NEWID(),
    Name VARCHAR(100) NOT NULL,
    CONSTRAINT Pk_Category_Id PRIMARY KEY (Id)
)
    GO

CREATE TABLE dbo.Books
(
    Id UNIQUEIDENTIFIER default NEWID(),
    Title VARCHAR(200) NOT NULL,
    Author VARCHAR(100) NOT NULL,
    CategoryId UNIQUEIDENTIFIER NOT NULL,

    CONSTRAINT Pk_Books_Id PRIMARY KEY (Id),
    CONSTRAINT FK_Books_CategoryId FOREIGN KEY (CategoryId) REFERENCES dbo.Category(Id)
)
    GO       