-- ACTUALIZAR CLIENTE
DECLARE @MensajeError VARCHAR(500)
DECLARE @Resultado BIT

EXECUTE ActualizarCliente @Id = 7, @Correo = 'laura.torres@ejemplo.com', @Pwd = 'qwerty', @Estado = 1, @Nombre = 'Laura Torres', 
@Telefono = '6681457812', @MensajeError = @MensajeError OUTPUT, 
@Resultado = @Resultado OUTPUT

SELECT 
    @Resultado AS Resultado,
    @MensajeError AS MensajeError

SELECT * FROM Usuario
SELECT * FROM Cliente

CREATE PROCEDURE ActualizarCliente(
    @Id INT,
    @Correo VARCHAR(100),
    @Pwd VARCHAR(50),
    @Estado BIT,
    @Nombre VARCHAR(100),
    @Telefono VARCHAR(15),
    @MensajeError VARCHAR(500) OUTPUT,
    @Resultado BIT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    SET @Resultado = 0;
    SET @MensajeError = '';
    
    BEGIN TRY
        -- Verifica si el usuario existe y es cliente
        IF NOT EXISTS (SELECT 1 FROM Usuario WHERE Id = @Id AND Rol = 1)
        BEGIN
            SET @MensajeError = 'Cliente no encontrado';
            SET @Resultado = 0;
            RETURN;
        END
        
        -- Verifica si el nuevo correo ya existe en otro usuario
        IF EXISTS (SELECT 1 FROM Usuario WHERE Correo = @Correo AND Id <> @Id)
        BEGIN
            SET @MensajeError = 'El correo electrónico ya está en uso por otro usuario';
            SET @Resultado = 0;
            RETURN;
        END
        
        BEGIN TRANSACTION;
        
        UPDATE Usuario
        SET Correo = @Correo,
            Pwd = @Pwd,
            Estado = @Estado
        WHERE Id = @Id;
        
        UPDATE Cliente
        SET Nombre = @Nombre,
            Telefono = @Telefono
        WHERE Id = @Id;
        
        COMMIT TRANSACTION;
        SET @Resultado = 1;
		SET @MensajeError = 'Actualizado correctamente'
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        SET @MensajeError = ERROR_MESSAGE();
        SET @Resultado = 0;
    END CATCH
END;