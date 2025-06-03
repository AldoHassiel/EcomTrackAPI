CREATE FUNCTION dbo.FechaDateToInt(
    @fecha DATETIME
) RETURNS INT
AS
BEGIN
    RETURN CAST(CONVERT(VARCHAR(8), @fecha, 112) AS INT);
END;
DROP TABLE Envio
DROP TABLE DetallePedido
DROP TABLE Pedido
DROP TABLE Pago

Create TABLE Pedido(
	Id INT IDENTITY PRIMARY KEY NOT NULL,
	IdCliente INT,
	FechaPedido DATETIME DEFAULT GETDATE(),
	FechaPedidoINT INT, 
	Importe DECIMAL(10,2),
	IdEstado INT,
	IdDireccion int,
	FOREIGN KEY (IdCliente) REFERENCES Cliente(Id),
	FOREIGN KEY (IdEstado) REFERENCES Estado(Id),
	FOREIGN KEY (IdDireccion) references Direccion(Id)
)

CREATE TABLE DetallePedido (
	Id INT IDENTITY PRIMARY KEY,
	IdPedido INT,
	IdProducto INT NOT NULL,
	Cantidad INT NOT NULL,
	Precio DECIMAL(10, 2) NOT NULL,
	Subtotal DECIMAL(10, 2) NOT NULL,
	FOREIGN KEY (IdPedido) REFERENCES Pedido(Id),
	FOREIGN KEY (IdProducto) REFERENCES Producto(Id)
)
SELECT * FROM Estado

