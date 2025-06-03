CREATE TABLE Envio(
	Id int identity primary key not null,
	FechaEnvio datetime,
	FechaEntrega datetime,
	IdPedido int,
	IdTransportista int, 
	IdEstado int,
	foreign key (IdPedido) references Pedido(Id),
	foreign key (IdTransportista) references Transportista(Id),
	foreign key (IdEstado) references Estado(Id)
)


select * from envio
select * from estado
select * from estados

go

ALTER PROCEDURE ObtenerEstadoPorEnvio
(
    @IdEnvio INT,
    @Resultado BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        SELECT 
            ev.Id AS IdEnvio,
            ev.IdEstado,
            es.Descripcion AS EstadoDescripcion
        FROM 
            Envio ev
        JOIN 
            Estado es ON ev.IdEstado = es.Id
        WHERE 
            ev.Id = @IdEnvio;

        SET @Resultado = 1;
        SET @Mensaje = 'Estado de envío obtenido correctamente';
    END TRY
    BEGIN CATCH
        SET @Resultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END;


EXEC ObtenerEstadoPorEnvio @EnvioId = 3;

SELECT
 es.Descripcion
FROM Envio e
INNER JOIN Estado es
ON es.Id = e.IdEstado
WHERE e.Id = 3

select * from Envio
