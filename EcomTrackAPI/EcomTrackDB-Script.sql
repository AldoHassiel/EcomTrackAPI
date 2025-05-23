/* Seccion Aldo */

CREATE TABLE Rol(
	Id INT PRIMARY KEY NOT NULL,
	Nombre VARCHAR(50)
)
INSERT INTO Rol(Id, Nombre) VALUES (1, 'Cliente')
INSERT INTO Rol(Id, Nombre) VALUES (2, 'Administrador')

CREATE TABLE Usuario(
	Id INT IDENTITY PRIMARY KEY NOT NULL,
	Correo VARCHAR(100) NOT NULL UNIQUE,
	Pwd VARCHAR(50) NOT NULL,
	Rol INT NOT NULL,
	Estado BIT NOT NULL,
	FechaRegistro DATETIME DEFAULT GETDATE(),
	FOREIGN KEY(Rol) REFERENCES Rol(Id)
)

CREATE TABLE Cliente(
	Id INT PRIMARY KEY NOT NULL,
	Nombre VARCHAR(100) NOT NULL,
	Telefono VARCHAR(15),
	TotalCompras INT DEFAULT 0,
	TotalGastado DECIMAL(10, 2) DEFAULT 0,
	FOREIGN KEY(Id) REFERENCES Usuario(Id)
)

CREATE TABLE Administrador(
	Id INT PRIMARY KEY NOT NULL,
	Nombre VARCHAR(100) NOT NULL,
	Telefono VARCHAR(15) NOT NULL,
	FOREIGN KEY(Id) REFERENCES Usuario(Id)
)

CREATE PROCEDURE RegistrarCliente(
	@Correo VARCHAR(100),
	@Pwd VARCHAR(50),
	@Nombre VARCHAR(100),
	@Telefono VARCHAR(15),
	@NuevoId INT OUTPUT
)
AS
BEGIN
	INSERT INTO Usuario(Correo, Pwd, Rol, Estado) VALUES (@Correo, @Pwd, 1, 1)
	SET @NuevoId = SCOPE_IDENTITY()
	INSERT INTO Cliente(Id, Nombre, Telefono, TotalCompras, TotalGastado) VALUES (@NuevoId, @Nombre, @Telefono, 0, 0)
END

CREATE PROCEDURE RegistrarAdministrador(
	@Correo VARCHAR(100),
	@Pwd VARCHAR(50),
	@Nombre VARCHAR(100),
	@Telefono VARCHAR(15),
	@NuevoId INT OUTPUT
)
AS
BEGIN
	INSERT INTO Usuario(Correo, Pwd, Rol, Estado) VALUES (@Correo, @Pwd, 2, 1)
	SET @NuevoId = SCOPE_IDENTITY()
	INSERT INTO Administrador(Id, Nombre, Telefono) VALUES (@NuevoId, @Nombre, @Telefono)
END

CREATE PROCEDURE ActualizarCliente(
	@Id INT,
	@Correo VARCHAR(100),
	@Pwd VARCHAR(50),
	@Estado BIT,
	@Nombre VARCHAR(100),
	@Telefono VARCHAR(15)
)
AS
BEGIN
	UPDATE Usuario
	SET
		Correo = @Correo,
		Pwd = @Pwd,
		Estado = @Estado
	WHERE Id = @Id

	UPDATE Cliente
	SET
		Nombre = @Nombre,
		Telefono = @Telefono
	WHERE Id = @Id
END

CREATE PROCEDURE ActualizarAdministrador(
	@Id INT,
	@Correo VARCHAR(100),
	@Pwd VARCHAR(50),
	@Estado BIT,
	@Nombre VARCHAR(100),
	@Telefono VARCHAR(15)
)
AS
BEGIN
	UPDATE Usuario
	SET
		Correo = @Correo,
		Pwd = @Pwd,
		Estado = @Estado
	WHERE Id = @Id

	UPDATE Administrador
	SET
		Nombre = @Nombre,
		Telefono = @Telefono
	WHERE Id = @Id
END

CREATE PROCEDURE EliminarCliente(
	@Id INT
)
AS
BEGIN
	DELETE FROM Cliente WHERE Id = @Id;
	DELETE FROM Usuario WHERE Id = @Id AND Rol = 1; 
END

CREATE PROCEDURE EliminarAdministrador(
	@Id INT
)
AS
BEGIN
	DELETE FROM Administrador WHERE Id = @Id;
	DELETE FROM Usuario WHERE Id = @Id AND Rol = 2; 
END

SELECT U.Id, C.Nombre, U.Correo, C.Telefono, U.FechaRegistro, C.TotalCompras, C.TotalGastado 
FROM Usuario U INNER JOIN Cliente C ON U.Id = C.Id WHERE U.Rol = 1

