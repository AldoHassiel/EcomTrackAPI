CREATE PROCEDURE ActualizarAdministrador
    @Id INT,
    @Correo VARCHAR(100),
    @Pwd VARCHAR(50),
    @Estado BIT,
    @Nombre VARCHAR(100),
    @Telefono VARCHAR(15),
    @MensajeError VARCHAR(500) OUTPUT,
    @Resultado BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @Resultado = 0;
    SET @MensajeError = '';
    
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Usuario WHERE Id = @Id AND Rol = 2)
        BEGIN
            SET @MensajeError = 'Administrador no encontrado';
            RETURN;
        END
        
        IF EXISTS (SELECT 1 FROM Usuario WHERE Correo = @Correo AND Id <> @Id)
        BEGIN
            SET @MensajeError = 'El correo ya está en uso';
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
        SET @MensajeError = 'Actualizado correctamente';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        SET @MensajeError = ERROR_MESSAGE();
    END CATCH
END;
GO



CREATE PROCEDURE ActualizarCantidadProductoDelCarrito
    @IdCliente INT,
    @IdProducto INT,
    @Cantidad INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF @Cantidad <= 0
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'La cantidad debe ser mayor a 0.';
            RETURN;
        END
        
        DECLARE @StockDisponible INT;
        DECLARE @ProductoNombre VARCHAR(100);
        
        SELECT @StockDisponible = Stock, @ProductoNombre = Nombre
        FROM Producto
        WHERE Id = @IdProducto AND Estado = 1;
        
        IF @ProductoNombre IS NULL
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'Producto no disponible.';
            RETURN;
        END
        
        IF @StockDisponible < @Cantidad
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'Stock insuficiente: ' + CAST(@StockDisponible AS VARCHAR);
            RETURN;
        END
        
        DECLARE @IdCarrito INT;
        DECLARE @CarritoActivo BIT;
        
        SELECT @IdCarrito = Id, @CarritoActivo = Activo
        FROM Carrito
        WHERE Cliente = @IdCliente;
        
        IF @IdCarrito IS NULL
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'Carrito no encontrado.';
            RETURN;
        END
        
        DECLARE @IdDetalle INT;
        DECLARE @CantidadActual INT;
        
        SELECT @IdDetalle = Id, @CantidadActual = Cantidad
        FROM DetalleCarrito
        WHERE Carrito = @IdCarrito AND Producto = @IdProducto;
        
        IF @IdDetalle IS NULL
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'Producto no encontrado en el carrito.';
            RETURN;
        END
        
        IF @Cantidad = @CantidadActual
        BEGIN
            SET @Resultado = 1;
            SET @Mensaje = 'Cantidad no cambiada.';
            RETURN;
        END
        
        UPDATE DetalleCarrito
        SET Cantidad = @Cantidad
        WHERE Id = @IdDetalle;
        
        SET @Resultado = 1;
        IF @Cantidad > @CantidadActual
            SET @Mensaje = 'Cantidad aumentada de ' + CAST(@CantidadActual AS VARCHAR) + ' a ' + CAST(@Cantidad AS VARCHAR);
        ELSE
            SET @Mensaje = 'Cantidad reducida de ' + CAST(@CantidadActual AS VARCHAR) + ' a ' + CAST(@Cantidad AS VARCHAR);
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = 'Error: ' + ERROR_MESSAGE();
    END CATCH
END;
GO



CREATE PROCEDURE ActualizarCategoria
    @Id INT,
    @Nombre VARCHAR(50),
    @Descripcion VARCHAR(100),
    @Estado BIT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Categoria WHERE Id = @Id)
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'Categoría no encontrada.';
            RETURN;
        END

        IF EXISTS (SELECT 1 FROM Categoria WHERE Nombre = @Nombre AND Id <> @Id)
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'Nombre ya en uso.';
            RETURN;
        END

        UPDATE Categoria
        SET Nombre = @Nombre,
            Descripcion = @Descripcion,
            Estado = @Estado
        WHERE Id = @Id;

        SET @Resultado = 1;
        SET @Mensaje = 'Categoría actualizada.';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END;
GO



CREATE PROCEDURE ActualizarCliente
    @Id INT,
    @Correo VARCHAR(100),
    @Pwd VARCHAR(50),
    @Estado BIT,
    @Nombre VARCHAR(100),
    @Telefono VARCHAR(15),
    @MensajeError VARCHAR(500) OUTPUT,
    @Resultado BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @Resultado = 0;
    SET @MensajeError = '';
    
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Usuario WHERE Id = @Id AND Rol = 1)
        BEGIN
            SET @MensajeError = 'Cliente no encontrado';
            RETURN;
        END
        
        IF EXISTS (SELECT 1 FROM Usuario WHERE Correo = @Correo AND Id <> @Id)
        BEGIN
            SET @MensajeError = 'Correo ya en uso';
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
        SET @MensajeError = 'Actualizado correctamente';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        SET @MensajeError = ERROR_MESSAGE();
    END CATCH
