create table Transportista (
	Id int identity primary key not null,
	Nombre varchar(50),
	Correo varchar(100),
	Telefono varchar(15),
	Estado bit default 1
)
go
ALTER PROCEDURE GuardarTransportista( 
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
		IF EXISTS (SELECT 1 FROM Transportista WHERE Correo = @Correo)
        BEGIN
            SET @Resultado = 0
            SET @Mensaje = 'Ya existe un transportista con ese correo.'
            RETURN
        END

		INSERT INTO Transportista(Nombre, Correo, Telefono, Estado)
		VALUES (@Nombre, @Correo, @Telefono, @Estado)

		SET @Resultado = 1
		SET @Mensaje = 'Registro exitoso'
	END TRY
	BEGIN CATCH
		SET @Resultado = 0
		SET @Mensaje = ERROR_MESSAGE()
	END CATCH
END
go
ALTER PROCEDURE ActualizarTransportista ( 
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
		IF NOT EXISTS (SELECT 1 FROM Transportista WHERE Id = @Id)
        BEGIN
            SET @Resultado = 0
            SET @Mensaje = 'El transportista no existe.'
            RETURN
        END

		IF EXISTS (
            SELECT 1 FROM Transportista
            WHERE @Correo = @Correo AND Id <> @Id
        )
        BEGIN
            SET @Resultado = 0
            SET @Mensaje = 'Ya existe otro transportista con ese correo.'
            RETURN
        END

		UPDATE Transportista
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
go
CREATE PROCEDURE EliminarTransportista (
	@Id INT,
	@Resultado BIT OUTPUT,
	@Mensaje NVARCHAR(200) OUTPUT
)
AS
BEGIN
	BEGIN TRY
		IF NOT EXISTS (SELECT 1 FROM Transportista WHERE Id = @Id)
        BEGIN
            SET @Resultado = 0
            SET @Mensaje = 'El transportista no existe.'
            RETURN
        END

		IF EXISTS (SELECT 1 FROM Envio WHERE IdTransportista = @Id)
        BEGIN
            SET @Resultado = 0
            SET @Mensaje = 'No se puede eliminar: el transportista esta asociado en algun envío.'
            RETURN
        END

		DELETE FROM Transportista
		WHERE Id = @Id

		SET @Resultado = 1
		SET @Mensaje = 'Transportista eliminado correctamente.'
	END TRY
	BEGIN CATCH
		SET @Resultado = 0
		SET @Mensaje = ERROR_MESSAGE()
	END CATCH
END


CREATE TABLE Estado(
	Id int identity PRIMARY KEY NOT NULL,
	Descripcion varchar(20)
)

CREATE TABLE MetodoPago (
	Id int identity PRIMARY KEY NOT NULL,
	Descripcion varchar(20)
)
CREATE TABLE TipoDireccion (
	Id int identity PRIMARY KEY NOT NULL,
	Descripcion varchar(20)
)


insert into TipoDireccion(Descripcion) values ('Casa')
insert into TipoDireccion(Descripcion) values ('Edificio')
insert into TipoDireccion(Descripcion) values ('Departamento')


select Id, Descripcion from TipoDireccion