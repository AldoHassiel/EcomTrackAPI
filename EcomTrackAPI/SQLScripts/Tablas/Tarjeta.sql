--Tarjetas FALTAN LOS PROCEDIMIENTOS DE ALMACENADO NO ESTÁ EJECUTADA
CREATE TABLE Tarjeta(
	Id INT IDENTITY PRIMARY KEY NOT NULL,
	Cliente INT,
	NumeroTarjeta varchar(16),
	MetodoDePago INT,
	MarcaTarjeta varchar(100),
	Activa bit,
	Predeterminada bit,
	FOREIGN KEY (Cliente) REFERENCES Cliente(Id),
	FOREIGN KEY (MetodoDePago) REFERENCES MetodoPago(Id)
)