END;
GO



CREATE PROCEDURE ActualizarEnvio
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

        SELECT @DescripcionEstado = Descripcion
        FROM Estado
        WHERE Id = @IdEstado;

        SELECT @IdPedido = IdPedido
        FROM Envio
        WHERE Id = @Id;

        IF @IdPedido IS NULL
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'Envío no encontrado.';
            RETURN;
        END

        UPDATE Envio
        SET IdEstado = @IdEstado,
            FechaEntrega = CASE 
                              WHEN @DescripcionEstado = 'Entregado' THEN GETDATE()
                              ELSE FechaEntrega
                           END
        WHERE Id = @Id;

        UPDATE Pedido
        SET IdEstado = @IdEstado
        WHERE Id = @IdPedido;

        SET @Resultado = 1;
        SET @Mensaje = CASE 
            WHEN @DescripcionEstado = 'Entregado' THEN 'Envío entregado.'
            ELSE 'Estado actualizado.'
        END;
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = 'Error: ' + ERROR_MESSAGE();
    END CATCH
END;
GO



CREATE PROCEDURE ActualizarEnvioTransportista
    @Id INT,
    @IdTransportista INT,
    @FechaEnvio DATETIME,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM Envio WHERE Id = @Id)
        BEGIN
            UPDATE Envio
            SET IdTransportista = @IdTransportista,
                FechaEnvio = @FechaEnvio
            WHERE Id = @Id;

            SET @Resultado = 1;
            SET @Mensaje = 'Transportista actualizado.';
        END
        ELSE
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'Envío no encontrado.';
        END
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = 'Error: ' + ERROR_MESSAGE();
    END CATCH
END;
GO



CREATE PROCEDURE ActualizarPedido
    @Id INT,
    @Estado INT
AS
BEGIN
    IF @Estado = 2
    BEGIN
        UPDATE Envio
        SET FechaEnvio = GETDATE()
        WHERE IdPedido = @Id;
    END

    IF @Estado = 1
    BEGIN
        UPDATE Envio
        SET FechaEntrega = GETDATE()
        WHERE IdPedido = @Id;
    END

    UPDATE Pedido
    SET IdEstado = @Estado 
    WHERE Id = @Id;

    UPDATE Envio
    SET IdEstado = @Estado 
    WHERE IdPedido = @Id;
END;
GO



CREATE PROCEDURE ActualizarProducto
    @Id INT,
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(100),
    @Categoria INT,
    @Precio DECIMAL(10,2),
    @Stock INT,
    @Proveedor INT,
    @UrlImagen NVARCHAR(255),
    @Estado BIT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Producto WHERE Id = @Id)
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'Producto no encontrado';
            RETURN;
        END

        IF EXISTS (SELECT 1 FROM Producto WHERE Nombre = @Nombre AND Id <> @Id)
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'Nombre ya en uso';
            RETURN;
        END

        IF NOT EXISTS (SELECT 1 FROM Categoria WHERE Id = @Categoria)
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'Categoría no existe';
            RETURN;
        END

        IF NOT EXISTS (SELECT 1 FROM Proveedor WHERE Id = @Proveedor)
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'Proveedor no existe';
            RETURN;
        END

        UPDATE Producto
        SET Nombre = @Nombre,
            Descripcion = @Descripcion,
            Categoria = @Categoria,
            Precio = @Precio,
            Stock = @Stock,
            Proveedor = @Proveedor,
            UrlImagen = @UrlImagen,
            Estado = @Estado
        WHERE Id = @Id;

        SET @Resultado = 1;
        SET @Mensaje = 'Producto actualizado';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END;
GO



CREATE PROCEDURE ActualizarProveedor
    @Id INT,
    @Nombre VARCHAR(50),
    @Correo VARCHAR(100),
    @Telefono VARCHAR(15),
    @Estado BIT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(200) OUTPUT
AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Proveedor WHERE Id = @Id)
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'Proveedor no existe.';
            RETURN;
        END

        IF EXISTS (SELECT 1 FROM Proveedor WHERE Correo = @Correo AND Id <> @Id)
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'Correo ya en uso.';
            RETURN;
        END

        UPDATE Proveedor
        SET Nombre = @Nombre,
            Correo = @Correo,
            Telefono = @Telefono,
            Estado = @Estado
        WHERE Id = @Id;

        SET @Resultado = 1;
        SET @Mensaje = 'Proveedor actualizado';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END;
