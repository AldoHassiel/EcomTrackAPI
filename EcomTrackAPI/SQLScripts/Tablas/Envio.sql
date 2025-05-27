	
	
	create table Envio (
		Id int identity primary key not null,
		FechaEnvio datetime default getdate(),
		IdPedido int,
		IdTransportista int, 
		IdEstado int,
		foreign key (IdPedido) references Pedidos(Id),
		foreign key (IdTransportista) references Transportista(Id),
		foreign key (IdEstado) references Estado(Id)
	)