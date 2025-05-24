CREATE TABLE Producto(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	Nombre VARCHAR(100) NOT NULL,
	Descripcion VARCHAR(100) NOT NULL,
	Categoria INT NOT NULL,
	Precio DECIMAL(10,2) NOT NULL,
	Stock INT NOT NULL DEFAULT 0,
	Proveedor INT NOT NULL,
	UrlImagen NVARCHAR(255) NOT NULL,
	FechaRegistro DATETIME DEFAULT GETDATE(),
	Estado BIT DEFAULT 1,
	FOREIGN KEY(Categoria) REFERENCES Categoria(Id),
	FOREIGN KEY(Proveedor) REFERENCES Proveedor(Id)
);

CREATE PROCEDURE GuardarProducto(
	@Nombre VARCHAR(100),
	@Descripcion VARCHAR(100),
	@Categoria INT,
	@Precio DECIMAL(10,2),
	@Stock INT,
	@Proveedor INT,
	@UrlImagen NVARCHAR(255),
	@Estado BIT,
	@Resultado BIT OUTPUT,
	@Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
	BEGIN TRY
		IF EXISTS (SELECT 1 FROM Producto WHERE Nombre = @Nombre)
        BEGIN
            SET @Resultado = 0
            SET @Mensaje = 'Ya existe un producto con ese nombre'
            RETURN
        END
		
		IF NOT EXISTS (SELECT 1 FROM Categoria WHERE Id = @Categoria)
        BEGIN
            SET @Resultado = 0
            SET @Mensaje = 'No existe la categoría'
            RETURN
        END

		IF NOT EXISTS (SELECT 1 FROM Proveedor WHERE Id = @Proveedor)
        BEGIN
            SET @Resultado = 0
            SET @Mensaje = 'No existe el proveedor'
            RETURN
        END

		INSERT INTO Producto(Nombre, Descripcion, Categoria, Precio, Stock, Proveedor, UrlImagen, FechaRegistro, Estado)
		VALUES (@Nombre, @Descripcion, @Categoria, @Precio, @Stock, @Proveedor, @UrlImagen, GETDATE(), @Estado)

		SET @Resultado = 1
		SET @Mensaje = 'Registro exitoso'

	END TRY
	BEGIN CATCH
		SET @Resultado = 0
		SET @Mensaje = ERROR_MESSAGE()
	END CATCH
END

CREATE PROCEDURE ActualizarProducto(
	@Id INT,
	@Nombre VARCHAR(100),
	@Descripcion VARCHAR(100),
	@Categoria INT,
	@Precio DECIMAL(10,2),
	@Stock INT,
	@Proveedor INT,
	@UrlImagen NVARCHAR(255),
	@Estado BIT,
	@Resultado BIT OUTPUT,
	@Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
	BEGIN TRY
		IF NOT EXISTS (SELECT 1 FROM Producto WHERE Id = @Id)
		BEGIN
			SET @Resultado = 0
			SET @Mensaje = 'El producto no existe'
			RETURN
		END

		IF EXISTS (SELECT 1 FROM Producto WHERE Nombre = @Nombre AND Id <> @Id)
		BEGIN
			SET @Resultado = 0
			SET @Mensaje = 'Ya existe otro producto con ese nombre'
			RETURN
		END

		IF NOT EXISTS (SELECT 1 FROM Categoria WHERE Id = @Categoria)
        BEGIN
            SET @Resultado = 0
            SET @Mensaje = 'No existe la categoría'
            RETURN
        END

        IF NOT EXISTS (SELECT 1 FROM Proveedor WHERE Id = @Proveedor)
        BEGIN
            SET @Resultado = 0
            SET @Mensaje = 'No existe el proveedor'
            RETURN
        END

		UPDATE Producto
		SET Nombre = @Nombre,
			Descripcion = @Descripcion,
			Categoria = @Categoria,
			Precio = @Precio,
			Stock = @Stock,
			Proveedor = @Proveedor,
			UrlImagen = @UrlImagen,
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

CREATE PROCEDURE EliminarProducto(
	@Id INT,
	@Resultado BIT OUTPUT,
	@Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
	BEGIN TRY
		IF NOT EXISTS (SELECT 1 FROM Producto WHERE Id = @Id)
		BEGIN
			SET @Resultado = 0
			SET @Mensaje = 'El producto no existe'
			RETURN
		END

		DELETE FROM Producto WHERE Id = @Id

		SET @Resultado = 1
		SET @Mensaje = 'Producto eliminado exitosamente'
	END TRY
	BEGIN CATCH
		SET @Resultado = 0
		SET @Mensaje = ERROR_MESSAGE()
	END CATCH
END

ALTER PROCEDURE ObtenerProductosFiltro
    @IdCategoria INT = 0,
    @IdProveedor INT = 0,
	@Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
	BEGIN TRY
		SELECT 
			P.Id,
			P.Nombre,
			P.Descripcion,
			P.Categoria AS IdCategoria,
			C.Nombre AS CategoriaNombre,
			P.Precio,
			P.Stock,
			P.Proveedor AS IdProveedor,
			PR.Nombre AS ProveedorNombre,
			P.UrlImagen,
			P.FechaRegistro,
			P.Estado
		FROM Producto P
		INNER JOIN Categoria C ON C.Id = P.Categoria
		INNER JOIN Proveedor PR ON PR.Id = P.Proveedor
		WHERE 
			(@IdCategoria = 0 OR P.Categoria = @IdCategoria) AND
			(@IdCategoria = 0 OR P.Proveedor = @IdCategoria)

		SET @Resultado = 1
		SET @Mensaje = 'Productos obtenidos correctamente'
	END TRY
	BEGIN CATCH
		SET @Resultado = 0
        SET @Mensaje = ERROR_MESSAGE()
	END CATCH
END