GO



CREATE PROCEDURE ActualizarTarjeta
    @Id INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    BEGIN TRY
        DECLARE @ClienteId INT;

        SELECT @ClienteId = Cliente
        FROM Tarjeta
        WHERE Id = @Id;

        IF @ClienteId IS NOT NULL
        BEGIN
            UPDATE Tarjeta
            SET Predeterminada = 0
            WHERE Cliente = @ClienteId;
            
            UPDATE Tarjeta
            SET Predeterminada = 1
            WHERE Id = @Id;

            SET @Resultado = 1;
            SET @Mensaje = 'Tarjeta marcada como predeterminada.';
        END
        ELSE
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'Tarjeta no encontrada.';
        END
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END;
GO



CREATE PROCEDURE ActualizarTransportista
    @Id INT,
    @Nombre VARCHAR(50),
    @Correo VARCHAR(100),
    @Telefono VARCHAR(15),
    @Estado BIT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(200) OUTPUT
AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Transportista WHERE Id = @Id)
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'Transportista no existe.';
            RETURN;
        END

        IF EXISTS (SELECT 1 FROM Transportista WHERE Correo = @Correo AND Id <> @Id)
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'Correo ya en uso.';
            RETURN;
        END

        UPDATE Transportista
        SET Nombre = @Nombre,
            Correo = @Correo,
            Telefono = @Telefono,
            Estado = @Estado
        WHERE Id = @Id;

        SET @Resultado = 1;
        SET @Mensaje = 'Transportista actualizado';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END;
GO



CREATE PROCEDURE AgregarDireccion
    @IdCliente INT,
    @TipoDireccion INT,
    @Calle VARCHAR(100),
    @Numero VARCHAR(10),
    @EntreCalles VARCHAR(200),
    @Colonia VARCHAR(100),
    @Ciudad VARCHAR(100),
    @IdEstado INT,
    @CodigoPostal VARCHAR(10)
AS
BEGIN
    UPDATE Direccion
    SET EsPredeterminado = 0
    WHERE IdCliente = @IdCliente;

    INSERT INTO Direccion (
        IdCliente, TipoDireccion, EsPredeterminado, Calle, Numero, 
        EntreCalles, Colonia, Ciudad, IdEstado, CodigoPostal, EstaActiva
    ) VALUES (
        @IdCliente, @TipoDireccion, 1, @Calle, @Numero, 
        @EntreCalles, @Colonia, @Ciudad, @IdEstado, @CodigoPostal, 1
    );
END;
GO



CREATE PROCEDURE AgregarProductoAlCarrito
    @IdCliente INT,
    @IdProducto INT,
    @Cantidad INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DECLARE @StockDisponible INT;
        DECLARE @PrecioProducto DECIMAL(10, 2);
        DECLARE @ProductoNombre VARCHAR(100);
        
        SELECT @StockDisponible = Stock, @PrecioProducto = Precio, @ProductoNombre = Nombre
        FROM Producto
        WHERE Id = @IdProducto AND Estado = 1;
        
        IF @ProductoNombre IS NULL
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'Producto no disponible.';
            RETURN;
        END
        
        IF @StockDisponible < @Cantidad
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'Stock insuficiente: ' + CAST(@StockDisponible AS VARCHAR);
            RETURN;
        END
        
        DECLARE @IdCarrito INT;
        DECLARE @CarritoActivo BIT;
        
        SELECT @IdCarrito = Id, @CarritoActivo = Activo
        FROM Carrito
        WHERE Cliente = @IdCliente;
        
        IF @IdCarrito IS NULL
        BEGIN
            INSERT INTO Carrito (Cliente, Activo)
            VALUES (@IdCliente, 1);
            
            SET @IdCarrito = SCOPE_IDENTITY();
        END
        ELSE IF @CarritoActivo = 0
        BEGIN
            UPDATE Carrito
            SET Activo = 1
            WHERE Id = @IdCarrito;
        END
        
        DECLARE @IdDetalle INT;
        DECLARE @CantidadActual INT;
        
        SELECT @IdDetalle = Id, @CantidadActual = Cantidad
        FROM DetalleCarrito
        WHERE Carrito = @IdCarrito AND Producto = @IdProducto;
        
        IF @IdDetalle IS NOT NULL
        BEGIN
            UPDATE DetalleCarrito
            SET Cantidad = @CantidadActual + @Cantidad
            WHERE Id = @IdDetalle;
            
            SET @Mensaje = 'Cantidad actualizada: ' + CAST(@Cantidad AS VARCHAR) + ' unidades agregadas.';
        END
        ELSE
        BEGIN
            INSERT INTO DetalleCarrito (Carrito, Producto, Cantidad, Precio)
            VALUES (@IdCarrito, @IdProducto, @Cantidad, @PrecioProducto);
            
            SET @Mensaje = 'Producto agregado al carrito.';
        END
        
        SET @Resultado = 1;
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = 'Error: ' + ERROR_MESSAGE();
    END CATCH
