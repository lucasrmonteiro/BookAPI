USE BookDatabse
GO
    
INSERT INTO BookDatabse.dbo.Category
(Id, Name)
VALUES(newid(), 'Fantsy');

INSERT INTO BookDatabse.dbo.Category
(Id, Name)
VALUES(newid(), 'Science Fiction');

INSERT INTO BookDatabse.dbo.Category
(Id, Name)
VALUES(newid(), 'Romance');
GO