EXEC ObtenerClientes 5

ALTER PROCEDURE ObtenerClientes(
	@TipoOrden INT = 0
	/*
    Valores para @TipoOrden:
    0 = Sin orden (orden natural)
    1 = Más compras (descendente)
    2 = Menos compras (ascendente)
    3 = Mayor gasto (descendente)
    4 = Menor gasto (ascendente)
    5 = Más recientes (fecha registro descendente)
    6 = Más antiguos (fecha registro ascendente)
    */
)
AS
BEGIN
	SELECT u.Id, c.Nombre, u.Correo, u.Pwd, c.Telefono, u.FechaRegistro, c.TotalCompras, c.TotalGastado, u.Estado
	FROM Usuario u INNER JOIN Cliente c ON u.Id = c.Id WHERE u.Rol = 1
	ORDER BY 
        CASE @TipoOrden
            WHEN 0 THEN u.Id
            ELSE NULL
        END,
        CASE WHEN @TipoOrden = 1 THEN c.TotalCompras END DESC,
        CASE WHEN @TipoOrden = 2 THEN c.TotalCompras END ASC,
        CASE WHEN @TipoOrden = 3 THEN c.TotalGastado END DESC,
        CASE WHEN @TipoOrden = 4 THEN c.TotalGastado END ASC,
        CASE WHEN @TipoOrden = 5 THEN u.FechaRegistro END DESC,
        CASE WHEN @TipoOrden = 6 THEN u.FechaRegistro END ASC;
END;