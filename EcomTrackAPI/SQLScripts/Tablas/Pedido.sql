
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