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
		IF EXISTS (SELECT 1 FROM Proveedor WHERE Correo = @Correo)
        BEGIN
            SET @Resultado = 0
            SET @Mensaje = 'Ya existe una proveedor con ese correo.'
            RETURN
        END

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
		IF NOT EXISTS (SELECT 1 FROM Proveedor WHERE Id = @Id)
        BEGIN
            SET @Resultado = 0
            SET @Mensaje = 'El proveedor no existe.'
            RETURN
        END

		IF EXISTS (
            SELECT 1 FROM Proveedor 
            WHERE @Correo = @Correo
        )
        BEGIN
            SET @Resultado = 0
            SET @Mensaje = 'Ya existe otra proveedor con ese correo.'
            RETURN
        END

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
		IF NOT EXISTS (SELECT 1 FROM Proveedor WHERE Id = @Id)
        BEGIN
            SET @Resultado = 0
            SET @Mensaje = 'El proveedor no existe.'
            RETURN
        END

		IF EXISTS (SELECT 1 FROM Producto WHERE Proveedor = @Id)
        BEGIN
            SET @Resultado = 0
            SET @Mensaje = 'No se puede eliminar: el proveedor esta asociado en algunos productos.'
            RETURN
        END

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