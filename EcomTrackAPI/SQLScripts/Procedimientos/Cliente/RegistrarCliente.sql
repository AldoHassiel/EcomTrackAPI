-- PROCEDIMIENTO ALMACENADO: REGISTRAR CLIENTE
DECLARE @NuevoId INT
DECLARE @MensajeError VARCHAR(500)
DECLARE @Resultado BIT

EXECUTE RegistrarCliente @Correo = 'laura.torres@ejemplo.com', @Pwd = 'qwerty', @Nombre = 'Laura Torres', 
@Telefono = '6681457812', @NuevoId = @NuevoId OUTPUT, @MensajeError = @MensajeError OUTPUT, 
@Resultado = @Resultado OUTPUT

SELECT 
    @Resultado AS Resultado,
    @NuevoId AS NuevoId,
    @MensajeError AS MensajeError

SELECT * FROM Usuario
SELECT * FROM Cliente


CREATE PROCEDURE RegistrarCliente(
    @Correo VARCHAR(100),
    @Pwd VARCHAR(50),
    @Nombre VARCHAR(100),
    @Telefono VARCHAR(15),
    @NuevoId INT OUTPUT,
    @MensajeError VARCHAR(500) OUTPUT,
    @Resultado BIT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    SET @Resultado = 0;
    SET @MensajeError = '';
    SET @NuevoId = 0;
    
    BEGIN TRY
        -- Verificar si el correo ya existe
        IF EXISTS (SELECT 1 FROM Usuario WHERE Correo = @Correo)
        BEGIN
            SET @MensajeError = 'El correo electrónico ya está registrado';
            SET @Resultado = 0;
            RETURN;
        END
        
        BEGIN TRANSACTION;
        
        INSERT INTO Usuario(Correo, Pwd, Rol, Estado)
        VALUES (@Correo, @Pwd, 1, 1);
        
        SET @NuevoId = SCOPE_IDENTITY();
        
        INSERT INTO Cliente(Id, Nombre, Telefono)
        VALUES (@NuevoId, @Nombre, @Telefono);
        
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