



	CREATE TABLE Transportista (
		Id INT IDENTITY PRIMARY KEY NOT NULL,
		Nombre VARCHAR(100),
		Correo VARCHAR(100),
		Telefono VARCHAR(15),
		Estado BIT default 1
	)


	CREATE TABLE Estado(
	Id int PRIMARY KEY NOT NULL,
	Descripcion varchar(20)
);

CREATE TABLE MetodoPago (
	Id int PRIMARY KEY NOT NULL,
	Descripcion varchar(20)
);
CREATE TABLE TipoDireccion (
	Id int PRIMARY KEY NOT NULL,
	Descripcion varchar(20)
);