/* Seccion Yuriana ACTUALIZADA*/
-------------------------------------TABLA PEDIDO -------------------------------------------
CREATE TABLE Pedido(
	Id INT IDENTITY PRIMARY KEY NOT NULL,
	Cliente INT,
	FechaPedido DATETIME DEFAULT GETDATE(),
	Importe DECIMAL(10,2),
	Estado INT,
	Calle VARCHAR(100) NOT NULL,
	EntreCalles VARCHAR(200),
    Numero VARCHAR(10),
    Colonia VARCHAR(100),
    Ciudad VARCHAR(100),
    Estado VARCHAR(100),
    CodigoPostal VARCHAR(10),
    Pais VARCHAR(100),
	FOREIGN KEY (Cliente) REFERENCES Cliente(Id),
	FOREIGN KEY (Estado) REFERENCES Estado(Id)
)

GO
CREATE PROCEDURE RegistrarPedido(
	@Id INT,
	@Cliente INT,
	@Importe DECIMAL(10,2),
    @Estado INT,
	@Calle VARCHAR(100), 
	@EntreCalles VARCHAR(200), 
	@Numero VARCHAR(20), 
	@Colonia VARCHAR(100), 
	@Ciudad VARCHAR(100), 
	@Estado VARCHAR(100), 
	@CodigoPostal VARCHAR(10),
	@Pais VARCHAR(100)
)
AS
BEGIN
		BEGIN
			INSERT INTO Pedido(Id, Cliente, FechaPedido, Importe, Estado, Calle, EntreCalles, Numero, Colonia, Ciudad, Estado, CodigoPostal, Pais)
			VALUES (@Id, @Cliente, GETDATE(), @Importe, @Estado, @Calle, @EntreCalles, @Numero, @Colonia, @Ciudad, @Estado, @CodigoPostal, @Pais);
		END

END

GO
CREATE PROCEDURE ActualizarPedido(
	@Id INT,
    @Estado INT
)
AS
BEGIN
			UPDATE Pedido
			SET Estado = @Estado 
			WHERE Id = @Id

END


------------------------------------------------------------------------------------------------------
-------------------------------------------TABLA PAGO ------------------------------------------------

CREATE TABLE Pago(
	Id INT IDENTITY PRIMARY KEY NOT NULL,
	Pedido INT,
	MetodoPago INT,
	ImporteAbonado DECIMAL(10,2),
	FechaPago DATETIME DEFAULT GETDATE(),
	FOREIGN KEY (Pedido) REFERENCES Pedido(Id),
	FOREIGN KEY (MetodoPago) REFERENCES MetodoPago(Id),
)

GO

CREATE PROCEDURE RegistrarPago(
	@Id INT,
	@Pedido INT,
	@MetodoPago INT,
	@ImporteAbonado DECIMAL(10,2)
)
AS
BEGIN
	INSERT INTO Pago(Id, Pedido, MetodoPago, ImporteAbonado, FechaPago)
	VALUES (@id, @Pedido, @MetodoPago, @ImporteAbonado, GETDATE())
END
-------------------------------------------------------------------------------------------------------------
---------------------------------------------TABLA DIRECCION ------------------------------------------------
CREATE TABLE Direccion(
	Id INT PRIMARY KEY NOT NULL,
	Cliente INT,
	Tipo INT,
	Calle VARCHAR(100) NOT NULL,
	EntreCalles VARCHAR(200),
    Numero VARCHAR(10),
    Colonia VARCHAR(100),
    Ciudad VARCHAR(100),
    Estado VARCHAR(100),
    CodigoPostal VARCHAR(10),
    Pais VARCHAR(100),
    FOREIGN KEY (Cliente) REFERENCES Cliente(Id),
	FOREIGN KEY (Tipo) REFERENCES Tipo(Id)
)
GO
CREATE PROCEDURE GuardarDireccion(
	@Id INT,
    @Cliente INT,
    @Tipo INT,
    @Calle VARCHAR(100),
    @EntreCalles VARCHAR(200),
    @Numero VARCHAR(10),
    @Colonia VARCHAR(100),
    @Ciudad VARCHAR(100),
    @Estado VARCHAR(100),
    @CodigoPostal VARCHAR(10),
    @Pais VARCHAR(100)
)
AS
BEGIN
		INSERT INTO Direcciones(Id, Cliente, Tipo, Calle, EntreCalles, Numero, Colonia, Ciudad, Estado, CodigoPostal, Pais)
		VALUES (@Id, @Cliente, @Tipo, @Calle, @EntreCalles,@Numero, @Colonia, @Ciudad, @Estado, @CodigoPostal, @Pais)
END
	
GO

CREATE PROCEDURE ActualizarDireccion
(
	@Id INT,
    @Cliente INT,
    @Tipo INT,
    @Calle VARCHAR(100),
    @EntreCalles VARCHAR(200),
    @Numero VARCHAR(10),
    @Colonia VARCHAR(100),
    @Ciudad VARCHAR(100),
    @Estado VARCHAR(100),
    @CodigoPostal VARCHAR(10),
    @Pais VARCHAR(100)
)
AS
BEGIN
			UPDATE Direccion
			SET 
			Cliente = @Cliente,
			Tipo = @Tipo,
			Calle = @Calle,
			EntreCalles = @EntreCalles,
			Numero = @Numero,
			Colonia = @Colonia,
			Ciudad = @Ciudad,
			Estado = @Estado,
			CodigoPostal = @CodigoPostal,
			Pais = @Pais
			WHERE Id = @Id
END

/*Evelyn*/


/*David*/


