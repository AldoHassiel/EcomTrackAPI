/* Seccion Aldo */


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


