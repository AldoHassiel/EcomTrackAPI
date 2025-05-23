EXEC IniciarSesion @Correo = 'aldo.acosta@ecomtrack.com', @Pwd = 'admin'
EXEC IniciarSesion @Correo = 'juan.perez@ejemplo.com', @Pwd = 'qwertys'

CREATE PROCEDURE IniciarSesion(
	@Correo VARCHAR(100),
	@Pwd VARCHAR(50)
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Autenticado BIT = 0;
	DECLARE @Mensaje VARCHAR(100) = 'Datos incorrectos';
	DECLARE @UsuarioId INT;
	DECLARE @RolId INT;
	DECLARE @Estado BIT;
	DECLARE @FechaRegistro DATETIME;

	SELECT @UsuarioId = Id, @RolId = Rol, @Estado = Estado, @FechaRegistro = FechaRegistro
	FROM Usuario WHERE Correo = @Correo AND Pwd COLLATE SQL_Latin1_General_CP1_CS_AS = @Pwd

	IF @UsuarioId IS NOT NULL
	BEGIN
		SET @Autenticado = 1;
        SET @Mensaje = 'Inicio de sesión exitoso';

		IF @Estado = 0
        BEGIN
            SET @Autenticado = 0;
            SET @Mensaje = 'Usuario inactivo';
        END
	END

	SELECT 
        @Autenticado AS Autenticado,
        @Mensaje AS Mensaje,
		ISNULL(@UsuarioId, -1) AS IdUsuario,
        CASE WHEN @RolId = 2 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS EsAdmin;
END;