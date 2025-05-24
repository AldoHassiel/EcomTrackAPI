/* Seccion Aldo */


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
    EstadoD VARCHAR(100),
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
	@EstadoD VARCHAR(100), 
	@CodigoPostal VARCHAR(10),
	@Pais VARCHAR(100)
)
AS
BEGIN
		BEGIN
			INSERT INTO Pedido(Id, Cliente, FechaPedido, Importe, Estado, Calle, EntreCalles, Numero, Colonia, Ciudad, EstadoD, CodigoPostal, Pais)
			VALUES (@Id, @Cliente, GETDATE(), @Importe, @Estado, @Calle, @EntreCalles, @Numero, @Colonia, @Ciudad, @EstadoD, @CodigoPostal, @Pais);
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


