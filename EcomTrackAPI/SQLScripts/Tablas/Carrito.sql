SELECT * FROM Carrito
SELECT * FROM DetalleCarrito


CREATE TABLE Carrito (
	Id INT IDENTITY PRIMARY KEY,
	Cliente INT NOT NULL,
	Activo BIT DEFAULT 1,
	FOREIGN KEY (Cliente) REFERENCES Cliente(Id)
)

CREATE TABLE DetalleCarrito(
	Id INT IDENTITY PRIMARY KEY,
	Carrito INT NOT NULL,
	Producto INT NOT NULL,
	Cantidad INT NOT NULL,
	Precio DECIMAL(10, 2) NOT NULL,
	Subtotal AS (Cantidad * Precio) PERSISTED,
	FOREIGN KEY (Carrito) REFERENCES Carrito(Id),
	FOREIGN KEY (Producto) REFERENCES Producto(Id)
)

ALTER PROCEDURE AgregarProductoAlCarrito
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
            SET @Mensaje = 'El producto no existe o no está disponible.';
            RETURN;
        END
        
        IF @StockDisponible < @Cantidad
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'Stock insuficiente. Disponible: ' + CAST(@StockDisponible AS VARCHAR);
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
            
            SET @Mensaje = 'Se agregaron ' + CAST(@Cantidad AS VARCHAR) + ' unidades más del producto "' + @ProductoNombre + '" al carrito.';
        END
        ELSE
        BEGIN
            INSERT INTO DetalleCarrito (Carrito, Producto, Cantidad, Precio)
            VALUES (@IdCarrito, @IdProducto, @Cantidad, @PrecioProducto);
            
            SET @Mensaje = 'Producto ''' + @ProductoNombre + ''' agregado al carrito correctamente.';
        END
        
        SET @Resultado = 1;
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = 'Error al agregar producto al carrito: ' + ERROR_MESSAGE();
    END CATCH
END

ALTER PROCEDURE ActualizarCantidadProductoDelCarrito
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
            SET @Mensaje = 'El producto no existe o no está disponible.';
            RETURN;
        END
        
        IF @StockDisponible < @Cantidad
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'Stock insuficiente. Disponible: ' + CAST(@StockDisponible AS VARCHAR);
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
            SET @Mensaje = 'No se encontró un carrito para el cliente.';
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
            SET @Mensaje = 'El producto ''' + @ProductoNombre + ''' no se encuentra en el carrito.';
            RETURN;
        END
        
        IF @Cantidad = @CantidadActual
        BEGIN
            SET @Resultado = 1;
            SET @Mensaje = 'La cantidad del producto ''' + @ProductoNombre + ''' ya es ' + CAST(@Cantidad AS VARCHAR) + '.';
            RETURN;
        END
        
        UPDATE DetalleCarrito
        SET Cantidad = @Cantidad
        WHERE Id = @IdDetalle;
        
        SET @Resultado = 1;
        IF @Cantidad > @CantidadActual
            SET @Mensaje = 'Cantidad del producto ''' + @ProductoNombre + ''' aumentada de ' + CAST(@CantidadActual AS VARCHAR) + ' a ' + CAST(@Cantidad AS VARCHAR) + '.';
        ELSE
            SET @Mensaje = 'Cantidad del producto ''' + @ProductoNombre + ''' reducida de ' + CAST(@CantidadActual AS VARCHAR) + ' a ' + CAST(@Cantidad AS VARCHAR) + '.';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = 'Error al actualizar la cantidad del producto: ' + ERROR_MESSAGE();
    END CATCH
END

ALTER PROCEDURE EliminarProductoDelCarrito
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
            SET @Mensaje = 'No se encontró un carrito para el cliente.';
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
            SET @Mensaje = 'El producto no se encuentra en el carrito.';
            RETURN;
        END
        
        DELETE FROM DetalleCarrito
        WHERE Id = @IdDetalle;
        
        IF NOT EXISTS (SELECT 1 FROM DetalleCarrito WHERE Carrito = @IdCarrito)
        BEGIN

            UPDATE Carrito
            SET Activo = 0
            WHERE Id = @IdCarrito;
            
            SET @Mensaje = 'Producto ''' + @ProductoNombre + ''' eliminado del carrito. El carrito ahora está vacío.';
        END
        ELSE
        BEGIN
            SET @Mensaje = 'Producto ''' + @ProductoNombre + ''' eliminado del carrito correctamente.';
        END
        
        SET @Resultado = 1;
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = 'Error al eliminar producto del carrito: ' + ERROR_MESSAGE();
    END CATCH
END

ALTER PROCEDURE ObtenerCarrito
    @IdCliente INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

        DECLARE @IdCarrito INT;
        DECLARE @CarritoActivo BIT;
        
        SELECT @IdCarrito = Id, @CarritoActivo = Activo
        FROM Carrito
        WHERE Cliente = @IdCliente;
        

        IF @IdCarrito IS NULL OR @CarritoActivo = 0
        BEGIN
            SET @Resultado = 1;
            SET @Mensaje = 'El carrito de compra actualmente está vacío.';
            
            SELECT 
                0 AS Total,
                0 AS CantidadProductos,
                1 AS EstaVacio;
            
            SELECT 
                0 AS IdDetalle,
                0 AS IdProducto,
                '' AS ProductoNombre,
                0 AS Cantidad,
                0 AS PrecioUnitario,
                0 AS Subtotal,
                '' AS UrlImagen
            WHERE 1 = 0;
            
            RETURN;
        END
        
        DECLARE @Total DECIMAL(10, 2) = 0;
        DECLARE @CantidadProductos INT = 0;
        
        SELECT 
            @Total = ISNULL(SUM(Subtotal), 0),
            @CantidadProductos = ISNULL(SUM(Cantidad), 0)
        FROM DetalleCarrito
        WHERE Carrito = @IdCarrito;
        
        SET @Resultado = 1;
        SET @Mensaje = 'Carrito obtenido correctamente.';
        
        SELECT 
            @Total AS Total,
            @CantidadProductos AS CantidadProductos,
            CASE WHEN @CantidadProductos = 0 THEN 1 ELSE 0 END AS EstaVacio;
        
        SELECT 
            d.Id AS IdDetalle,
            p.Id AS IdProducto,
            p.Nombre AS ProductoNombre,
            d.Cantidad,
            d.Precio AS PrecioUnitario,
            d.Subtotal,
            p.UrlImagen
        FROM DetalleCarrito d
        INNER JOIN Producto p ON d.Producto = p.Id
        WHERE d.Carrito = @IdCarrito
        ORDER BY p.Nombre;
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = 'Error al obtener el carrito: ' + ERROR_MESSAGE();
    END CATCH
END

ALTER TRIGGER ActualizarPreciosCarrito
ON Producto
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    IF UPDATE(Precio)
    BEGIN
        UPDATE dc
        SET Precio = i.Precio
        FROM DetalleCarrito dc
        INNER JOIN inserted i ON dc.Producto = i.Id
        INNER JOIN Carrito c ON dc.Carrito = c.Id
        WHERE c.Activo = 1;
    END
END

CREATE PROCEDURE ConfirmarCarrito
    @IdCliente INT,
    @IdDireccion INT,
    @IdMetodoPago INT,
    @IdTransportista INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        -- Variables para el procesamiento
        DECLARE @IdCarrito INT;
        DECLARE @IdPedido INT;
        DECLARE @Total DECIMAL(10, 2);
        DECLARE @CantidadProductos INT;
        DECLARE @FechaActual DATETIME = GETDATE();
        
        -- Verificar si el cliente existe
        IF NOT EXISTS (SELECT 1 FROM Cliente WHERE Id = @IdCliente)
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'El cliente no existe.';
            RETURN;
        END
        
        -- Verificar si la dirección corresponde al cliente
        IF NOT EXISTS (SELECT 1 FROM Direcciones WHERE Id = @IdDireccion AND Cliente = @IdCliente)
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'La dirección no corresponde al cliente o no existe.';
            RETURN;
        END
        
        -- Verificar si el método de pago existe
        IF NOT EXISTS (SELECT 1 FROM MetodoPago WHERE Id = @IdMetodoPago)
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'El método de pago no existe.';
            RETURN;
        END
        
        -- Verificar si el transportista existe
        IF NOT EXISTS (SELECT 1 FROM Transportista WHERE Id = @IdTransportista)
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'El transportista seleccionado no existe.';
            RETURN;
        END
        
        -- Buscar el carrito activo del cliente
        SELECT @IdCarrito = Id
        FROM Carrito
        WHERE Cliente = @IdCliente AND Activo = 1;
        
        IF @IdCarrito IS NULL
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'No se encontró un carrito activo para el cliente.';
            RETURN;
        END
        
        -- Verificar que el carrito tenga productos
        SELECT @CantidadProductos = COUNT(*), @Total = SUM(Subtotal)
        FROM DetalleCarrito
        WHERE Carrito = @IdCarrito;
        
        IF @CantidadProductos = 0 OR @Total IS NULL
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'El carrito está vacío.';
            RETURN;
        END
        
        -- Verificar stock disponible para todos los productos
        IF EXISTS (
            SELECT 1
            FROM DetalleCarrito dc
            INNER JOIN Producto p ON dc.Producto = p.Id
            WHERE dc.Carrito = @IdCarrito AND dc.Cantidad > p.Stock
        )
        BEGIN
            SET @Resultado = 0;
            SET @Mensaje = 'Uno o más productos no tienen suficiente stock disponible.';
            RETURN;
        END
        
        -- Iniciar transacción para mantener la integridad de datos
        BEGIN TRANSACTION;
        
        -- Crear el pedido
        INSERT INTO Pedidos (
            Cliente, 
            FechaPedido, 
            Importe, 
            Estado, 
            MetodoPago, 
            Direccion
        )
        VALUES (
            @IdCliente,
            @FechaActual,
            @Total,
            'Pendiente',  -- Estado inicial del pedido
            @IdMetodoPago,
            @IdDireccion
        );
        
        -- Obtener el ID del pedido recién creado
        SET @IdPedido = SCOPE_IDENTITY();
        
        -- Crear el envío asociado al pedido
        INSERT INTO Envios (
            Pedido,
            Transportista,
            FechaEnvio,
            Estado
        )
        VALUES (
            @IdPedido,
            @IdTransportista,
            NULL,  -- La fecha de envío se establece cuando se realiza el envío
            'Pendiente'  -- Estado inicial del envío
        );
        
        -- Transferir los productos del carrito al detalle de pedido
        INSERT INTO DetallePedidos (
            Pedido, 
            Producto, 
            Cantidad, 
            Precio,
            Subtotal
        )
        SELECT 
            @IdPedido,
            dc.Producto,
            dc.Cantidad,
            dc.Precio,
            dc.Subtotal
        FROM DetalleCarrito dc
        WHERE dc.Carrito = @IdCarrito;
        
        -- Actualizar el stock de los productos
        UPDATE p
        SET p.Stock = p.Stock - dc.Cantidad
        FROM Producto p
        INNER JOIN DetalleCarrito dc ON p.Id = dc.Producto
        WHERE dc.Carrito = @IdCarrito;
        
        -- Eliminar los detalles del carrito
        DELETE FROM DetalleCarrito
        WHERE Carrito = @IdCarrito;
        
        -- Marcar el carrito como inactivo
        UPDATE Carrito
        SET Activo = 0
        WHERE Id = @IdCarrito;
        
        -- Confirmar la transacción
        COMMIT TRANSACTION;
        
        SET @Resultado = 1;
        SET @Mensaje = 'Pedido creado correctamente con el ID: ' + CAST(@IdPedido AS VARCHAR);
    END TRY
    BEGIN CATCH
        -- Si hay error, revertir todos los cambios
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        SET @Resultado = 0;
        SET @Mensaje = 'Error al confirmar el carrito: ' + ERROR_MESSAGE();
    END CATCH
END