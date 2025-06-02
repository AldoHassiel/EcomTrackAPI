CREATE TABLE Direccion(
    Id INT IDENTITY PRIMARY KEY NOT NULL,
    IdCliente INT NOT NULL,
    TipoDireccion INT NOT NULL,
    EsPredeterminado BIT DEFAULT 1,
    Calle VARCHAR(100) NOT NULL,
    Numero VARCHAR(10) NOT NULL,
    EntreCalles VARCHAR(200) NOT NULL,
    Colonia VARCHAR(100) NOT NULL,
    Ciudad VARCHAR(100) NOT NULL,
    IdEstado INT NOT NULL,
    CodigoPostal VARCHAR(10) NOT NULL,
    EstaActiva BIT DEFAULT 1 NOT NULL,
    FOREIGN KEY (IdCliente) REFERENCES Cliente(Id),
    FOREIGN KEY (TipoDireccion) REFERENCES TipoDireccion(Id),
    FOREIGN KEY (IdEstado) REFERENCES Estados(Id)
);

CREATE PROCEDURE ObtenerDirecciones
    @IdCliente INT
AS
BEGIN
    SELECT
        d.Id,
        d.TipoDireccion AS IdTipoDireccion,
        td.Descripcion AS TipoDireccionNombre,
        d.Calle,
        d.Numero,
        d.EntreCalles,
        d.Colonia,
        d.Ciudad,
        d.IdEstado,
        e.Nombre AS EstadoNombre,
        d.CodigoPostal,
        d.EsPredeterminado,
        d.EstaActiva
    FROM Direccion d
    INNER JOIN TipoDireccion td ON d.TipoDireccion = td.Id
    INNER JOIN Estados e ON d.IdEstado = e.Id
    WHERE d.IdCliente = @IdCliente AND d.EstaActiva = 1
    ORDER BY d.EsPredeterminado DESC, d.Id ASC
END

EXEC ObtenerDirecciones 47

CREATE PROCEDURE AgregarDireccion
    @IdCliente INT,
    @TipoDireccion INT,
    @Calle VARCHAR(100),
    @Numero VARCHAR(10),
    @EntreCalles VARCHAR(200),
    @Colonia VARCHAR(100),
    @Ciudad VARCHAR(100),
    @IdEstado INT,
    @CodigoPostal VARCHAR(10)
AS
BEGIN
    UPDATE Direccion
		SET EsPredeterminado = 0
    WHERE IdCliente = @IdCliente;

    INSERT INTO Direccion (
        IdCliente, TipoDireccion, EsPredeterminado, Calle, Numero, EntreCalles, Colonia, Ciudad, IdEstado, CodigoPostal, EstaActiva
    ) VALUES (
        @IdCliente, @TipoDireccion, 1, @Calle, @Numero, @EntreCalles, @Colonia, @Ciudad, @IdEstado, @CodigoPostal, 1
    );
END

CREATE PROCEDURE EditarDireccion
    @Id INT,
    @IdCliente INT,
    @TipoDireccion INT,
    @Calle VARCHAR(100),
    @Numero VARCHAR(10),
    @EntreCalles VARCHAR(200),
    @Colonia VARCHAR(100),
    @Ciudad VARCHAR(100),
    @IdEstado INT,
    @CodigoPostal VARCHAR(10),
    @EsPredeterminado BIT
AS
BEGIN

    IF @EsPredeterminado = 1
    BEGIN
        UPDATE Direccion
        SET EsPredeterminado = 0
        WHERE IdCliente = @IdCliente;
    END

    UPDATE Direccion
    SET
        TipoDireccion = @TipoDireccion,
        Calle = @Calle,
        Numero = @Numero,
        EntreCalles = @EntreCalles,
        Colonia = @Colonia,
        Ciudad = @Ciudad,
        IdEstado = @IdEstado,
        CodigoPostal = @CodigoPostal,
        EsPredeterminado = @EsPredeterminado
    WHERE Id = @Id AND IdCliente = @IdCliente;
END

CREATE PROCEDURE EliminarDireccion
    @Id INT,
    @IdCliente INT
AS
BEGIN
    UPDATE Direccion
    SET EstaActiva = 0
    WHERE Id = @Id AND IdCliente = @IdCliente;
END