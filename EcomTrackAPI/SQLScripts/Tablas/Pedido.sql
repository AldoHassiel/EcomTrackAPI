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

EXEC ActualizarPedido @Id = 3, @Estado = 1
SELECT * FROM Envio
SELECT * FROM Pedido


DECLARE @Resultado BIT;
DECLARE @Mensaje VARCHAR(500);

EXEC ConfirmarCompra
    @IdCliente = 47,
    @IdTarjeta = 49,
    @IdDireccion = 12,
    @Resultado = @Resultado OUTPUT,
    @Mensaje = @Mensaje OUTPUT;

-- Ver los resultados
SELECT @Resultado AS Resultado, @Mensaje AS Mensaje;

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
SELECT * FROM Producto WHERE Id = 8