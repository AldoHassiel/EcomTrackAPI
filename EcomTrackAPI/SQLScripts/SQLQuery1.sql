ALTER PROCEDURE ActualizarEnvio
    @Id INT,
    @IdEstado INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        DECLARE @DescripcionEstado VARCHAR(100);
        DECLARE @IdPedido INT;

        -- Obtener la descripción del estado
        SELECT @DescripcionEstado = Descripcion
        FROM Estado
        WHERE Id = @IdEstado;

        -- Obtener el IdPedido del envío
        SELECT @IdPedido = IdPedido
        FROM Envio
        WHERE Id = @Id;

        -- Validar si el envío existe
        IF @IdPedido IS NULL
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'No se encontró un envío con el ID especificado.';
            RETURN;
        END

        -- Actualizar el estado del envío (y FechaEntrega si aplica)
        UPDATE Envio
        SET 
            IdEstado = @IdEstado,
            FechaEntrega = CASE 
                              WHEN @DescripcionEstado = 'Entregado' THEN GETDATE()
                              ELSE FechaEntrega
                           END
        WHERE Id = @Id;

        -- Actualizar el estado del pedido relacionado
        UPDATE Pedido
        SET IdEstado = @IdEstado
        WHERE Id = @IdPedido;

        SET @Resultado = 1;
        SET @Mensaje = 
            CASE 
                WHEN @DescripcionEstado = 'Entregado' THEN 'Estado actualizado y fecha de entrega registrada. También se actualizó el estado del pedido.'
                ELSE 'Estado del envío y del pedido actualizados correctamente.'
            END;
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = 'Error al actualizar el envío o el pedido: ' + ERROR_MESSAGE();
    END CATCH
END;

select * from Pedido