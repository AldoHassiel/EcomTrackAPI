select * from Envio

SELECT e.Id, e.FechaEnvio, e.FechaEntrega, e.IdPedido, e.IdTransportista,
                t.Nombre AS NombreTransportista, e.IdEstado, es.Descripcion AS EstadoDescripcion 
                FROM Envio e LEFT OUTER JOIN Transportista t ON e.IdTransportista = t.Id 
                LEFT OUTER JOIN Estado es ON e.IdEstado = es.Id;;
				
select * from Pedido