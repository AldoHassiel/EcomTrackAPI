CREATE TRIGGER trg_ActualizarCantidadProductosInsert
ON Producto
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Categoria
    SET CantidadProducto = ISNULL(CantidadProducto, 0) + 1
    WHERE Id IN (SELECT Categoria FROM inserted);
END;
GO


CREATE TRIGGER trg_ActualizarCantidadProductosDelete
ON Producto
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Categoria
    SET CantidadProducto = ISNULL(CantidadProducto, 0) - 1
    WHERE Id IN (SELECT Categoria FROM deleted);
END;
GO


CREATE TRIGGER trg_ActualizarCantidadProductosUpdate
ON Producto
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    IF UPDATE(Categoria)
    BEGIN
        UPDATE Categoria
        SET CantidadProducto = ISNULL(CantidadProducto, 0) - 1
        WHERE Id IN (SELECT Categoria FROM deleted);
        
        UPDATE Categoria
        SET CantidadProducto = ISNULL(CantidadProducto, 0) + 1
        WHERE Id IN (SELECT Categoria FROM inserted);
    END
END;
GO


CREATE TRIGGER trg_ActualizarEstadisticasCliente
ON Pedido
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Cliente
    SET 
        TotalCompras = ISNULL(TotalCompras, 0) + 1,
        TotalGastado = ISNULL(TotalGastado, 0) + i.Importe
    FROM Cliente c
    INNER JOIN inserted i ON c.Id = i.IdCliente;
END;
GO



CREATE TRIGGER trg_VerificarStockCarrito
ON DetalleCarrito
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    IF EXISTS (
        SELECT 1
        FROM inserted i
        INNER JOIN Producto p ON i.Producto = p.Id
        WHERE i.Cantidad > p.Stock
    )
    BEGIN
        RAISERROR('No hay suficiente stock para uno o más productos', 16, 1);
        RETURN;
    END
    
    INSERT INTO DetalleCarrito (Carrito, Producto, Cantidad, Precio)
    SELECT Carrito, Producto, Cantidad, Precio
    FROM inserted;
END;
GO


CREATE TRIGGER trg_ActualizarStockPedido
ON DetallePedido
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Producto
    SET Stock = Stock - i.Cantidad
    FROM Producto p
    INNER JOIN inserted i ON p.Id = i.IdProducto;
END;
GO


CREATE TRIGGER trg_AuditoriaUpdateProducto
ON Producto
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO AuditoriaProducto (
        IdProducto, 
        FechaUpdate, 
        Usuario, 
        CampoModificado, 
        ValorAnterior, 
        ValorNuevo
    )
    SELECT 
        d.Id,
        GETDATE(),
        SYSTEM_USER,
        CASE 
            WHEN d.Nombre <> i.Nombre THEN 'Nombre'
            WHEN d.Descripcion <> i.Descripcion THEN 'Descripción'
            WHEN d.Precio <> i.Precio THEN 'Precio'
            WHEN d.Stock <> i.Stock THEN 'Stock'
            WHEN d.Estado <> i.Estado THEN 'Estado'
            ELSE 'Otros'
        END,
        CASE 
            WHEN d.Nombre <> i.Nombre THEN d.Nombre
            WHEN d.Descripcion <> i.Descripcion THEN d.Descripcion
            WHEN d.Precio <> i.Precio THEN CAST(d.Precio AS VARCHAR(50))
            WHEN d.Stock <> i.Stock THEN CAST(d.Stock AS VARCHAR(50))
            WHEN d.Estado <> i.Estado THEN CAST(d.Estado AS VARCHAR(50))
            ELSE NULL
        END,
        CASE 
            WHEN d.Nombre <> i.Nombre THEN i.Nombre
            WHEN d.Descripcion <> i.Descripcion THEN i.Descripcion
            WHEN d.Precio <> i.Precio THEN CAST(i.Precio AS VARCHAR(50))
            WHEN d.Stock <> i.Stock THEN CAST(i.Stock AS VARCHAR(50))
            WHEN d.Estado <> i.Estado THEN CAST(i.Estado AS VARCHAR(50))
            ELSE NULL
        END
    FROM deleted d
    INNER JOIN inserted i ON d.Id = i.Id
    WHERE 
        d.Nombre <> i.Nombre OR
        d.Descripcion <> i.Descripcion OR
        d.Precio <> i.Precio OR
        d.Stock <> i.Stock OR
        d.Estado <> i.Estado;
END;
GO