ALTER PROCEDURE ConfirmarCompra(
    @IdCliente INT,
    @IdTarjeta INT,
    @IdDireccion INT,
	@Resultado BIT OUTPUT,
	@Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        -- Declara las variables
        DECLARE @IdPedido INT;
        DECLARE @FechaPedido DATETIME = GETDATE();
        DECLARE @FechaPedidoINT INT = dbo.FechaDateToInt(@FechaPedido);
		DECLARE @Total DECIMAL(10, 2);
        DECLARE @CantidadProductos INT;

		-- Verifica que haya stock
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
            SET @Mensaje = 'Uno o más productos del carrito no tienen stock suficiente.';
            RETURN;
        END;

		-- Obtiene el total
		 SELECT 
            @Total = ISNULL(SUM(dc.Subtotal), 0),
            @CantidadProductos = ISNULL(SUM(dc.Cantidad), 0)
        FROM DetalleCarrito dc
        INNER JOIN Carrito c ON dc.Carrito = c.Id
        WHERE c.Cliente = @IdCliente AND c.Activo = 1;

		-- Crea el pedido
        INSERT INTO Pedido (IdCliente, FechaPedido, FechaPedidoINT, Importe, IdEstado, IdDireccion)
        VALUES (@IdCliente, @FechaPedido, @FechaPedidoINT, @Total, 3, @IdDireccion);

        -- Obtiene el pedido recien realizado
        SET @IdPedido = SCOPE_IDENTITY();

        -- Inserta los detalles del pedido desde el carrito
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

		-- Limpia el carrito del cliente
        DELETE FROM DetalleCarrito
        WHERE Carrito IN (
            SELECT Id FROM Carrito WHERE Cliente = @IdCliente AND Activo = 1
        );

		-- Inserta el pago en su propia tabla
		INSERT INTO Pago(IdPedido, IdTarjeta, ImporteAbonado, FechaPago)
		VALUES (@IdPedido, @IdTarjeta, @Total, @FechaPedido);

		-- Inserta los datos en envio
		INSERT INTO Envio(FechaEnvio, FechaEntrega, IdPedido, IdTransportista, IdEstado)
		VALUES (NULL, NULL, @IdPedido, NULL, 3);

		SET @Resultado = 1;
		SET @Mensaje = 'Compra realizada con éxito';
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @Resultado = 0;
		SET @Mensaje = ERROR_MESSAGE();
		RETURN;
    END CATCH
END;



GO
select * from envio

ALTER PROCEDURE ActualizarPedido(
	@Id INT,
    @Estado INT
)
AS
BEGIN
	IF @Estado = 2
	BEGIN
		UPDATE Envio
			SET FechaEnvio = GETDATE()
		WHERE IdPedido = @Id
	END

	IF @Estado = 1
	BEGIN
		UPDATE Envio
			SET FechaEntrega = GETDATE()
		WHERE IdPedido = @Id
	END

	UPDATE Pedido
		SET IdEstado = @Estado 
	WHERE Id = @Id

	UPDATE Envio
		SET IdEstado = @Estado 
	WHERE Id = @Id
END

EXEC ActualizarPedido @Id = 3, @Estado = 3
SELECT * FROM Envio
SELECT * FROM Pedido


CREATE TRIGGER ActualizarStock
ON DetallePedido
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE p
    SET p.Stock = p.Stock - i.Cantidad
    FROM Producto p
    INNER JOIN Inserted i ON p.Id = i.IdProducto;

    IF EXISTS (
        SELECT 1
        FROM Producto
        WHERE Stock < 0
    )
    BEGIN
        ROLLBACK TRANSACTION;
    END;
END;

SELECT * FROM Pedido
SELECT * FROM Estado
SELECT * FROM Direccion
SELECT * FROM Producto WHERE Id = 8

EXEC ObtenerPedidosDelCliente @IdCliente = 47
EXEC ObtenerDetallePedidoCliente @IdCliente = 47, @IdPedido = 3

ALTER PROCEDURE ObtenerPedidosDelCliente
    @IdCliente INT
AS
BEGIN
    SELECT 
        p.Id,
        p.FechaPedido,
        p.Importe,
        e.Descripcion AS Estado
    FROM Pedido p
    JOIN Estado e ON p.IdEstado = e.Id
    WHERE p.IdCliente = @IdCliente;
END

ALTER PROCEDURE ObtenerDetallePedidoCliente
    @IdCliente INT,
    @IdPedido INT
AS
BEGIN
	SET NOCOUNT ON;
    -- Productos del pedido
    SELECT 
        prod.Id AS IdProducto,
        prod.Nombre AS NombreProducto,
        dp.Cantidad,
        dp.Precio,
        dp.Subtotal AS SubtotalProducto,
		prod.UrlImagen
    FROM DetallePedido dp
    INNER JOIN Producto prod ON dp.IdProducto = prod.Id
    INNER JOIN Pedido p ON dp.IdPedido = p.Id
    WHERE p.IdCliente = @IdCliente
      AND p.Id = @IdPedido;
   -- Tarjeta 
	SELECT 
		t.NumeroTarjeta,
		mp.Descripcion AS MetodoDePago,
		t.MarcaTarjeta
	FROM Pago p
	INNER JOIN Tarjeta t ON t.Id = p.IdTarjeta
	INNER JOIN MetodoPago mp ON mp.Id = t.MetodoDePago
	WHERE IdPedido = @IdPedido
	
    -- Dirección
    SELECT 
        td.Descripcion AS NombreTipoDireccion,
        d.Calle,
        d.Numero,
        d.EntreCalles,
        d.Colonia,
        d.Ciudad,
        es.Nombre AS EstadoDireccion,
        d.CodigoPostal
    FROM Pedido p
    INNER JOIN Direccion d ON p.IdDireccion = d.Id
    INNER JOIN TipoDireccion td ON d.TipoDireccion = td.Id
    INNER JOIN Estados es ON d.IdEstado = es.Id
    WHERE p.Id = @IdPedido;

    -- Estado del pedido, importe y transportista
	SELECT 
		e.Descripcion AS EstadoPedido,
		p.Importe,
		t.Nombre AS Transportista
	FROM Pedido p
	INNER JOIN Estado e ON p.IdEstado = e.Id
	LEFT JOIN Envio en ON en.IdPedido = p.Id
	LEFT JOIN Transportista t ON t.Id = en.IdTransportista
	WHERE p.Id = 3;

	-- Transportista
	SELECT * FROM Envio
END

-- SOLO DE PRUEBA: NO VA EN LA API
CREATE PROCEDURE EliminarPedidoCompleto
    @IdPedido INT
AS
BEGIN
    DELETE FROM DetallePedido WHERE IdPedido = @IdPedido;

    DELETE FROM Envio WHERE IdPedido = @IdPedido;

    DELETE FROM Pago WHERE IdPedido = @IdPedido;

    DELETE FROM Pedido WHERE Id = @IdPedido;
END