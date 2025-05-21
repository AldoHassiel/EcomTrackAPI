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
/*falta borrar*/

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

go

create trigger ActualizarCantidadProducto
	on Producto
	after insert
	as
	begin
		update Categoria
		set CantidadProducto = (
			select count(*) 
			from Producto 
			where Categoria = i.Categoria
		)
		from Categoria c
		join inserted i on c.Id = i.Categoria;
end;


select * from proveedor

execute GuardarProducto @Id = 0, @Nombre = 'earpods', @Descripcion = 'earpods', @Categoria = 2, @Precio = 5, @Stock = 0, @Proveedor = 1

select * from producto

select * from Categoria

Execute GuardarCategoria @Id = 0, @Nombre = 'audfinos', @Descripcion = 'Telefonos', @CantidadProducto = 0, @Estado = 1

create table Proveedor (
	Id int primary key not null,
	Nombre varchar(50),
	Correo varchar(100),
	Telefono varchar(15),
	Estado bit
)

create procedure GuardarProveedor (
	@Id int, 
	@Nombre varchar(50), 
	@Correo varchar(100),
	@Telefono varchar(15),
	@Estado bit
)
as
begin
	if @Id = 0
	begin
		declare @NuevoId INT
		select @NuevoId = ISNULL(MAX(Id),0) + 1 FROM Proveedor

		INSERT INTO Proveedor(Id, Nombre, Correo, Telefono, Estado)
		VALUES (@NuevoId, @Nombre, @Correo, @Telefono, @Estado)
	end
	else
	begin
		UPDATE Proveedor
		SET 
		Nombre = @Nombre, 
		Correo = @Correo,
		Telefono = @Telefono,
		Estado = @Estado
		where Id = @Id
	end
end

execute GuardarProveedor @Id = 0, @Nombre = 'Apple', @Correo = '@gmail.com', @Telefono = '666666666', @Estado = 1

select * from proveedor

create table Envio (
	Id int primary key not null,
	FechaEnvio datetime default getdate(),
	IdPedido int,
	IdTransportista int, 
	IdEstado int,
	foreign key (IdPedido) references Pedido(Id),
	foreign key (IdTransportista) references Transportista(Id),
	foreign key (IdEstado) references Estado(Id)
)

create procedure GuardarEnvio (
	@Id int, 
	@IdPedido int,
	@IdTransportista int,
	@IdEstado int
)
as
begin
	if @Id = 0
	begin
		declare @NuevoId INT
		select @NuevoId = ISNULL(MAX(Id),0) + 1 FROM Envio

		INSERT INTO Envio(Id, IdPedido, IdTransportista, IdEstado)
		VALUES (@NuevoId, @IdPedido, @IdTransportista, @IdEstado)
	end
	else
	begin
		UPDATE Envio
		SET 
		IdEstado = @IdEstado
		where Id = @Id
	end
end


/*Sección Eve actualizado */

create table Categoria (
	Id int identity primary key not null,
	Nombre varchar(50),
	Descripcion varchar(100),
	CantidadProducto int,
	Estado bit
)
go
create procedure GuardarCategoria (
	@Id int, 
	@Nombre varchar(50), 
	@Descripcion varchar(100),
	@CantidadProducto int,
	@Estado bit
)
as
begin
	begin
		INSERT INTO Categoria(Nombre, Descripcion, CantidadProducto, Estado)
		VALUES (@Nombre, @Descripcion, @CantidadProducto, @Estado)
	end
end

go

create procedure ActualizarCategoría (
	@Id int, 
	@Nombre varchar(50), 
	@Descripcion varchar(100),
	@CantidadProducto int,
	@Estado bit
)
as
	begin
		UPDATE Categoria
		SET 
		Nombre = @Nombre, 
		Descripcion = @Descripcion,
		CantidadProducto = @CantidadProducto,
		Estado = @Estado
		where Id = @Id
	end

go

create trigger ActualizarCantidadProducto
	on Producto
	after insert
	as
	begin
		update Categoria
		set CantidadProducto = (
			select count(*) 
			from Producto 
			where Categoria = i.Categoria
		)
		from Categoria c
		join inserted i on c.Id = i.Categoria;
end;


select * from proveedor

execute GuardarProducto @Id = 0, @Nombre = 'earpods', @Descripcion = 'earpods', @Categoria = 2, @Precio = 5, @Stock = 0, @Proveedor = 1

select * from producto

select * from Categoria

Execute GuardarCategoria @Id = 0, @Nombre = 'audfinos', @Descripcion = 'Telefonos', @CantidadProducto = 0, @Estado = 1


create table Proveedor (
	Id int identity primary key not null,
	Nombre varchar(50),
	Correo varchar(100),
	Telefono varchar(15),
	Estado bit
)

create procedure GuardarProveedor (
	@Id int, 
	@Nombre varchar(50), 
	@Correo varchar(100),
	@Telefono varchar(15),
	@Estado bit
)
as
begin
	begin
		INSERT INTO Proveedor(Nombre, Correo, Telefono, Estado)
		VALUES (@Nombre, @Correo, @Telefono, @Estado)
	end
end

create procedure ActualizarProveedor(
	@Id int,
	@Nombre varchar(50),
	@Correo varchar(100),
	@Telefono varchar(15),
	@Estado bit
)
as
	begin
		UPDATE Proveedor
		SET 
		Nombre = @Nombre, 
		Correo = @Correo,
		Telefono = @Telefono,
		Estado = @Estado
		where Id = @Id
	end

execute GuardarProveedor @Id = 0, @Nombre = 'Apple', @Correo = '@gmail.com', @Telefono = '666666666', @Estado = 1

select * from proveedor

create table Envio (
	Id int identity primary key not null,
	FechaEnvio datetime default getdate(),
	IdPedido int,
	IdTransportista int, 
	IdEstado int,
	foreign key (IdPedido) references Pedido(Id),
	foreign key (IdTransportista) references Transportista(Id),
	foreign key (IdEstado) references Estado(Id)
)

create procedure GuardarEnvio (
	@Id int, 
	@IdPedido int,
	@IdTransportista int,
	@IdEstado int
)
as
begin
	begin
		INSERT INTO Envio(IdPedido, IdTransportista, IdEstado)
		VALUES (@IdPedido, @IdTransportista, @IdEstado)
	end
end


create procedure ActualizarEnvio(
	@Id int, 
	@IdEstado int
)
as
	begin
		UPDATE Envio
		SET 
		IdEstado = @IdEstado
		where Id = @Id
end














/* Seccion David */
CREATE TABLE Estado(
	Id int PRIMARY KEY NOT NULL,
	Descripcion varchar(20)
);

CREATE TABLE MetodoPago (
	Id int PRIMARY KEY NOT NULL,
	Descripcion varchar(20)
);
CREATE TABLE TipoDireccion (
	Id int PRIMARY KEY NOT NULL,
	Descripcion varchar(20)
);
CREATE TABLE Transportista (
	Id INT PRIMARY KEY NOT NULL,
	Nombre VARCHAR(100),
	Correo VARCHAR(100),
	Telefono VARCHAR(15),
);



