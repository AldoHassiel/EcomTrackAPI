SELECT * FROM Categoria

CREATE TABLE Categoria (
	Id INT IDENTITY PRIMARY KEY NOT NULL,
	Nombre VARCHAR(50) NOT NULL,
	Descripcion varchar(100) NOT NULL,
	CantidadProducto INT DEFAULT 0,
	Estado BIT DEFAULT 1
)

ALTER PROCEDURE GuardarCategoria (
    @Id INT, 
    @Nombre VARCHAR(50), 
    @Descripcion VARCHAR(100),
    @Estado BIT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM Categoria WHERE Nombre = @Nombre)
        BEGIN
            SET @Resultado = 0
            SET @Mensaje = 'Ya existe una categoría con ese nombre.'
            RETURN
        END

        INSERT INTO Categoria (Nombre, Descripcion, Estado)
        VALUES (@Nombre, @Descripcion, @Estado)

        SET @Resultado = 1
        SET @Mensaje = 'Registro exitoso.'
    END TRY
    BEGIN CATCH
        SET @Resultado = 0
        SET @Mensaje = ERROR_MESSAGE()
    END CATCH
END


ALTER PROCEDURE ActualizarCategoria (
    @Id INT, 
    @Nombre VARCHAR(50), 
    @Descripcion VARCHAR(100),
    @Estado BIT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Categoria WHERE Id = @Id)
        BEGIN
            SET @Resultado = 0
            SET @Mensaje = 'La categoría no existe.'
            RETURN
        END

        IF EXISTS (
            SELECT 1 FROM Categoria 
            WHERE Nombre = @Nombre AND Id <> @Id
        )
        BEGIN
            SET @Resultado = 0
            SET @Mensaje = 'Ya existe otra categoría con ese nombre.'
            RETURN
        END

        UPDATE Categoria
        SET 
            Nombre = @Nombre, 
            Descripcion = @Descripcion,
            Estado = @Estado
        WHERE Id = @Id

        SET @Resultado = 1
        SET @Mensaje = 'Actualización exitosa.'
    END TRY
    BEGIN CATCH
        SET @Resultado = 0
        SET @Mensaje = ERROR_MESSAGE()
    END CATCH
END


CREATE PROCEDURE EliminarCategoria (
    @Id INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Categoria WHERE Id = @Id)
        BEGIN
            SET @Resultado = 0
            SET @Mensaje = 'La categoría no existe.'
            RETURN
        END

        IF EXISTS (SELECT 1 FROM Producto WHERE CategoriaId = @Id)
        BEGIN
            SET @Resultado = 0
            SET @Mensaje = 'No se puede eliminar: hay productos asociados a esta categoría.'
            RETURN
        END

        DELETE FROM Categoria WHERE Id = @Id

        SET @Resultado = 1
        SET @Mensaje = 'Categoría eliminada correctamente.'
    END TRY
    BEGIN CATCH
        SET @Resultado = 0
        SET @Mensaje = ERROR_MESSAGE()
    END CATCH
END
