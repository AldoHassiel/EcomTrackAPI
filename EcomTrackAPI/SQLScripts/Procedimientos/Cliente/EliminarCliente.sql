DECLARE @MensajeError VARCHAR(500)
DECLARE @Resultado BIT

EXECUTE EliminarCliente @Id = 1, @MensajeError = @MensajeError OUTPUT, @Resultado = @Resultado OUTPUT

SELECT 
    @Resultado AS Resultado,
    @MensajeError AS MensajeError

SELECT * FROM Usuario
SELECT * FROM Cliente

CREATE PROCEDURE EliminarCliente(
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
        -- Verificar si el usuario existe y es cliente
        IF NOT EXISTS (SELECT 1 FROM Usuario WHERE Id = @Id AND Rol = 1)
        BEGIN
            SET @MensajeError = 'Cliente no encontrado';
            SET @Resultado = 0;
            RETURN;
        END
        
        BEGIN TRANSACTION;
        
        DELETE FROM Usuario WHERE Id = @Id;
        
        COMMIT TRANSACTION;
        SET @Resultado = 1;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        SET @MensajeError = ERROR_MESSAGE();
        SET @Resultado = 0;
    END CATCH
END;