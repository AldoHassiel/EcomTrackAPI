DECLARE @MensajeError VARCHAR(500)
DECLARE @Resultado BIT

EXECUTE EliminarAdministrador @Id = 2, @MensajeError = @MensajeError OUTPUT, @Resultado = @Resultado OUTPUT

SELECT 
    @Resultado AS Resultado,
    @MensajeError AS MensajeError

SELECT * FROM Usuario
SELECT * FROM Cliente
SELECT * FROM Administrador

CREATE PROCEDURE EliminarAdministrador(
    @Id INT,
    @MensajeError VARCHAR(500) OUTPUT,
    @Resultado BIT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    SET @Resultado = 0;
    SET @MensajeError = '';
    
    BEGIN TRY
        -- Verifica si el usuario existe y es administrador
        IF NOT EXISTS (SELECT 1 FROM Usuario WHERE Id = @Id AND Rol = 2)
        BEGIN
            SET @MensajeError = 'Administrador no encontrado';
            SET @Resultado = 0;
            RETURN;
        END

        DELETE FROM Usuario WHERE Id = @Id;
        
        SET @Resultado = 1;
    END TRY
    BEGIN CATCH
        SET @MensajeError = ERROR_MESSAGE();
        SET @Resultado = 0;
    END CATCH
END;