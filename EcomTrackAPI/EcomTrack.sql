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
CREATE TABLE Pedidos(
	Id INT PRIMARY KEY NOT NULL,
	Cliente INT,
	FechaPedido DATETIME DEFAULT GETDATE(),
	Importe FLOAT,
	Estado INT,
	FOREIGN KEY (Cliente) REFERENCES Cliente(Id),
	FOREIGN KEY (Estado) REFERENCES Estado(Id)
)

--VERIFICAR LA LÓGICA
CREATE PROCEDURE RegistrarPedidos(
	@Id INT,
	@Cliente INT,
	@Importe FLOAT,
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
	ImporteAbonado FLOAT,
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
	@ImporteAbonado FLOAT
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
	INSERT INTO Direcciones(Id, Cliente, Tipo, Calle, EntreCalles, Numero, Colonia, Ciudad, Estado, CodigoPostal, Pais)
    VALUES (@Id, @Cliente, @Tipo, @Calle, @EntreCalles,@Numero, @Colonia, @Ciudad, @Estado, @CodigoPostal, @Pais)
END





/* Seccion Evelyn */






/* Seccion David */



