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

CREATE PROCEDURE ObtenerEstadoPorEnvio
    @EnvioId INT
AS
BEGIN
    SELECT 
        es.Descripcion
    FROM 
        Envio e
    INNER JOIN 
        Estado es
    ON 
        es.Id = e.IdEstado
    WHERE 
        e.Id = @EnvioId;
END;

EXEC ObtenerEstadoPorEnvio @EnvioId = 3;

SELECT
 es.Descripcion
FROM Envio e
INNER JOIN Estado es
ON es.Id = e.IdEstado
WHERE e.Id = 3

