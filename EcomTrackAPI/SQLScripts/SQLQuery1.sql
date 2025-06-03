ALTER PROCEDURE ActualizarEnvio
    @Id INT,
    @IdEstado INT,
    @IdTransportista INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Intentar actualizar
        UPDATE Envio
        SET 
            IdEstado = @IdEstado,
            IdTransportista = @IdTransportista
        WHERE 
            Id = @Id;

        IF @@ROWCOUNT = 0
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'No se encontró un envío con el ID especificado.';
        END
        ELSE
        BEGIN
            SET @Resultado = 1;
            SET @Mensaje = 'El estado y transportista del envío fueron actualizados correctamente.';
        END
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = 'Error al actualizar el envío: ' + ERROR_MESSAGE();
    END CATCH
END