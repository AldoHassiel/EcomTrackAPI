CREATE TABLE Pago(
	Id INT IDENTITY PRIMARY KEY NOT NULL,
	IdPedido INT,
	IdTarjeta INT, --Tarjeta tiene metodo de pago
	ImporteAbonado DECIMAL(10,2),
	FechaPago DATETIME DEFAULT GETDATE(),
	FOREIGN KEY (IdPedido) REFERENCES Pedido(Id),
	FOREIGN KEY (IdTarjeta) REFERENCES Tarjeta(Id),
)