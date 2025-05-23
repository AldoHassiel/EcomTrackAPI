--PROCEDIMIENTO ALMACENADO: ACTUALIZAR ADMINISTRADOR
DECLARE @MensajeError VARCHAR(500)
DECLARE @Resultado BIT

EXECUTE ActualizarAdministrador @Id = 2, @Correo = 'yuri@ecomtrack.com', @Pwd = 'Lavanda', @Estado = 1, @Nombre = 'AnaYuri', 
@Telefono = '6681784512', @MensajeError = @MensajeError OUTPUT, 
@Resultado = @Resultado OUTPUT

SELECT 
    @Resultado AS Resultado,
    @MensajeError AS MensajeError

SELECT * FROM Usuario
SELECT * FROM Administrador

CREATE PROCEDURE ActualizarAdministrador(
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
        -- Verificar si el usuario existe y es administrador
        IF NOT EXISTS (SELECT 1 FROM Usuario WHERE Id = @Id AND Rol = 2)
        BEGIN
            SET @MensajeError = 'Administrador no encontrado';
            SET @Resultado = 0;
            RETURN;
        END
        
        -- Verificar si el nuevo correo ya existe en otro usuario
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
        
        UPDATE Administrador
        SET Nombre = @Nombre,
            Telefono = @Telefono
        WHERE Id = @Id;
        
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