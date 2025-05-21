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

/* Seccion Yuriana */
CREATE TABLE Pedido(
	Id INT IDENTITY PRIMARY KEY NOT NULL,
	Cliente INT,
	FechaPedido DATETIME DEFAULT GETDATE(),
	Importe DECIMAL(10,2),
	Estado INT,
	FOREIGN KEY (Cliente) REFERENCES Cliente(Id),
	FOREIGN KEY (Estado) REFERENCES Estado(Id)
)

--VERIFICAR LA LÓGICA
CREATE PROCEDURE RegistrarPedido(
	@Id INT,
	@Cliente INT,
	@Importe DECIMAL(10,2),
    @Estado INT
)
AS
BEGIN
	IF(@Id = 0)
		BEGIN
			DECLARE @NuevoId INT
			SELECT @NuevoId = ISNULL(MAX(Id), 0) + 1 FROM Pedidos
			INSERT INTO Pedidos(Id, Cliente, FechaPedido, Importe, Estado)
			VALUES (@NuevoId, @Cliente, GETDATE(), @Importe, @Estado);
		END

	ELSE
		BEGIN
			UPDATE Pedidos
			SET Estado = @Estado 
			WHERE Id = @Id

		END
END

CREATE TABLE Pago(
	Id INT PRIMARY KEY NOT NULL,
	Pedido INT,
	MetodoPago INT,
	ImporteAbonado DECIMAL(10,2),
	FechaPago DATETIME DEFAULT GETDATE(),
	FOREIGN KEY (Pedido) REFERENCES Pedidos(Id),
	FOREIGN KEY (MetodoPago) REFERENCES MetodoPago(Id),

)

--PRGUNTAR SI EL ID LO PUEDO CAMBIAR A IDENTITY EN LA TABLA DE PAGOS
--CHECAR QUE PASA CON LOS PEDIDOS DEVUELTOS
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

--TABLA DIRECCIONES
CREATE TABLE Direcciones(
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
    FOREIGN KEY (Cliente) REFERENCES Cliente(Id)
);
--Corroborar si va ser posible modificar la direccion que tiene registrada o si mejor la tomamos como otra direccion para la base de datos
--Mi propuesta: darle al usuario unicamente la opcion de añadir una direccion (no permitir modificar la que tiene) y eliminar las que quiera
--pero se lo añadimos como campo a la tabla direccion (habilitada/deshabilitada)

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
	BEGIN
	IF(@Id = 0)
		BEGIN
			DECLARE @NuevoId INT
			SELECT @NuevoId = ISNULL(MAX(Id), 0) + 1 FROM Direcciones
			INSERT INTO Direcciones(Id, Cliente, Tipo, Calle, EntreCalles, Numero, Colonia, Ciudad, Estado, CodigoPostal, Pais)
			VALUES (@Id, @Cliente, @Tipo, @Calle, @EntreCalles,@Numero, @Colonia, @Ciudad, @Estado, @CodigoPostal, @Pais)
		END

	ELSE 
		BEGIN
			UPDATE Direcciones
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
	END
END
	

/*Evelyn*/


/*David*/


