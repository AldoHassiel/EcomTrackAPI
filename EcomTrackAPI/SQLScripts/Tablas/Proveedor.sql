create table Proveedor (
	Id int identity primary key not null,
	Nombre varchar(50),
	Correo varchar(100),
	Telefono varchar(15),
	Estado bit default 1
)

CREATE PROCEDURE GuardarProveedor( 
	@Id INT,
	@Nombre VARCHAR(50), 
	@Correo VARCHAR(100),
	@Telefono VARCHAR(15),
	@Estado BIT,
	@Resultado BIT OUTPUT,
	@Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
	BEGIN TRY
		INSERT INTO Proveedor (Nombre, Correo, Telefono, Estado)
		VALUES (@Nombre, @Correo, @Telefono, @Estado)

		SET @Resultado = 1
		SET @Mensaje = 'Registro exitoso'
	END TRY
	BEGIN CATCH
		SET @Resultado = 0
		SET @Mensaje = ERROR_MESSAGE()
	END CATCH
END

CREATE PROCEDURE ActualizarProveedor ( 
	@Id INT,
	@Nombre VARCHAR(50), 
	@Correo VARCHAR(100),
	@Telefono VARCHAR(15),
	@Estado BIT,
	@Resultado BIT OUTPUT,
	@Mensaje VARCHAR(200) OUTPUT
)
AS
BEGIN
	BEGIN TRY
		UPDATE Proveedor
		SET 
			Nombre = @Nombre,
			Correo = @Correo,
			Telefono = @Telefono,
			Estado = @Estado
		WHERE Id = @Id

		SET @Resultado = 1
		SET @Mensaje = 'Actualización exitosa'
	END TRY
	BEGIN CATCH
		SET @Resultado = 0
		SET @Mensaje = ERROR_MESSAGE()
	END CATCH
END

CREATE PROCEDURE EliminarProveedor (
	@Id INT,
	@Resultado BIT OUTPUT,
	@Mensaje NVARCHAR(200) OUTPUT
)
AS
BEGIN
	BEGIN TRY
		DELETE FROM Proveedor
		WHERE Id = @Id

		SET @Resultado = 1
		SET @Mensaje = 'Proveedor eliminado correctamente.'
	END TRY
	BEGIN CATCH
		SET @Resultado = 0
		SET @Mensaje = ERROR_MESSAGE()
	END CATCH
END

CREATE PROCEDURE EliminarProveedor (
	@Id INT,
	@Resultado BIT OUTPUT,
	@Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
	BEGIN TRY
		DELETE FROM Proveedor
		WHERE Id = @Id

		SET @Resultado = 1
		SET @Mensaje = 'Eliminado'
	END TRY
	BEGIN CATCH
		SET @Resultado = 0
		SET @Mensaje = ERROR_MESSAGE()
	END CATCH
END
