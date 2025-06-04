SELECT * FROM Usuario
SELECT * FROM Cliente
SELECT * FROM Administrador

SELECT U.Id, C.Nombre, U.Correo, C.Telefono, U.FechaRegistro
FROM Usuario U INNER JOIN Administrador C ON U.Id = C.Id


CREATE TABLE Rol(
    Id INT PRIMARY KEY NOT NULL,
    Nombre VARCHAR(50) NOT NULL
);

INSERT INTO Rol(Id, Nombre) VALUES (1, 'Cliente');
INSERT INTO Rol(Id, Nombre) VALUES (2, 'Administrador');

CREATE TABLE Usuario(
    Id INT IDENTITY PRIMARY KEY NOT NULL,
    Correo VARCHAR(100) NOT NULL UNIQUE,
    Pwd VARCHAR(50) NOT NULL,
    Rol INT NOT NULL,
    Estado BIT NOT NULL DEFAULT 1,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    FOREIGN KEY(Rol) REFERENCES Rol(Id)
);

CREATE TABLE Cliente(
    Id INT PRIMARY KEY NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Telefono VARCHAR(15),
    TotalCompras INT DEFAULT 0,
    TotalGastado DECIMAL(10, 2) DEFAULT 0,
    FOREIGN KEY(Id) REFERENCES Usuario(Id) ON DELETE CASCADE
);

select * from cliente

update Cliente
set TotalGastado=0 

update Cliente
set TotalCompras =0

SELECT TOP 5 
    Nombre, 
    TotalCompras, 
    TotalGastado
FROM 
    Cliente
ORDER BY 
    TotalCompras DESC;

CREATE TRIGGER ActualizarTotalComprasyTotalGastado
ON Pedido
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Cliente
    SET TotalCompras = TotalCompras + 1,
        TotalGastado = TotalGastado + inserted.Importe
    FROM Cliente
    INNER JOIN inserted ON Cliente.Id = inserted.IdCliente;
END;


/*ALTER TRIGGER ActualizarTotalGastado
ON Pedido
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Cliente
		SET TotalGastado = TotalGastado + (SELECT Importe FROM inserted)
	WHERE Id = (SELECT IdCliente FROM inserted)
END;*/


UPDATE Cliente
SET 
    TotalCompras = 10,
    TotalGastado = 5000.50
WHERE 
    Id = 6;

UPDATE Cliente
SET 
    TotalCompras = 12,
    TotalGastado = 456545
WHERE 
    Id =44;

	UPDATE Cliente
SET 
    TotalCompras = 7,
    TotalGastado = 7897987
WHERE 
    Id =7;

CREATE TABLE Administrador(
    Id INT PRIMARY KEY NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Telefono VARCHAR(15) NOT NULL,
    FOREIGN KEY(Id) REFERENCES Usuario(Id) ON DELETE CASCADE
);
