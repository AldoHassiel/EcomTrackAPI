ALTER PROCEDURE ObtenerVentasPeriodo(
	@FechaInicio INT,
	@FechaFin INT,
	@Estado INT
)
AS
BEGIN
	-- El 0 invalida las condiciones
	SELECT
		SUM(Importe) AS TotalVenta,
		COUNT(Id) AS TotalPedidos,
		ROUND(AVG(Importe), 2) AS Promedio
	FROM Pedido
	WHERE
        (@Estado = 0 OR IdEstado = @Estado) AND
        (
            (@FechaInicio = 0 AND @FechaFin = 0)
            OR (FechaPedidoINT BETWEEN @FechaInicio AND @FechaFin)
        )

	SELECT
		p.Id AS IdPedido,
		c.Id AS IdCliente,
		c.Nombre AS NombreCliente,
		p.FechaPedido,
		p.Importe AS Total,
		t.MarcaTarjeta,
		es.Descripcion AS Estado
	FROM Pedido p
	INNER JOIN Cliente c ON c.Id = p.IdCliente
	INNER JOIN Pago pa ON pa.IdPedido = p.Id
	INNER JOIN Tarjeta t ON t.Id = pa.IdTarjeta
	INNER JOIN Estado es ON es.Id = p.IdEstado
	WHERE (@Estado = 0 OR p.IdEstado = @Estado) AND
        (
            (@FechaInicio = 0 AND @FechaFin = 0)
            OR (p.FechaPedidoINT BETWEEN @FechaInicio AND @FechaFin)
        )
	ORDER BY p.FechaPedido ASC
END


SELECT * FROM Pedido
SELECT * FROM Envio