END;
GO



CREATE PROCEDURE ConfirmarCompra
    @IdCliente INT,
    @IdTarjeta INT,
    @IdDireccion INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        DECLARE @IdPedido INT;
        DECLARE @FechaPedido DATETIME = GETDATE();
        DECLARE @FechaPedidoINT INT = dbo.FechaDateToInt(@FechaPedido);
        DECLARE @Total DECIMAL(10, 2);
        DECLARE @CantidadProductos INT;

        IF EXISTS (
            SELECT 1
            FROM DetalleCarrito dc
            INNER JOIN Carrito c ON dc.Carrito = c.Id
            INNER JOIN Producto p ON dc.Producto = p.Id
            WHERE c.Cliente = @IdCliente AND c.Activo = 1
              AND dc.Cantidad > p.Stock
        )
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'Stock insuficiente.';
            RETURN;
        END;

        SELECT 
            @Total = ISNULL(SUM(dc.Subtotal), 0),
            @CantidadProductos = ISNULL(SUM(dc.Cantidad), 0)
        FROM DetalleCarrito dc
        INNER JOIN Carrito c ON dc.Carrito = c.Id
        WHERE c.Cliente = @IdCliente AND c.Activo = 1;

        INSERT INTO Pedido (IdCliente, FechaPedido, FechaPedidoINT, Importe, IdEstado, IdDireccion)
        VALUES (@IdCliente, @FechaPedido, @FechaPedidoINT, @Total, 3, @IdDireccion);

        SET @IdPedido = SCOPE_IDENTITY();

        INSERT INTO DetallePedido (IdPedido, IdProducto, Cantidad, Precio, Subtotal)
        SELECT 
            @IdPedido,
            dc.Producto,
            dc.Cantidad,
            dc.Precio,
            dc.Subtotal
        FROM DetalleCarrito dc
        INNER JOIN Carrito c ON dc.Carrito = c.Id
        WHERE c.Cliente = @IdCliente AND c.Activo = 1;

        DELETE FROM DetalleCarrito
        WHERE Carrito IN (
            SELECT Id FROM Carrito WHERE Cliente = @IdCliente AND Activo = 1
        );

        INSERT INTO Pago(IdPedido, IdTarjeta, ImporteAbonado, FechaPago)
        VALUES (@IdPedido, @IdTarjeta, @Total, @FechaPedido);

        INSERT INTO Envio(FechaEnvio, FechaEntrega, IdPedido, IdTransportista, IdEstado)
        VALUES (NULL, NULL, @IdPedido, NULL, 3);

        SET @Resultado = 1;
        SET @Mensaje = 'Compra confirmada.';
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END;
GO



CREATE PROCEDURE EditarDireccion
    @Id INT,
    @IdCliente INT,
    @TipoDireccion INT,
    @Calle VARCHAR(100),
    @Numero VARCHAR(10),
    @EntreCalles VARCHAR(200),
    @Colonia VARCHAR(100),
    @Ciudad VARCHAR(100),
    @IdEstado INT,
    @CodigoPostal VARCHAR(10),
    @EsPredeterminado BIT
AS
BEGIN
    IF @EsPredeterminado = 1
    BEGIN
        UPDATE Direccion
        SET EsPredeterminado = 0
        WHERE IdCliente = @IdCliente;
    END

    UPDATE Direccion
    SET TipoDireccion = @TipoDireccion,
        Calle = @Calle,
        Numero = @Numero,
        EntreCalles = @EntreCalles,
        Colonia = @Colonia,
        Ciudad = @Ciudad,
        IdEstado = @IdEstado,
        CodigoPostal = @CodigoPostal,
        EsPredeterminado = @EsPredeterminado
    WHERE Id = @Id AND IdCliente = @IdCliente;
END;
GO

CREATE PROCEDURE EliminarAdministrador
    @Id INT,
    @MensajeError VARCHAR(500) OUTPUT,
    @Resultado BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @Resultado = 0;
    SET @MensajeError = '';
    
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Usuario WHERE Id = @Id AND Rol = 2)
        BEGIN
            SET @MensajeError = 'Administrador no encontrado';
            RETURN;
        END

        DELETE FROM Usuario WHERE Id = @Id;
        
        SET @Resultado = 1;
        SET @MensajeError = 'Eliminado correctamente';
    END TRY
    BEGIN CATCH
        SET @MensajeError = ERROR_MESSAGE();
    END CATCH
