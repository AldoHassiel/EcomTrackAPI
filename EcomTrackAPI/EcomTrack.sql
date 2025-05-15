CREATE DATABASE DB_EcomTrack
USE DB_EcomTrack;

/*  Tablas de referencias
CREATE TABLE Producto(
	ID INT PRIMARY KEY NOT NULL,
	Nombre VARCHAR(50),
	IdCategoria INT,
	Precio DECIMAL(10,2),
	CantidadDisponible INT,
	IdProveedor INT,
	FOREIGN KEY (IdProveedor) REFERENCES Proveedor(ID),
	FOREIGN KEY (IdCategoria) REFERENCES Categoria(ID)
);

CREATE TABLE Proveedor(
	ID INT PRIMARY KEY NOT NULL,
	Nombre VARCHAR(100),
	CorreoElectronico VARCHAR(100),
	Telefono VARCHAR(15),
	FechaRegistro DATETIME DEFAULT GETDATE()
);

CREATE TABLE Transportista(
	ID INT PRIMARY KEY NOT NULL,
	Nombre VARCHAR(100),
	CorreoElectronico VARCHAR(100),
	Telefono VARCHAR(15),
	FechaRegistro DATETIME DEFAULT GETDATE()
);

CREATE TABLE Categoria(
	ID INT PRIMARY KEY NOT NULL,
	Nombre VARCHAR(100),
	Descripcion VARCHAR(200)
);

CREATE TABLE Pedido(
	ID INT PRIMARY KEY NOT NULL,
	IdCliente INT,
	Fecha DATETIME DEFAULT GETDATE(),
	Importe DECIMAL(10,2),
	Estado VARCHAR(30) CHECK (Estado IN ('Pendiente','Procesado','Entregado')),
	FOREIGN KEY (IdCliente) REFERENCES Cliente(ID)
);

CREATE TABLE DetallePedido(
	ID INT PRIMARY KEY NOT NULL,
	IdPedido INT,
	IdProducto INT,
	CantidadSolicitada INT,
	Subtotal DECIMAL(10,2),
	FOREIGN KEY (IdPedido) REFERENCES Pedido(ID),
	FOREIGN KEY (IdProducto) REFERENCES Producto(ID)
);

CREATE TABLE Pago(
	ID INT PRIMARY KEY NOT NULL,
	IdPedido INT,
	Metodo VARCHAR(20) CHECK (Metodo IN ('Efectivo','Tarjeta','Transferencia')),
	Importe DECIMAL(10,2),
	Fecha DATETIME DEFAULT GETDATE(),
	FOREIGN KEY (IdPedido) REFERENCES Pedido(ID)
);
*/

/* Seccion Aldo */
CREATE TABLE Cliente(
	Id INT PRIMARY KEY NOT NULL,
	Nombre VARCHAR(100),
	Correo VARCHAR(100),
	Pwd VARCHAR(100),
	Telefono VARCHAR(15),
	FechaRegistro DATETIME DEFAULT GETDATE()
);

INSERT INTO Cliente(Id, Nombre) values (1, 'Aldo');
SELECT * FROM Cliente
SELECT ISNULL(MAX(Id),0) + 1 FROM Cliente

CREATE TABLE Envio(
	ID INT PRIMARY KEY NOT NULL,
	IdPedido INT,
	Estado VARCHAR(20) CHECK (Estado IN ('Pendiente','En camino','Entregado')),
	FechaEnvio DATETIME DEFAULT GETDATE(),
	IdTransportista INT,
	FOREIGN KEY (IdPedido) REFERENCES Pedido(ID),
	FOREIGN KEY (IdTransportista) REFERENCES Transportista(ID)
);
SELECT * FROM Cliente
EXECUTE GuardarCliente @Id = 0, @Nombre = 'Aldo Hassiel', @Correo = 'aldo@gmail.com', @Pwd = '1421', @Telefono ='6681461721'

CREATE PROCEDURE GuardarCliente(
	@Id INT,
	@Nombre VARCHAR(100),
	@Correo VARCHAR(100),
	@Pwd VARCHAR(100),
	@Telefono VARCHAR(15)
)
AS
BEGIN
	IF @Id = 0
	BEGIN
		DECLARE @NuevoId INT
		SELECT @NuevoId = ISNULL(MAX(Id),0) + 1 FROM Cliente

		INSERT INTO Cliente(Id, Nombre, Correo, Pwd,Telefono, FechaRegistro)
		VALUES (@NuevoId, @Nombre, @Correo, @Pwd,@Telefono, GETDATE())
	END
	ELSE
	BEGIN
		UPDATE Cliente
		SET 
		Nombre = @Nombre, 
		Correo = @Correo, 
		Telefono = @Telefono
		WHERE Id = @Id
	END
END




/* Seccion Yuriana */







/* Seccion Evelyn */
/*editar, guardar, borrar*/

create table Categoria (
	Id int primary key not null,
	Nombre varchar(50),
	Descripcion varchar(100),
	CantidadProducto int,
	Estado bit
)

create procedure GuardarCategoria (
	@Id int, 
	@Nombre varchar(50), 
	@Descripcion varchar(100),
	@CantidadProducto int,
	@Estado bit
)
as
begin
	if @Id = 0
	begin
		declare @NuevoId INT
		select @NuevoId = ISNULL(MAX(Id),0) + 1 FROM Categoria 

		INSERT INTO Categoria(Id, Nombre, Descripcion, CantidadProducto, Estado)
		VALUES (@NuevoId, @Nombre, @Descripcion, @CantidadProducto, @Estado)
	end
	else
	begin
		UPDATE Categoria
		SET 
		Nombre = @Nombre, 
		Descripcion = @Descripcion,
		CantidadProducto = @CantidadProducto,
		Estado = @Estado
		where Id = @Id
	end
end

select * from Categoria
Execute GuardarCategoria @Id = 0, @Nombre = 'audfinos', @Descripcion = 'Telefonos', @CantidadProducto = 0, @Estado = 1


/* Seccion David */



