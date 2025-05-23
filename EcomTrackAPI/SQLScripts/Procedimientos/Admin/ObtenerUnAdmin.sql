EXEC ObtenerAdministradorPorId 10
CREATE PROCEDURE ObtenerAdministradorPorId(
	@Id INT
)
AS
BEGIN
	SELECT u.Id, a.Nombre, u.Correo, u.Pwd, a.Telefono, u.FechaRegistro,  u.Estado
	FROM Usuario u INNER JOIN Administrador a ON u.Id = a.Id WHERE u.Rol = 2 AND u.Id = @Id
END;