END;
GO



CREATE PROCEDURE EliminarCategoria
    @Id INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Categoria WHERE Id = @Id)
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'Categoría no existe.';
            RETURN;
        END

        IF EXISTS (SELECT 1 FROM Producto WHERE Categoria = @Id)
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'Categoría tiene productos asociados.';
            RETURN;
        END

        DELETE FROM Categoria WHERE Id = @Id;

        SET @Resultado = 1;
        SET @Mensaje = 'Categoría eliminada.';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END;
GO



CREATE PROCEDURE EliminarCliente
    @Id INT,
    @MensajeError VARCHAR(500) OUTPUT,
    @Resultado BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET @Resultado = 0;
    SET @MensajeError = '';
    
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Usuario WHERE Id = @Id AND Rol = 1)
        BEGIN
            SET @MensajeError = 'Cliente no encontrado';
            RETURN;
        END
        
        BEGIN TRANSACTION;
        
        DELETE FROM Usuario WHERE Id = @Id;
        
        COMMIT TRANSACTION;
        SET @Resultado = 1;
        SET @MensajeError = 'Eliminado correctamente';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        SET @MensajeError = ERROR_MESSAGE();
    END CATCH
END;
GO


CREATE PROCEDURE EliminarDireccion
    @Id INT,
    @IdCliente INT
AS
BEGIN
    UPDATE Direccion
    SET EstaActiva = 0
    WHERE Id = @Id AND IdCliente = @IdCliente;
END;
GO



CREATE PROCEDURE EliminarPedidoCompleto
    @IdPedido INT
AS
BEGIN
    DELETE FROM DetallePedido WHERE IdPedido = @IdPedido;

    DELETE FROM Envio WHERE IdPedido = @IdPedido;

    DELETE FROM Pago WHERE IdPedido = @IdPedido;

    DELETE FROM Pedido WHERE Id = @IdPedido;
END;
GO


CREATE PROCEDURE EliminarProducto
    @Id INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Producto WHERE Id = @Id)
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'Producto no existe';
            RETURN;
        END

        DELETE FROM Producto WHERE Id = @Id;

        SET @Resultado = 1;
        SET @Mensaje = 'Producto eliminado';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END;
GO


CREATE PROCEDURE EliminarProductoDelCarrito
    @IdCliente INT,
    @IdProducto INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DECLARE @IdCarrito INT;
        
        SELECT @IdCarrito = Id
        FROM Carrito
        WHERE Cliente = @IdCliente;
        
        IF @IdCarrito IS NULL
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'Carrito no encontrado.';
            RETURN;
        END
        
        DECLARE @IdDetalle INT;
        DECLARE @ProductoNombre VARCHAR(100);
        
        SELECT @IdDetalle = d.Id, @ProductoNombre = p.Nombre
        FROM DetalleCarrito d
        INNER JOIN Producto p ON d.Producto = p.Id
        WHERE d.Carrito = @IdCarrito AND d.Producto = @IdProducto;
        
        IF @IdDetalle IS NULL
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'Producto no encontrado en el carrito.';
            RETURN;
        END
        
        DELETE FROM DetalleCarrito
        WHERE Id = @IdDetalle;
        
        IF NOT EXISTS (SELECT 1 FROM DetalleCarrito WHERE Carrito = @IdCarrito)
        BEGIN
            UPDATE Carrito
            SET Activo = 0
            WHERE Id = @IdCarrito;
            
            SET @Mensaje = 'Carrito vaciado.';
        END
        ELSE
        BEGIN
            SET @Mensaje = 'Producto eliminado.';
        END
        
        SET @Resultado = 1;
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = 'Error: ' + ERROR_MESSAGE();
    END CATCH
END;
GO


CREATE PROCEDURE EliminarProveedor
    @Id INT,
    @Resultado BIT OUTPUT,
    @Mensaje NVARCHAR(200) OUTPUT
AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Proveedor WHERE Id = @Id)
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'Proveedor no existe.';
            RETURN;
        END

        IF EXISTS (SELECT 1 FROM Producto WHERE Proveedor = @Id)
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'Proveedor tiene productos asociados.';
            RETURN;
        END

        DELETE FROM Proveedor WHERE Id = @Id;

        SET @Resultado = 1;
        SET @Mensaje = 'Proveedor eliminado.';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END;
GO


CREATE PROCEDURE EliminarTarjeta
    @Id INT, 
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN