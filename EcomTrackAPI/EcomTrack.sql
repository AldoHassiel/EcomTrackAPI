CREATE DATABASE DB_EcomTrack
USE DB_EcomTrack;

CREATE TABLE Cliente(
	ID INT PRIMARY KEY NOT NULL,
	Nombre VARCHAR(100),
	CorreoElectronico VARCHAR(100),
	Telefono VARCHAR(15),
	Direccion VARCHAR(200),
	FechaRegistro DATETIME DEFAULT GETDATE()
);

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

CREATE PROCEDURE GuardarCliente(
	@Id INT,
	@Nombre VARCHAR(100),
	@CorreoElectronico VARCHAR(100),
	@Telefono VARCHAR(15),
	@Direccion VARCHAR(200)
)
AS
BEGIN
	IF @Id = 0
	BEGIN
		DECLARE @NuevoId INT
		SELECT @NuevoId = ISNULL(MAX(Id),0) + 1 FROM Cliente

		INSERT INTO Cliente(ID, Nombre, CorreoElectronico, Telefono, Direccion, FechaRegistro)
		VALUES (@NuevoId, @Nombre, @CorreoElectronico, @Telefono, @Direccion, GETDATE())
	END
	ELSE
	BEGIN
		UPDATE Cliente
		SET 
		Nombre = @Nombre, 
		CorreoElectronico = @CorreoElectronico, 
		Telefono = @Telefono,
		Direccion = @Direccion
		WHERE ID = @Id
	END
END

CREATE PROCEDURE 