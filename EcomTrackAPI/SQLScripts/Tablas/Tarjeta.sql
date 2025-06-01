--Tarjetas FALTAN LOS PROCEDIMIENTOS DE ALMACENADO NO ESTÁ EJECUTADA
CREATE TABLE Tarjeta(
	Id INT IDENTITY PRIMARY KEY NOT NULL,
	Cliente INT,
	NumeroTarjeta varchar(16),
	MetodoDePago INT,
	MarcaTarjeta varchar(100),
	Activa bit,
	Predeterminada bit,
	FOREIGN KEY (Cliente) REFERENCES Cliente(Id),
	FOREIGN KEY (MetodoDePago) REFERENCES MetodoPago(Id)
)

GO 

CREATE PROCEDURE GuardarTarjeta (
    @Cliente INT, 
    @NumeroTarjeta VARCHAR(16),
    @MetodoDePago INT,
	@MarcaTarjeta varchar(100),
	@Activa bit,
	@Predeterminada bit,
    @Resultado BIT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    BEGIN TRY
        INSERT INTO Tarjeta (Cliente, NumeroTarjeta, MetodoDePago, MarcaTarjeta, Activa, Predeterminada)
        VALUES ( @Cliente, @NumeroTarjeta, @MetodoDePago, @MarcaTarjeta, @Activa, @Predeterminada)

        SET @Resultado = 1
        SET @Mensaje = 'Tarjeta guardada.'
    END TRY
    BEGIN CATCH
        SET @Resultado = 0
        SET @Mensaje = ERROR_MESSAGE()
    END CATCH
END

GO
CREATE PROCEDURE EliminarTarjeta (
    @Id INT, 
    @Resultado BIT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    BEGIN TRY
        UPDATE Tarjeta
        SET Activa = 0
        WHERE Id = @Id

        SET @Resultado = 1
        SET @Mensaje = 'Tarjeta eliminada correctamente.'
    END TRY
    BEGIN CATCH
        SET @Resultado = 0
        SET @Mensaje = ERROR_MESSAGE()
    END CATCH
END


GO
CREATE PROCEDURE ActualizarTarjeta (
    @Id INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    BEGIN TRY
        DECLARE @ClienteId INT

        -- Verificar si existe la tarjeta y obtener el Cliente
        SELECT @ClienteId = Cliente
        FROM Tarjeta
        WHERE Id = @Id

        IF @ClienteId IS NOT NULL
        BEGIN
            -- Desactivar otras tarjetas predeterminadas del mismo cliente
            UPDATE Tarjeta
            SET Predeterminada = 0
            WHERE Cliente = @ClienteId

            -- Activar la nueva tarjeta como predeterminada
            UPDATE Tarjeta
            SET Predeterminada = 1
            WHERE Id = @Id

            SET @Resultado = 1
            SET @Mensaje = 'Tarjeta marcada como predeterminada.'
        END
        ELSE
        BEGIN
            SET @Resultado = 0
            SET @Mensaje = 'No se encontró una tarjeta con el Id proporcionado.'
        END
    END TRY
    BEGIN CATCH
        SET @Resultado = 0
        SET @Mensaje = ERROR_MESSAGE()
    END CATCH
END