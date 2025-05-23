EXEC ObtenerClientePorId @Id = 1

CREATE PROCEDURE ObtenerClientePorId(
	@Id INT
)
AS
BEGIN
	SELECT u.Id, c.Nombre, u.Correo, u.Pwd, c.Telefono, u.FechaRegistro, c.TotalCompras, c.TotalGastado, u.Estado
	FROM Usuario u INNER JOIN Cliente c ON u.Id = c.Id WHERE u.Rol = 1 AND u.Id = @Id
END;